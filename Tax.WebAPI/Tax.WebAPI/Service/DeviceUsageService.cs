using Tax.Data.Models;
using Tax.WebAPI.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Service
{
    public class DeviceUsageService
    {
        ApplicationDbContext context;

        public DeviceUsageService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void LogDeviceUsage(DeviceUsage clientData, string username)
        {
            clientData.User = UserQueries.GetUserByName(context, username);
            clientData.Timestamp = DateTime.Now;

            context.DeviceUsage.Add(clientData);
        }
    }
}