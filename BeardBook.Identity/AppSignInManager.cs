﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BeardBook.Entities;

namespace BeardBook.Identity
{
    public class AppSignInManager : SignInManager<User, int>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }
    }
}