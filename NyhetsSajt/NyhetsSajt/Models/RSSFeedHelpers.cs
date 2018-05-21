using NyhetsSajt.Models.Entites;
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

                if (RSSData != null)
                {
                    XDocument xml = ParseXMLSource(RSSData);

                    if (xml != null)
                    {

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
                }
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
            try
            {
                string RSSData = wclient.DownloadString(url);

                return RSSData;
            }
            catch (Exception e)
            {
                return null;
            }

            
        }


        private static XDocument ParseXMLSource(string RSSData)
        {           

            try
            {
                XDocument xml = XDocument.Parse(RSSData);
                return xml;
            }
            catch(Exception e)
            {
                return null;
            }
           

            
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


        public static List<RSSFeedStatus> CheckRssStatus(IEnumerable<RSSUrl> rSSUrls)
        {
            List<RSSFeedStatus> rSSFeedStatusList = new List<RSSFeedStatus>();

            foreach (var rSSUrl in rSSUrls)
            {
                RSSFeedStatus rSSFeedStatus = new RSSFeedStatus();

                rSSFeedStatus.RssUrl = rSSUrl;

                string RSSData = ReadXMLSource(rSSUrl.Url);

                if (RSSData != null)
                {
                    XDocument xml = ParseXMLSource(RSSData);

                    if (xml != null)
                    {
                        rSSFeedStatus.Message = "Read & parse to XML OK!";
                        rSSFeedStatus.Active = true;
                    }                    
                    else
                    {
                        rSSFeedStatus.Message = "Error parsing to XML";
                        rSSFeedStatus.Active = false;
                    }
                }
                else
                {
                    rSSFeedStatus.Message = "Error reading from Url";
                    rSSFeedStatus.Active = false;
                }

                rSSFeedStatusList.Add(rSSFeedStatus);
            }

            return rSSFeedStatusList;
        }
    }
}
