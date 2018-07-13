using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace nat_geo.net
{
    class Program
    {
        // Return string between two strings :)
        private static string Between(string theString, string sThis, string sThat)
        {
            int pFrom = theString.IndexOf(sThis) + sThis.Length;
            int pTo = theString.Substring(pFrom).IndexOf(sThat);

            return theString.Substring(pFrom, pTo - pFrom);
        }

        // C# Slug function: Props to Kamran Ayub with my small changes
        // http://predicatet.blogspot.com/2009/04/improved-c-slug-generator-or-how-to.html
        private static string GenerateSlug(this string phrase)
        {
            phrase = Regex.Replace(phrase, @"[^a-z0-9\s-]", ""); // invalid chars           
            phrase = Regex.Replace(phrase, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            phrase = Regex.Replace(phrase, @"\s", "-"); // hyphens   

            return phrase.ToLower();
        }

        static void Main(string[] args)
        {
            // First get data from the picture of the day page (which is not the actual image)
            const string URL = "https://www.nationalgeographic.com/photography/photo-of-the-day/";
            WebClient webReader = new WebClient();
            string rawHTML = webReader.DownloadString(URL);

            // Scrub the raw data to find the actual picture of the day URL
            string picURL = Between(rawHTML, "<meta property=\"og:image\" content=", "\"");

            // Scrub again and get the title which will be used to name fthe filename
            string title = Between(rawHTML, "<meta property=\"og:title\" content=\"", "\"");
            string userprofileFolder = Environment.GetEnvironmentVariable("USERPROFILE");
            string fileName = userprofileFolder + GenerateSlug(title) + ".jpg";

            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            // Download the Web resource and save it into the current filesystem folder.
            webReader.DownloadFile(picURL, fileName);
        }
    }
}
