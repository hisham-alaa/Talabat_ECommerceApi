using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtension
    {

        public static async Task<AppUser?> GetUserWithItsAddressAsync(this UserManager<AppUser> _userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(x => x.Email == email);

            return user;
        }

    }
}
