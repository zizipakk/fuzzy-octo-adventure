using Tax.Portal.Helpers;
using System.Web;
using System.Web.Mvc;

namespace Tax.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
