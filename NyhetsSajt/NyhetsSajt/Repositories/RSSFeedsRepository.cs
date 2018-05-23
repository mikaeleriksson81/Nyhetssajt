using Microsoft.EntityFrameworkCore;
using NyhetsSajt.Data;
using NyhetsSajt.Interfaces;
using NyhetsSajt.Models.Entites;
using NyhetsSajt.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyhetsSajt.Repositories
{
    public class RSSFeedsRepository : IRSSFeedsRepository
    {
        private readonly ApplicationDbContext db;

        public RSSFeedsRepository(ApplicationDbContext context)
        {
            db = context;
        }
        
        public void CreateRSSUrl(RSSUrl rSSUrl)
        {
            db.RSSUrls.Add(rSSUrl);
            db.SaveChanges();
        }
        
        public void DeleteRSSUrl(RSSUrl rSSUrl)
        {
            db.Entry(rSSUrl).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public void EditRSSUrl(RSSUrl rSSUrl)
        {
            db.Entry(rSSUrl).State = EntityState.Modified;
            db.SaveChanges();
        }

        public RSSUrl GetRSSUrl(int id)
        {
            RSSUrl rSSUrl = db.RSSUrls.SingleOrDefault(r => r.Id == id);

            return rSSUrl;
        }

        public IEnumerable<RSSUrl> GetRSSUrls()
        {
            return db.RSSUrls.ToList();
        }

        public IEnumerable<string> GetRSSUrlsAsString()
        {
            return db.RSSUrls.Select(r => r.Url).ToList();
        }
    }
}
