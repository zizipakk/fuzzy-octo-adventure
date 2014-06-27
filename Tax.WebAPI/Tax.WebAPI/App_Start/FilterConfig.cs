using System;
using System.Globalization;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Tax.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    //public class AddLastModifiedHeader : System.Web.Http.Filters.ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    //    {
    //        //string date = DateTime.Now.ToUniversalTime().ToString();
    //        string date = DateTime.UtcNow.ToUniversalTime().ToString("R");
    //        if (null != actionExecutedContext.Response && null != actionExecutedContext.Response.Content)
    //        {
    //            actionExecutedContext.Response.Content.Headers.TryAddWithoutValidation("Last-Modified", date);
    //        }
    //    }

    //    public void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        //    if (filterContext.ActionParameters.ContainsKey(""))
    //        //    {
    //        //        var aParam = filterContext.ActionParameters[""];
    //        //    }
    //        throw new NotImplementedException();
    //    }
    //}
}
