using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NyhetsSajt.Models;
using NyhetsSajt.Models.Entites;
using NyhetsSajt.Interfaces;

namespace NyhetsSajt.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRSSFeedsRepository _rSSFeedsRepository;

        public AdminController(IRSSFeedsRepository rSSFeedsRepository)
        {
            _rSSFeedsRepository = rSSFeedsRepository;
        }


        public IActionResult Index()
        {
            var feedUrls = _rSSFeedsRepository.GetRSSUrls();

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
                _rSSFeedsRepository.CreateRSSUrl(rSSUrl);
                return RedirectToAction("Index");
            }
            return View(rSSUrl);
        }

        public IActionResult EditRSSUrl(int id)
        {
            RSSUrl rSSUrl = _rSSFeedsRepository.GetRSSUrl(id);

            if (rSSUrl == null)
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
                _rSSFeedsRepository.EditRSSUrl(rSSUrl);
                return RedirectToAction("Index");
            }
            return View(rSSUrl);
        }

        
        public IActionResult DeleteRSSUrl(int id)
        {
            RSSUrl rSSUrl = _rSSFeedsRepository.GetRSSUrl(id);

            if (rSSUrl == null)
            {
                return NotFound();
            }

            _rSSFeedsRepository.DeleteRSSUrl(rSSUrl);

            return RedirectToAction("Index");
        }
    }
}