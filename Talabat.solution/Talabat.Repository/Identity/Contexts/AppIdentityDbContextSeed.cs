using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity.Contexts
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedAppUserDataAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Nasr",
                    Email = "ahmed.nasr@linkdev.com",
                    UserName = "ahmed.nasr",
                    PhoneNumber = "01021125956"
                };
                await _userManager.CreateAsync(user, "P@S$w0rd");
            }

        }
    }
}
