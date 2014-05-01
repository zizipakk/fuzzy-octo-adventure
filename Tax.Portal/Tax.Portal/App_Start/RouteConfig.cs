using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tax.MvcGlobalisationSupport;

namespace Tax.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            const string defaultRouteUrl = "{controller}/{action}/{id}";

            RouteValueDictionary defaultRouteValueDictionary = new RouteValueDictionary(new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            }
);
            Route defaultRoute = new Route(defaultRouteUrl, defaultRouteValueDictionary, new MvcRouteHandler());
            routes.Add("DefaultGlobalised", new GlobalisedRoute(defaultRoute.Url, defaultRoute.Defaults));

            routes.Add("Default", new Route(defaultRouteUrl, defaultRouteValueDictionary, new MvcRouteHandler()));

        }
    }
}
