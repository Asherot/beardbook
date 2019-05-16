using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using BeardBook.Entities;

namespace BeardBook.Identity
{
    public class AppUserManager : UserManager<User, int>
    {
        public AppUserManager(IUserStore<User, int> store)
            : base(store)
        {
        }

        public static void InitializeUserManager(AppUserManager manager, IAppBuilder app)
        {
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false,
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.EmailService = new EmailService();
            var dataProtectionProvider = app.GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("BeardBook"))
                    {
                        TokenLifespan = TimeSpan.FromHours(1.0)
                    };
            }
        }
    }
}