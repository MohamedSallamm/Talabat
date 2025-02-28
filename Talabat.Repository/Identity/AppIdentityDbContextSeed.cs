using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var User = new AppUser()
                {
                    DisplayName = "Mohamed Sallam",
                    Email = "mohamedsallam1995@gmail.com",
                    UserName = "mohamedsallam",
                    PhoneNumber = "01158703600"
                };
                await _userManager.CreateAsync(User, "P@$$w0rd");
            }

        }
    }
}
