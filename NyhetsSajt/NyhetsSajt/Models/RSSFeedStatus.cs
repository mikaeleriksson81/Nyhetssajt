using NyhetsSajt.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyhetsSajt.Models
{
    public class RSSFeedStatus
    {
        public RSSUrl RssUrl { get; set; }

        public bool Active { get; set; }

        public string Message { get; set; }
    }
}
