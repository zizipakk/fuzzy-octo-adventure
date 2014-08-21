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
    public class TagsController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TagsController));

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Tags")]
        [ResponseType(typeof(IEnumerable<TagsBindingModel>))]
        public IHttpActionResult GetTags(string lang)
        {
            try
            {
                log.Info(string.Format("Get all of Tags on language: {0}", lang));

                var tagsService = new TagsService(context);
                var tags = tagsService.GetTags(lang);

                if (tags == null)
                {
                    log.Info("Not found, end");
                    return NotFound();
                }
                else if (tags.Count() == 0)
                {
                    log.Info("Empty result");
                    var empty = new Dictionary<string, string>();
                    return Ok(empty);
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(tags)));
                    log.Info("OK, end");
                    return Ok(tags);
                }
            }
            catch (Exception ex)
            {
                log.Error("Tags access error: ", ex);
                throw;
            }
        }

    }
}