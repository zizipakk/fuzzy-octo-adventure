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
    public class TagsService
    {
        ApplicationDbContext context;

        public TagsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// All tags
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public IEnumerable<TagsBindingModel> GetTags(string lang)
        {
            var resl = context.TagsLocal
                        .Where(x => x.Language.ShortName == lang)
                        .ToList()
                        .Select(x => new TagsBindingModel
                        {
                            Id = x.TagsGlobalId.ToString(),
                            Name = x.Name
                        })
                        //.OrderBy(x => x.Name)
                        ;

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
