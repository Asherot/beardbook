using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BeardBook.Commands;
using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace BeardBook
{
    public static class SimpleInjectorConfig
    {
        public static Container Initialize(IAppBuilder app)
        {
            var container = InitializeContainer(app);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static Container InitializeContainer(IAppBuilder app)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            #region Register Identity

            container.RegisterSingleton(app);

            container.Register(() => new BeardBookDbContext("DefaultConnection"), Lifestyle.Scoped);

            container.Register<IUserStore<User, int>>(() =>
                new CustomUserStore(container.GetInstance<BeardBookDbContext>()), Lifestyle.Scoped);

            container.Register<AppUserManager>(Lifestyle.Scoped);
            container.RegisterInitializer<AppUserManager>(manager => AppUserManager.InitializeUserManager(manager, app));

            container.Register<SignInManager<User, int>, AppSignInManager>(Lifestyle.Scoped);
            container.Register(() =>
                container.IsVerifying()
                    ? new OwinContext(new Dictionary<string, object>()).Authentication
                    : HttpContext.Current.GetOwinContext().Authentication,
                Lifestyle.Scoped);

            #endregion

            container.Register(typeof(IQueryHandler<,>), new [] { typeof(IQueryHandler<,>).Assembly }, Lifestyle.Scoped);
            container.Register(typeof(ICommandHandler<>), new [] { typeof(ICommandHandler<>).Assembly }, Lifestyle.Scoped);

            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(TransactionQueryHandlerDecorator<,>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(TransactionCommandHandlerDecorator<>));

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            return container;
        }
    }
}