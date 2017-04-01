﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BeardBook.ModelBinders;
using BeardBook.Models.HomeViewModels;

namespace BeardBook
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
            System.Web.Mvc.ModelBinders.Binders[typeof(HomeViewModel)] = new HomeViewModelBinder();
        }
    }
}
