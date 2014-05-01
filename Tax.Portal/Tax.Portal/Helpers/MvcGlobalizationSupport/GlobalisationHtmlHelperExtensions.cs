using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Tax.MvcGlobalisationSupport
{
    public static class GlobalisationHtmlHelperExtensions
    {
        static void AddOtherValues(RouteData routeData, RouteValueDictionary destinationRoute)
        {
            foreach (var routeInformation in routeData.Values)
            {
                if (routeInformation.Key == GlobalisedRoute.CultureKey)
                    continue; //Do not re-add, it will throw, this is the old value anyway.
                destinationRoute.Add(routeInformation.Key, routeInformation.Value);
            }
        }

        public static MvcHtmlString GlobalisedRouteLink(this HtmlHelper htmlHelper, string linkText, string targetCultureName, string htmlclass, RouteData routeData)
        {
            RouteValueDictionary globalisedRouteData = new RouteValueDictionary();
            globalisedRouteData.Add(GlobalisedRoute.CultureKey, targetCultureName);

            //var aRoute

            if (0 < htmlHelper.ViewData.ModelState.Count)
            {
                foreach (var key in htmlHelper.ViewData.ModelState)
                {
                    if (!routeData.Values.ContainsKey(key.Key) && null != key.Value.Value && "id" == key.Key)
                    {
                        routeData.Values.Add(key.Key, key.Value.Value.AttemptedValue);
                    }
                }
            }

            if (null != htmlHelper.ViewData.Model && !routeData.Values.Any(t => "id" == t.Key))
            {
                routeData.Values.Add("id", htmlHelper.ViewData.ModelMetadata.SimpleDisplayText);
            }


            AddOtherValues(routeData, globalisedRouteData);
            return htmlHelper.RouteLink(linkText, globalisedRouteData, new Dictionary<string,object>() { {"class", htmlclass} });
        }
    }
}
