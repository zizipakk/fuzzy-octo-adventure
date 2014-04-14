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
    public class DeviceUsageController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DeviceUsageController));

        public void Post(DeviceUsage clientData)
        {
            try
            {
                log.Info("Device usage data received for: " + User.Identity.Name + " " + clientData.IMEI);

                var deviceUsageService = new DeviceUsageService(context);
                deviceUsageService.LogDeviceUsage(clientData, User.Identity.Name);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error("Device usage save error: ", ex);
                throw ex;
            }
            
        }
    }
}
