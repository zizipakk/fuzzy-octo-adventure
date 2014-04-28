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
    public class DevicesController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DevicesController));

        [HttpPut]
        [AllowAnonymous]
        [Route("api/PutDevice")]
        public IHttpActionResult PutDevice(string token, string type, string lang)
        {
            try
            {
                log.Info(string.Format("Put device by token: {0}, type: {1}, language {2}", token, type, lang));

                var deviceService = new DevicesService(context);
                string message = deviceService.PutDevice(token, type, lang);

                if (message != null)
                {
                    log.Info(string.Format("Parameter issue: {0}, end", message));
                    return BadRequest(message);
                }
                log.Info("OK, end");
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Tags access error: ", ex);
                throw;
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("api/DeleteDevice")]
        public IHttpActionResult DeleteDevice(string token)
        {
            try
            {
                log.Info(string.Format("Delete device by token: {0}", token));

                var deviceService = new DevicesService(context);
                string message = deviceService.DeleteDevice(token);

                if (message != null)
                {
                    log.Info(string.Format("Parameter issue: {0}, end", message));
                    return BadRequest(message);
                }
                log.Info("OK, end");
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Tags access error: ", ex);
                throw;
            }
        }

    }
}