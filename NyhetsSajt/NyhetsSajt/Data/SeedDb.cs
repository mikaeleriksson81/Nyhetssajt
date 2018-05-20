using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NyhetsSajt.Models;
using NyhetsSajt.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyhetsSajt.Data
{
    public class SeedDb
    {
        public static async Task Initialize
            (ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            if (!context.RSSUrls.Any())
            {
                context.RSSUrls.Add(new RSSUrl() { FeedName = "Expressen", Url = "http://www.expressen.se/Pages/OutboundFeedsPage.aspx?id=3642159&viewstyle=rss" });
                context.RSSUrls.Add(new RSSUrl() { FeedName = "Norrköpings Tidningar", Url = "http://www.nt.se/nyheter/norrkoping/rss/" });
                context.RSSUrls.Add(new RSSUrl() { FeedName = "Svenska Dagbladet", Url = "https://www.svd.se/?service=rss" });
                context.SaveChanges();
            }

            var roleExist = await roleManager.RoleExistsAsync("Admin");

            var userExist = await userManager.FindByNameAsync("admin");

            if (!roleExist && userExist == null)
            {
                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    ApplicationUser admin = new ApplicationUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com"
                    };


                    var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (roleResult.Succeeded)
                    {
                        var userResult = await userManager.CreateAsync(admin);
                        if (userResult.Succeeded)
                        {
                            var passwordResult = await userManager.AddPasswordAsync(admin, "Admin1234!");
                            if (passwordResult.Succeeded)
                            {
                                var userRoleResult = await userManager.AddToRoleAsync(admin, "Admin");
                                if (userRoleResult.Succeeded)
                                {
                                    
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}
