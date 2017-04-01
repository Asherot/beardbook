using System;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SimpleInjector;
using BeardBook.Identity;
using BeardBook.Entities;

namespace BeardBook
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app, Container container)
        {
            app.CreatePerOwinContext(container.GetInstance<AppUserManager>);
            app.CreatePerOwinContext(container.GetInstance<AppSignInManager>);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator
                    .OnValidateIdentity<AppUserManager, User, int>(
                        TimeSpan.FromMinutes(30),
                        (manager, user) => user.GenerateUserIdentityAsync(manager),
                        id => int.Parse(id.GetUserId()))
                }
            });            
        }
    }
}