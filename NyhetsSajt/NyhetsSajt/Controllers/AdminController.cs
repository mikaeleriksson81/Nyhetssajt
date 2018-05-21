using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NyhetsSajt.Data;
using NyhetsSajt.Models;
using NyhetsSajt.Models.Entites;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;

namespace NyhetsSajt.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminController(ApplicationDbContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            var feedUrls = db.RSSUrls.ToList();

            List<RSSFeedStatus> rSSFeedStatusList = RSSFeedHelpers.CheckRssStatus(feedUrls);

            return View(rSSFeedStatusList);
        }

        public IActionResult CreateRSSUrl()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRSSUrl(RSSUrl rSSUrl)
        {
            if(ModelState.IsValid)
            {
                db.RSSUrls.Add(rSSUrl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rSSUrl);
        }

        public IActionResult EditRSSUrl(int id)
        {
            RSSUrl rSSUrl = db.RSSUrls.SingleOrDefault(r=>r.Id == id);

            if(rSSUrl== null)
            {
                return StatusCode(404);
            }
            return View(rSSUrl);
        }

        [HttpPost]
        public IActionResult EditRSSUrl(RSSUrl rSSUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rSSUrl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rSSUrl);
        }

        
        public IActionResult DeleteRSSUrl(int id)
        {
            RSSUrl rSSUrl = db.RSSUrls.SingleOrDefault(r => r.Id == id);

            if (rSSUrl == null)
            {
                return StatusCode(404);
            }

            db.Entry(rSSUrl).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}