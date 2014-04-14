using Tax.Data.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Tax.WebAPI.Controllers
{
    [Authorize]
    public abstract class TaxWebAPIBaseController : ApiController
    {
        protected ApplicationDbContext context;

        public TaxWebAPIBaseController()
        {
            context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
