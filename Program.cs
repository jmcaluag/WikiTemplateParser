using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace WikiTemplateParser
{
    class Program
    {
        public static List<Episode> seasonList = new List<Episode>();

        static void Main(string[] args)
        {
            string folderPath = @"C:\Users\Mikey\SoftDev_Projects\CSharp_Projects\WikiEntertainment\WikiTemplateFile\";

            using(StreamReader reader = File.OpenText(folderPath + "WikiTemplateSeason.txt"))
            {
                Boolean collectStatus = false; //Ignores all lines until "{{Episode list"
                while(reader.Peek() > -1)

                {
                    string currentLine = reader.ReadLine();
                    
                    if(FindEpisode(currentLine))
                    {
                        seasonList.Add(new Episode());
                        collectStatus = true;
                    }
                    else if(FindEndEpisode(currentLine))
                    {
                        collectStatus = false;
                    }
                    else if (collectStatus)
                    {
                        CollectEpisodeDetails(seasonList[seasonList.Count -1], currentLine);
                    }
                }

            }

            Console.WriteLine("Testing for stability: {0}", seasonList.Count);
            Console.Write("Enter episode number: ");
            string episodeInput = Console.ReadLine();
            ShowMeEpisodeDetails(Convert.ToInt32(episodeInput) - 1);
            
        }

        //Finds an episode Block
        public static Boolean FindEpisode(string wikiTemplateLine)
        {
            Boolean episodeTemplate = false;

            episodeTemplate = wikiTemplateLine.Contains("{{Episode list");

            return episodeTemplate;
        }

        //Finds the end of the episode block
        public static Boolean FindEndEpisode(string wikiTemplateLine)
        {
            Boolean episodeTemplate = false;

            //Avoids ending the episode at the OriginalAirDate field.
            episodeTemplate = wikiTemplateLine.Contains("}}") && !wikiTemplateLine.Contains("{{Start date");

            return episodeTemplate;
        }

        public static void CollectEpisodeDetails(Episode episode, string readerLine)
        {
            string episodeKey = RetrieveEpisodeKey(readerLine);
            string episodeValue = RetrieveEpisodeValue(readerLine);

            AssignValueToEpisode(episode, episodeKey, episodeValue);
        }

        public static string RetrieveEpisodeKey(string readerLine)
        {
            //Divides key and value and removes the leader pipe and space.
            string episodeKey = readerLine.Split("=")[0].Trim().Substring(2);
            return episodeKey;
        }

        public static string RetrieveEpisodeValue(string readerLine)
        {
            string episodeValue = readerLine.Split("=")[1].Trim();
            return episodeValue;
        }

        public static void AssignValueToEpisode(Episode episode, string episodeKey, string episodeValue)
        {
            switch(episodeKey)
            {
                case "1":
                    episode.season = episodeValue;
                    break;
                case "EpisodeNumber":
                    episode.episodeNumberOverall = Convert.ToInt32(episodeValue);
                    break;
                case "EpisodeNumber2":
                    episode.episodeNumberInSeason = Convert.ToInt32(episodeValue);
                    break;
                case "Title":
                    episode.title = episodeValue;
                    break;
                case "TranslitTitle":
                    episode.titleRomaji = episodeValue;
                    break;
                case "NativeTitle":
                    episode.titleKanji = episodeValue;
                    break;
                case "OriginalAirDate":
                    episode.originalAirDate = ParseWikiDate(episodeValue);
                    break;
                default:
                    break;
            }
        }

        public static DateTime ParseWikiDate(string episodeValue)
        {
            //Format in the form of: {{Start date|2016|4|10}}
            string[] dateValues = episodeValue.Split("|");
            Regex pattern = new Regex(@"\d+"); //Ignores }} at the end for day value.
            int day = Convert.ToInt32(pattern.Match(dateValues[3]).Value);

            return new DateTime(Convert.ToInt32(dateValues[1]), Convert.ToInt32(dateValues[2]), day);
        }

        //Method for testing: Prints all details
        public static void ShowMeEpisodeDetails(int episodeNumber)
        {
            Console.WriteLine("Season: {0}", seasonList[episodeNumber].season);
            Console.WriteLine("Series Number: {0}", seasonList[episodeNumber].episodeNumberOverall);
            Console.WriteLine("Season Episode Number: {0}", seasonList[episodeNumber].episodeNumberInSeason);
            Console.WriteLine("Title: {0}", seasonList[episodeNumber].title);
            Console.WriteLine("Title Romaji: {0}", seasonList[episodeNumber].titleRomaji);
            Console.WriteLine("Title Kanji: {0}", seasonList[episodeNumber].titleKanji);
            Console.WriteLine("Date: {0}", seasonList[episodeNumber].originalAirDate);
        }
    }
}
