using Tax.Data.Models;
using Tax.WebAPI.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Tax.WebAPI.Controllers
{
    public class SurveyController : TaxWebAPIBaseController
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SurveyController));

        public IHttpActionResult Post([FromBody] Survey survey)
        {
            try
            {
                log.Info("Saving survey data for: " + User.Identity.Name);

                var surveyService = new SurveyService(context);
                surveyService.SaveSurvey(survey, User.Identity.Name);

                context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                log.Error("Error saving survey: ", ex);
                throw;
            }
        }
    }
}
