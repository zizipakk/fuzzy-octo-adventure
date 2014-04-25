using System.Data.Entity;
using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax.WebAPI.Query;
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
            File file = null;
            //var user = UserQueries.GetUserByPBXExtension(context, extensionID);
            //if (null != user && user.SinoszUser != null)
            //{
            //    var attachedFile = UserQueries.GetUserByPBXExtension(context, extensionID)
            //                        .SinoszUser.AttachedFile.Where(f => f.FileType == null)
            //                        .SingleOrDefault();
            //    if (attachedFile != null)
            //    {
            //        file = context.File.Find(attachedFile.FileId);
            //    }
            //}
            return file;
        }
    }
}