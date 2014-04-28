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
    public class ImagesService
    {
        ApplicationDbContext context;

        public ImagesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public File GetImage(string id)
        {
            Guid gid;
            if (!Guid.TryParse(id, out gid))
            {
                return null;
            }

            File file = context.File.Find(gid);

            return file;
        }
    }
}