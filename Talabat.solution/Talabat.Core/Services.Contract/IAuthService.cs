using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Core.Services.Contract
{
    public interface IAuthService
    {
        public Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
