using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NyhetsSajt.Interfaces;
using NyhetsSajt.Models;

namespace NyhetsSajt.Controllers
{
    public class RSSFeedsController : Controller
    {
        private readonly IRSSFeedsRepository _rSSFeedsRepository;

        public RSSFeedsController(IRSSFeedsRepository rSSFeedsRepository)
        {
            _rSSFeedsRepository = rSSFeedsRepository;            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRSSFeedItems()
        {

            //Backup sources incase trouble with the database
            //List<string> feedUrls = new List<string> {
            //                        "http://www.nt.se/nyheter/norrkoping/rss/",
            //                        "http://www.expressen.se/Pages/OutboundFeedsPage.aspx?id=3642159&viewstyle=rss",
            //                        "https://www.svd.se/?service=rss"
            //};

            var feedUrls = _rSSFeedsRepository.GetRSSUrlsAsString();

            var allRSSItems = RSSFeedHelpers.GetRSSFeedItems(feedUrls);            

            return Json(allRSSItems);            
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}