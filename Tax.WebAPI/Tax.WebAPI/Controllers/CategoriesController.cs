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
    public class CategoriesController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CategoriesController));

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Categories")]
        [ResponseType(typeof(IEnumerable<CategoriesBindingModel>))]
        public IHttpActionResult GetCategories(string lang)
        {
            try
            {
                log.Info(string.Format("Get all of Categories on language: {0}", lang));

                var categoriesService = new CategoriesService(context);
                var categories = categoriesService.GetCategories(lang);

                if (categories == null || categories.Count() == 0)
                {
                    log.Info("Not found, end");
                    return NotFound();
                }
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(categories)));
                log.Info("OK, end");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                log.Error("Categories access error: ", ex);
                throw;
            }
        }

    }
}