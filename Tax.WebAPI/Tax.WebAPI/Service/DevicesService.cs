using System.Data.Entity;
using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax.WebAPI.Helpers;
using System.Web.Mvc;

namespace Tax.WebAPI.Service
{
    public class DevicesService
    {
        ApplicationDbContext context;

        public DevicesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string PutDevice(string token, string type, string lang)
        {
            var res = context.Device.FirstOrDefault(x => x.Token == token);
            if (null != res)
                return string.Format("Already uploaded token: {0}", token);
            res = context.Device.Create();
            res.Token = token;
            res.DeviceType = context.DeviceType.SingleOrDefault(x => x.Name == type);
            if (null == res.DeviceType)
                return string.Format("Not valid type: {0}", type);
            res.Language = context.Language.SingleOrDefault(x => x.ShortName == lang);
            if (null == res.Language)
                return string.Format("Not valid language: {0}", lang);
            context.Entry(res).State = EntityState.Added;
            context.SaveChanges();

            return null;
        }

        public string DeleteDevice(string token)
        {
            var res = context.Device.Where(x => x.Token == token);
            if (null == res || res.Count() == 0)
            {
                return string.Format("Not found token: {0}", token);
            }
            else
            {
                foreach (var item in res.ToList())
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }

            return null; 
        }

    }
}
