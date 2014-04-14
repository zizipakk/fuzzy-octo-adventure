using Tax.Portal.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Tax.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_BeginRequest()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("hu");
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

        protected void Application_Start()
        {            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            //http://www.dotnet-tricks.com/Tutorial/mvc/aDN4031112-Removing-the-Web-Form-View-Engine-for-better-performance-of-Razor-View-Engine.html
            //Remove All Engine
            ViewEngines.Engines.Clear();
            //Add Razor Engine
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        protected void Application_Error()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Application_Error"))
            {
                log.Info("begin");
                var exception = Server.GetLastError();
                log.Error("Application_Error", exception);
                var httpException = exception as HttpException;
                Response.Clear();
                Server.ClearError();
                var routeData = new RouteData();
                routeData.Values["controller"] = "Errors";
                routeData.Values["action"] = "General";
                routeData.Values["exception"] = exception;
                Response.StatusCode = 500;
                if (httpException != null)
                {
                    Response.StatusCode = httpException.GetHttpCode();
                    switch (Response.StatusCode)
                    {
                        case 403:
                            routeData.Values["action"] = "Http403";
                            break;
                        case 404:
                            routeData.Values["action"] = "Http404";
                            break;
                        case 500:
                            routeData.Values["action"] = "Http500";
                            break;
                    }
                }
                IController errorsController = new ErrorsController();
                var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
                errorsController.Execute(rc);
                log.Info("end");
            }
        }

    }
}
