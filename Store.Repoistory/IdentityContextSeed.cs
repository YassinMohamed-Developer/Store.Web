using Microsoft.AspNetCore.Identity;
using Store.Data.Context;
using Store.Data.Entity.IdentityEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory
{
    public class IdentityContextSeed
    {
        public static async Task IdentitySeedAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Yassin Mohamed",
                    Email = "YassinMohamed@gmail.com",
                    UserName = "YassinMohamed",
                    Address = new Address
                    {
                        FirstName = "Yassin",
                        LastName = "Mohamed",
                        City = "Abasia",
                        State = "Cairo",
                        Street = "3",
                        PostalCode = "12134",
                    }
                };
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
