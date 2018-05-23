using NyhetsSajt.Models.Entites;
using NyhetsSajt.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyhetsSajt.Interfaces
{
    public interface IRSSFeedsRepository
    {
        IEnumerable<string> GetRSSUrlsAsString();

        IEnumerable<RSSUrl> GetRSSUrls();

        void CreateRSSUrl(RSSUrl rSSUrl);

        RSSUrl GetRSSUrl(int id);

        void EditRSSUrl(RSSUrl rSSUrl);

        void DeleteRSSUrl(RSSUrl rSSUrl);
    }
}
