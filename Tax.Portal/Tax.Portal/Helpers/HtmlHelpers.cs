using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Tax.Portal.Helpers
{
    public class HtmlHelpers
    {
        public static string AppBaseUrl(UrlHelper Url)
        {
            var request = HttpContext.Current.Request;
            var scheme = ConfigurationManager.AppSettings["Scheme"];

            if (string.IsNullOrEmpty(scheme) 
                || String.Empty.Equals(scheme.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                scheme = request.Url.Scheme;
            }
            return string.Format("{0}://{1}{2}", scheme, request.Url.Authority, Url.Content("~"));//tartalmazza  avirtuális directory-t a Content
        }

        public static string AppRootUrl()
        {
            var request = HttpContext.Current.Request;
            var scheme = ConfigurationManager.AppSettings["Scheme"];

            if (string.IsNullOrEmpty(scheme)
                || String.Empty.Equals(scheme.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                scheme = request.Url.Scheme;
            }
            return string.Format("{0}://{1}{2}", scheme, //http v. https
                                                request.Url.Authority, //domain v ip:port
                                                request.ApplicationPath //virtual directory
                                );
        }
    }
}