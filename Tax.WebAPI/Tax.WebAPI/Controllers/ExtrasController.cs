using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tax.WebAPI.Models;
using Tax.WebAPI.Service;
using log4net;
using Tax.Data.Models;
using System.Web.Http.Description;
using Tax.WebAPI.Results;
using Newtonsoft.Json;

namespace Tax.WebAPI.Controllers
{
    public class ExtrasController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExtrasController));

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Extras")]
        [ResponseType(typeof(IEnumerable<ExtrasBindingModel>))]        
        public IHttpActionResult Extras(string lang)
        {
            try
            {
                log.Info(string.Format("Get all of Extras on language: {0}", lang));

                var extrasService = new ExtrasService(context);
                var extras = extrasService.GetExtras(lang);

                if (extras == null || extras.Count() == 0)
                {
                    log.Info("Not found, end");
                    return NotFound();
                }
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(extras)));
                log.Info("OK, end");

                return Ok(extras);
                //Response.AddHeader('Last-Modified', DateTime.Now.ToUniversalTime().ToString());
            }
            catch (Exception ex)
            {
                log.Error("All of Extras access by language error: ", ex);
                throw;
            }
        }

    }
}