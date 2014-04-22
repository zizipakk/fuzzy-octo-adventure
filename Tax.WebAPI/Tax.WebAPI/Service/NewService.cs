using System.Data.Entity;
using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax.WebAPI.Query;


namespace Tax.WebAPI.Service
{
    public class NewService
    {
        ApplicationDbContext context;

        public NewService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Only one news after search
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NewBindingModel GetNew(string id, string lang)
        {
            Guid gId; 
            if (null == id)
            {
                return null;
            }
            else
            {
                gId = Guid.Parse(id);
                var res = context.NewsGlobal.Where(x => x.Id == gId)
                            .SelectMany(x => context.NewsLocal.Where(y => y.Language.Name == lang)
                                ,(x, y) => new {x , y} 
                            ).SingleOrDefault();

                return new NewBindingModel
                {
                    Id = res.x.Id,
                    Date = ((res.x.PublishingDate.Value.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000).ToString(),
                    Headline_pictureId = res.x.Headline_picture.stream_id,
                    ThumbnailId = res.x.Thumbnail.stream_id,
                    Tags = res.x.TagsGlobal.SelectMany(v => context.TagsLocal.Where(z => z.Language.Name == lang), (v, z) => new TagBindingModel { Id = v.Id, Name = z.Name}),
                    Title1 = res.y.Title1,
                    Title2 = res.y.Title2,
                    Subtitle = res.y.Subtitle,
                    Body = res.y.Body_text
                };
            }

        }

        //public File GetUserPhotoByPBXExtension(string extensionID)
        //{
        //    File file = null;
        //    var user = UserQueries.GetUserByPBXExtension(context, extensionID);
        //    if (null!=user && user.SinoszUser != null)
        //    {
        //        var attachedFile = UserQueries.GetUserByPBXExtension(context, extensionID)
        //                            .SinoszUser.AttachedFile.Where(f => f.FileType == null)
        //                            .SingleOrDefault();
        //        if (attachedFile != null)
        //        {
        //            file = context.File.Find(attachedFile.FileId);
        //        }
        //    }
        //    return file;
        //}
    }
}