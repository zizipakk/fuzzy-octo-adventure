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
            return Content("Hiba történt és az üzemeltetés értesült erről, ez minden, amit erről tudunk.", "text/plain");
        }

        public virtual ActionResult Http500(Exception exception)
        {
            return Content("Általános probléma", "text/plain");
        }

        public virtual ActionResult Http404()
        {
            return Content("Nem található", "text/plain");
        }

        public virtual ActionResult Http403()
        {
            return Content("Tiltott terület", "text/plain");
        }
    }
}