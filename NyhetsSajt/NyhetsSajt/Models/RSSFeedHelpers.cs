using NyhetsSajt.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NyhetsSajt.Models
{
    internal class RSSFeedHelpers
    {
        private readonly static WebClient wclient = new WebClient();


        internal static IEnumerable<FeedItem> GetRSSFeedItems(IEnumerable<string> feedUrls)
        {

            List<FeedItem> allRSSItems = new List<FeedItem>();


            foreach (string url in feedUrls)
            {
                string RSSData = ReadXMLSource(url);

                XDocument xml = ParseXMLSource(RSSData);


                var feedInfo = xml.Descendants("channel").Select(x =>
                            new RSSFeed
                            {
                                Title = (string)x.Element("title"),
                                Link = GetUrl((string)x.Element("link")),
                                Description = (string)x.Element("description"),
                                Image = GetImageUrl(x)
                            }).SingleOrDefault();


                var RSSItems = (from x in xml.Descendants("item")
                                select new FeedItem
                                {
                                    Title = ((string)x.Element("title")).Replace("&quot;", "''"),
                                    Link = (string)x.Element("link"),
                                    Description = ((string)x.Element("description")).Replace("&quot;", "''"),
                                    PubDate = GetDateFromElement(x.Element("pubDate")),
                                    Category = (string)x.Element("category"),
                                    RSSFeed = feedInfo
                                });

                allRSSItems.AddRange(RSSItems);
            }

            return allRSSItems;
        }


        private static string GetDateFromElement(XElement xElement)
        {
            if ((string)xElement == null)
                return null;

            DateTime date = Convert.ToDateTime((string)xElement);

            string shortDate = date.ToShortDateString();

            string shortTime = date.ToShortTimeString();

            return $"{shortDate}, {shortTime} ";
        }


        private static string ReadXMLSource(string url)
        {
            string RSSData = wclient.DownloadString(url);

            return RSSData;
        }


        private static XDocument ParseXMLSource(string RSSData)
        {
            XDocument xml = XDocument.Parse(RSSData);

            return xml;
        }

        private static string GetUrl(string url)
        {
            if (url != null)
            {
                if(url[url.Length-1].Equals('/'))
                {
                    url = url.Substring(0,url.Length - 1);
                }
                return url;
            }

            return null;
        }


        private static string GetImageUrl(XElement xe)
        {
            if (xe.Element("image") != null)
            {
                return (string)xe.Element("image").Element("url");
            }

            return null;
        }
    }
}
