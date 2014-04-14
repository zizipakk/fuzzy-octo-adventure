using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Configuration;
using Newtonsoft.Json;
using Tax.WebAPI.Service;
using Tax.Data.Models;

namespace Tax.WebAPI.Controllers
{
    public class BugPostController : TaxWebAPIBaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BugPostController));

        [System.Web.Http.AllowAnonymous]
        public IHttpActionResult Get()
        {
            var ex = new ApplicationException("WebApi teszt");
            log.Error("WebApi teszt exception", ex);
            throw ex;
        }

        [System.Web.Http.AllowAnonymous]
        public IHttpActionResult Post([FromBody] BugPost model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("QueueBugPost"))
            {
                try
                {
                    log.Info("begin");

                    //elmentjük
                    var bugpostService = new BugPostService(context);
                    bugpostService.AddBugPost(model);
                    context.SaveChanges();

                    var HostBaseUrl = ConfigurationManager.AppSettings["AppenderHostBaseUrl"];
                    var Action = ConfigurationManager.AppSettings["AppenderAction"];

                    if (!string.IsNullOrWhiteSpace(HostBaseUrl) && !string.IsNullOrWhiteSpace(Action))
                    {
                        log.Info("Resend queue");

                        var client = new RestClient(HostBaseUrl);
                        var request = new RestRequest(Action, Method.POST);

                        request.AddParameter("UserName", model.UserName);
                        request.AddParameter("ProjectName", model.ProjectName);
                        request.AddParameter("AreaName", model.AreaName);
                        request.AddParameter("Description", model.Description);
                        request.AddParameter("ExtraInformation", model.ExtraInformation);
                        request.AddParameter("EmailAddress ", model.EmailAddress);
                        request.AddParameter("IsForceNewBug", model.IsForceNewBug);
                        request.AddParameter("IsFriendlyResponse", model.IsFriendlyResponse);

                        log.Info(string.Format("request: {0}", JsonConvert.SerializeObject(request)));

                        client.ExecuteAsync(request, ExecuteCompleted);
                    }
                    
                    log.Info("end");
                    return Ok();
                }
                catch (Exception ex)
                {
                    log.Error("Error QueueBugPost: ", ex);
                    throw;
                }

            }
        }

        private void ExecuteCompleted(IRestResponse response)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ExecuteCompleted"))
            {
                log.Info("begin");
                log.Info(string.Format("response: {0}",JsonConvert.SerializeObject(response)));

                if (null != response.ErrorException)
                {
                    log.Error("Error QueueBugPost request execute: ", response.ErrorException);
                }

                log.Info("end");
            }
        }
    
    }

}