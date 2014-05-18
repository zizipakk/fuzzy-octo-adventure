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
    public class CategoriesService
    {
        ApplicationDbContext context;

        public CategoriesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// All categories
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public IEnumerable<CategoriesBindingModel> GetCategories(string lang)
        {
            var resl = context.CategoriesGlobal
                        .SelectMany(x => x.CategoriesLocal.Where(y =>
                                                                    y.CategoriesGlobalId == x.Id
                                                                    && y.Language.ShortName == lang)
                            , (x, y) => new { x, y })
                        .ToList()
                        .Select(s => new CategoriesBindingModel
                        {
                            Id = s.x.Id.ToString(),
                            Name = s.y.Name,
                            Order = s.x.Order
                        })
                        .OrderBy(x => x.Order);

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
