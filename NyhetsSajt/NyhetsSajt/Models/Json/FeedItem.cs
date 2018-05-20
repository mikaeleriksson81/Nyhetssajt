using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyhetsSajt.Models.Json
{
    public class FeedItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public RSSFeed RSSFeed { get; set; }
    }
}
