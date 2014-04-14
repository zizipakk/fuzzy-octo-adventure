using Tax.WebAPI.Models;
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
    [Authorize(Roles = "Jeltolmácsok, Diszpécser")]
    public class OriginalDialedNumberController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OriginalDialedNumberController));

        public InterpreterCallerInfo Get()
        {
            log.Info("Getting original dialed number for user: " + User.Identity.Name);

            try
            {
                log.Info("Get caller infor for: " + User.Identity.Name);

                var PBXService = new PBXService(context);
                return PBXService.GetInterpreterCallerInfo(User.Identity.Name);
            }
            catch(Exception ex)
            {
                log.Error("Caller info query error: ", ex);
                throw ex;
            }

        }
    }
}
