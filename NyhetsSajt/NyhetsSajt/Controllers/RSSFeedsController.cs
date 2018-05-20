using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NyhetsSajt.Data;
using NyhetsSajt.Models;

namespace NyhetsSajt.Controllers
{
    public class RSSFeedsController : Controller
    {
        private readonly ApplicationDbContext db;

        public RSSFeedsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRSSFeedItems()
        {

            //Ifall databasen krånglar går det att manuellt köra med några url:er

            //List<string> feedUrls = new List<string> {
            //                        "http://www.nt.se/nyheter/norrkoping/rss/",
            //                        "http://www.expressen.se/Pages/OutboundFeedsPage.aspx?id=3642159&viewstyle=rss",
            //                        "https://www.svd.se/?service=rss"
            //};

            var feedUrls = db.RSSUrls.Select(r=>r.Url).ToList();

            var allRSSItems = RSSFeedHelpers.GetRSSFeedItems(feedUrls);


            return Json(allRSSItems.OrderByDescending(i => i.PubDate));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}