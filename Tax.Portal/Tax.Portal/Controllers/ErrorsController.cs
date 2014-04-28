using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax.Portal.Controllers
{
    public partial class ErrorsController : Controller
    {
        public virtual ActionResult General()
        {
            return Content("An error occurred and the operation was made aware, that's all you can about it.", "text/plain");
        }

        public virtual ActionResult Http500(Exception exception)
        {
            return Content("There is a common problem.", "text/plain");
        }

        public virtual ActionResult Http404()
        {
            return Content("Not found.", "text/plain");
        }

        public virtual ActionResult Http403()
        {
            return Content("Restricted area.", "text/plain");
        }
    }
}