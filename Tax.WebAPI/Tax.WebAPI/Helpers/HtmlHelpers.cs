using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Tax.WebAPI.Helpers
{
    public class HtmlHelpers
    {
        public static string AppBaseUrl(string url)
        {
            var request = HttpContext.Current.Request;
            var scheme = ConfigurationManager.AppSettings["Scheme"];

            if (string.IsNullOrEmpty(scheme) 
                || String.Empty.Equals(scheme.Trim(), StringComparison.OrdinalIgnoreCase))
            {//ha nincs beállítva a web configban, akkor jön a kérésből
                scheme = request.Url.Scheme;
            }
            return string.Format("{0}://{1}{2}", scheme, request.Url.Authority, url);
        }
    }
}