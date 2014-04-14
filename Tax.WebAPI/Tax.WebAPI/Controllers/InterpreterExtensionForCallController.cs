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
    public class InterpreterExtensionForCallController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InterpreterExtensionForCallController));

        public string Get(string callId)
        {
            log.Info("Getting interpreter extension id for user and call: " + User.Identity.Name + " " + callId);

            try
            {
                var pbxService = new PBXService(context);
                var profileService = new ProfileService(context);

                var pbxExtension = profileService.GetUserProfile(User.Identity.Name).PBXExtension;
                return pbxService.GetInterpreterExtensionForCall(callId, pbxExtension);
            }
            catch (Exception ex)
            {
                log.Error("Interpreter extension query error: ", ex);
                throw ex;
            }

        }
    }
}
