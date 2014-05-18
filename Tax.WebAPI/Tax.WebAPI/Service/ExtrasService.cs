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
    public class ExtrasService
    {
        ApplicationDbContext context;

        public ExtrasService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// All Extras
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public IEnumerable<ExtrasBindingModel> GetExtras(string lang)
        {
            var resl = context.ExtrasGlobal
                        .Where(x => x.NewsStatus.NameGlobal == "Published")
                        .Include(x => x.CategoriesGlobal)
                        .SelectMany(x => x.ExtrasLocal.Where(y =>
                                                                    y.ExtrasGlobalId == x.Id
                                                                    && y.Language.ShortName == lang)
                            , (x, y) => new { x, y })
                        //.SelectMany(v => context.CategoriesLocal.Where(z =>
                        //                                            z.CategoriesGlobalId == v.x.CategoriesGlobal.Id
                        //                                            && z.Language.ShortName == lang)
                        //    , (v, z) => new { v, z })
                        //.ToList()
                        //.Select(s => new ExtrasBindingModel
                        //{
                        //    Id = s.v.x.Id.ToString(),
                        //    Title1 = s.v.y.Title1,
                        //    Title2 = s.v.y.Title2,
                        //    Subtitle = s.v.y.Subtitle,
                        //    Body = s.v.y.Body_text,
                        //    Order = s.v.x.Order,
                        //    Date = TimestampHelpers.GetTimestamp((DateTime)s.v.x.PublishingDate),
                        //    Category = new CategoriesBindingModel { Id = s.z.CategoriesGlobalId.ToString(), Name = s.z.Name, Order = s.v.x.CategoriesGlobal.Order } 
                        //})
                        .ToList()
                        .Select(s => new ExtrasBindingModel
                        {
                            Id = s.x.Id.ToString(),
                            Title1 = s.y.Title1,
                            Title2 = s.y.Title2,
                            Subtitle = s.y.Subtitle,
                            Body = s.y.Body_text,
                            Order = s.x.Order,
                            Date = TimestampHelpers.GetTimestamp((DateTime)s.x.PublishingDate),
                            Category = s.x.CategoriesGlobal.Id.ToString()
                        })
                        .OrderBy(o => o.Order)
                        .ThenByDescending(o => o.Date);

            if (null == resl)
            {
                return null;
            }
            else
            {
                return resl;
            }
        }    
    }
}
