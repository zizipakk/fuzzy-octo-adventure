using Tax.Data.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Tax.Portal.Models;


namespace Tax.Portal.Controllers
{
    public partial class HomeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.Index))
            {
                log.Info("begin");

                //ApplicationDbContext db = new ApplicationDbContext();

                //string usrId = User.Identity.GetUserId();
                //var sinoszrole = db.ApplicationUserRole.FirstOrDefault(x => x.UserId == usrId
                //                    && (x.Role.Name == "SinoszUser" || x.Role.Name == "SinoszAdmin"));

                //if (sinoszrole != null)
                //{
                //    log.Info("end");
                //    return RedirectToAction(MVC.Sinosz.New());
                //}
                //else
                //{
                //    log.Info("end");
                //    return View();//Home/Index
                //}

                log.Info("end");
                return View();//Home/Index

            }
        }

        [AllowAnonymous]
        public virtual ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.About))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.Contact))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult WaitForEmailValidation()
        {
            ViewBag.Message = "Regisztráció folyamatban";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.WaitForEmailValidation))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult MissingEmailValidation()
        {
            ViewBag.Message = "Hiba, nincs megerősítve a regisztráció";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.MissingEmailValidation))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult WaitForEmailValidationAfterPreRegistration()
        {
            //ViewBag.Message = "Az előregisztráció folyamatban";
            ViewBag.Message = "A regisztráció folyamatban";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.WaitForEmailValidationAfterPreRegistration))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult IsNotElected()
        {
            ViewBag.Message = "A kijelölés eszközkölcsönzésre visszavonásra került.";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.IsNotElected))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult IsReservationClosed()
        {
            ViewBag.Message = "Nyilvántartásunk szerint az eszközátvétel lezárult.";

            using (log4net.ThreadContext.Stacks["NDC"].Push(MVC.Home.Actions.ActionNames.IsReservationClosed))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }
    }
}