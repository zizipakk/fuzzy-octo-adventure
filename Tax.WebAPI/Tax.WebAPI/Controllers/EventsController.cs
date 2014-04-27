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
    public class EventsController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EventsController));

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Events")]
        [ResponseType(typeof(IEnumerable<EventsBindingModel>))]
        public IHttpActionResult Events(string lang)
        {
            try
            {
                log.Info(string.Format("Get all of Events on language: {0}", lang));

                var eventsService = new EventsService(context);
                var events = eventsService.GetEvents(lang);

                if (events == null || events.Count() == 0)
                {
                    log.Info("Not found, end");
                    return NotFound();
                }
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(events)));
                log.Info("OK, end");
                return Ok(events);
            }
            catch (Exception ex)
            {
                log.Error("Aii of Events access by language error: ", ex);
                throw;
            }
        }

    }
}