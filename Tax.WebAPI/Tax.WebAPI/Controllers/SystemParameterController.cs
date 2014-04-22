using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tax.Data.Models;
using log4net;
using Tax.WebAPI.Query;

namespace Tax.WebAPI.Controllers
{
    public class SystemParameterController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SystemParameterController));

        //[AllowAnonymous]
        //public IEnumerable<SystemParameter> Get()
        //{
        //    return SystemParameterQueries.GetAllPublicParameters(context);
        //}

        //[AllowAnonymous]
        //public SystemParameter Get(string name)
        //{
        //    return SystemParameterQueries.GetPublicParameterByName(context, name);
        //}
    }
}
