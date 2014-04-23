using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax.WebAPI.Controllers
{
    public class APITestController : Controller
    {
        [AllowAnonymous]
        public ActionResult GetNew()
        {
            return View();
        }

        public ActionResult GetNews()
        {
            return View();
        }

        public ActionResult ChatMessage()
        {
            return View();
        }

        public ActionResult AddressBook()
        {
            return View();
        }

        public ActionResult SystemParameter()
        {
            return View();
        }
	}
}