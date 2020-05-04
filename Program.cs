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

            startParser(folderPath);

            Console.WriteLine("Testing for stability: {0}", seasonList.Count);
            Console.Write("Enter episode number: ");
            string episodeInput = Console.ReadLine();
            ShowMeEpisodeDetails(Convert.ToInt32(episodeInput) - 1);
            
        }

        public static void startParser(string folderPath)
        {
            using (StreamReader reader = File.OpenText(folderPath + "WikiTemplateSeason.txt"))
            {
                Boolean collectStatus = false; //Ignores all lines until "{{Episode list"
                while(reader.Peek() > -1)

                {
                    string currentLine = reader.ReadLine();
                    
                    if (FindEpisode(currentLine)) //Finds the beginning of an Episode and switches "on" the CollectEpisodeDetails method.
                    {
                        seasonList.Add(new Episode());
                        collectStatus = true;
                    }
                    else if (FindEndEpisode(currentLine)) //Finds the end of an Episode and switches "off" the CollectEpisodeDetails method.
                    {
                        collectStatus = false;
                    }
                    else if (collectStatus)
                    {
                        if (CheckEpisodeDetail(currentLine))
                        {
                            //Ignores <hr> tags and other non-episode details under the collectStatus of "true".

                            continue;
                        }
                        CollectEpisodeDetails(seasonList[seasonList.Count -1], currentLine);
                    }
                }
            }
        }

        //Finds an episode Block
        public static Boolean FindEpisode(string wikiTemplateLine)
        {
            Boolean episodeTemplate = wikiTemplateLine.Contains("{{Episode list");

            return episodeTemplate;
        }

        //Finds the end of the episode block
        public static Boolean FindEndEpisode(string wikiTemplateLine)
        {
            Boolean episodeTemplate = wikiTemplateLine.Equals("}}");

            return episodeTemplate;
        }

        public static Boolean CheckEpisodeDetail(string wikiTemplateLine)
        {
            string readerLine = wikiTemplateLine.Trim();
            Boolean validEpisodeDetail = !readerLine[0].Equals('|');
            return validEpisodeDetail;
        }

        public static void CollectEpisodeDetails(Episode episode, string readerLine)
        {
            string[] partialLines = readerLine.Split('=');

            string episodeKey = ParseWikiText(partialLines[0]);
            string episodeValue = ParseWikiText(partialLines[1]);

            AssignValueToEpisode(episode, episodeKey, episodeValue);
        }

        public static void AssignValueToEpisode(Episode episode, string episodeKey, string episodeValue)
        {
            switch (episodeKey)
            {
                case "1":
                    episode.season = episodeValue;
                    break;
                case "EpisodeNumber":
                    episode.episodeNumberOverall = Convert.ToSingle(episodeValue);
                    break;
                case "EpisodeNumber2":
                    episode.episodeNumberInSeason = Convert.ToSingle(episodeValue);
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

        public static string ParseWikiText(string readerLine) //Extracts values, ignores {, }, [, ].
        {
            string partialWikiText = readerLine.Trim();

            if(partialWikiText.Length == 0)
            {
                return null;
            }
            else if(partialWikiText[0].Equals('|')) //Looks for Episode value substring.
            {
                return readerLine.Substring(2).Trim();
            }
            else if ((partialWikiText.Contains("{{") && partialWikiText.Contains("}}")) || (partialWikiText.Contains("[[") && partialWikiText.Contains("]]")))
            {
                string wikiTextValue = "";
                Regex pattern = new Regex(@"(?<=\[\[|\{\{).*?(?=\]\]|\}\})");

                if(partialWikiText.Contains("[[") && !partialWikiText.Contains("Start date"))
                {
                    wikiTextValue = pattern.Match(partialWikiText).Value.Trim();

                    return ParseWikiLink(wikiTextValue);
                }
                else
                {
                    wikiTextValue = pattern.Match(partialWikiText).Value.Trim();
                    return wikiTextValue;
                }

            }
            else
            {
                return readerLine.Trim();
            }
        }

        public static string ParseWikiLink(string readerLine) //Not Implemented yet!
        {
            //In the format of "Link location|Link Label" or "Link Label"

            string linkLabel = "";

            if(readerLine.Contains('|'))
            {
                string[] partialDetails = readerLine.Split('|');
                linkLabel = partialDetails[1];

                return linkLabel;
            }
            else
            {
                return linkLabel;
            }

        }

        public static DateTime ParseWikiDate(string episodeValue) //Value being passed in has to be trimmed
        {
            //Format in the form of: Start date|2016|4|10
            string[] dateValues = episodeValue.Split("|");

            return new DateTime(Convert.ToInt32(dateValues[1]), Convert.ToInt32(dateValues[2]), Convert.ToInt32(dateValues[3]));
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
            Console.WriteLine("Date: {0}", seasonList[episodeNumber].originalAirDate.GetDateTimeFormats('d'));
        }
    }
}