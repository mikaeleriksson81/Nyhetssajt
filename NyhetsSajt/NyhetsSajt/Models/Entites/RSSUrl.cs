using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NyhetsSajt.Models.Entites
{
    public class RSSUrl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Feed Name")]
        public string FeedName { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
