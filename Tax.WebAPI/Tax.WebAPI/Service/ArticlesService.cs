using System.Data.Entity;
using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax.WebAPI.Helpers;
using System.Web.Mvc;
using System.Diagnostics;

namespace Tax.WebAPI.Service
{
    public class ArticlesService
    {
        ApplicationDbContext context;

        public ArticlesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Searching for articles by tags & language & page
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="lang"></param>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IEnumerable<ArticlesBindingModel> GetArticles(string query, string[] tags, string lang, int? page)
        {
            if (page <= 0)
            {
                return null;
            }
            else
            {
                if (null == page)
                {
                    page = 1;
                }
                string pagesize = null;
                int pagesizeNum;
                int skipNum;
                if (page == 1)
                {
                    pagesize = context.SystemParameter.FirstOrDefault(x => x.Name == "FirstPageSize" && x.Public).Value;
                    if (null == pagesize)
                    {
                        return null;
                    }
                    else
                    {
                        if (!int.TryParse(pagesize, out pagesizeNum))
                        {
                            return null;
                        }    
                    }
                    skipNum = 0;
                }
                else
                {
                    string firstpagesize = context.SystemParameter.FirstOrDefault(x => x.Name == "FirstPageSize" && x.Public).Value;
                    if (null == firstpagesize)
                    {
                        return null;
                    }
                    else
                    {
                        if (!int.TryParse(firstpagesize, out skipNum))
                        {
                            return null;
                        }                         
                    }
                    pagesize = context.SystemParameter.FirstOrDefault(x => x.Name == "NextPageSize" && x.Public).Value;
                    if (null == pagesize)
                    {
                        return null;
                    }
                    else
                    {
                        if (!int.TryParse(pagesize, out pagesizeNum))
                        {
                            return null;
                        }
                    }
                    skipNum = skipNum + (((int)page - 2) * pagesizeNum);
                }

                string baseurl = HtmlHelpers.AppBaseUrl("/api/Image?id=");
                var searchList = SearchHelpers.GetSaerchList(query, lang);

                List<Guid> tagsGL = new List<Guid>();
                foreach (string t in tags.ToList())
                {
                    tagsGL.Add(Guid.Parse(t));
                }
                Guid[] tagsGA = tagsGL.ToArray();

                var res = context.NewsGlobal
                            .Where(x => x.NewsStatus.NameGlobal == "Published"
                                    && (
                                        tags.Count() == 0
                                        || x.TagsGlobal.Select(y => y.Id).Intersect(tagsGA).Any()
                                        )
                            )
                            //.Include(x => x.TagsGlobal)
                            .SelectMany(x => x.NewsLocal.Where(y =>
                                                                y.NewsGlobalId == x.Id
                                                                && y.Language.ShortName == lang), (x, y) => new { x, y })
                            .ToList()//bonyi searchlistnél behal az ef
                            .Where(s =>
                                    searchList.Count() == 0
                                    ||
                                    (
                                        (null != s.y.Title1 && searchList.Any(ss => s.y.Title1.Contains(ss)))
                                        || 
                                        (null != s.y.Title2 && searchList.Any(ss => s.y.Title2.Contains(ss)))
                                        || 
                                        (null != s.y.Subtitle && searchList.Any(ss => s.y.Subtitle.Contains(ss)))
                                        || 
                                        (null != s.y.Body_text && searchList.Any(ss => s.y.Body_text.Contains(ss)))
                                    )
                            )
                            //.ToList()
                            .Select(s => new ArticlesBindingModel
                            {
                                Id = s.x.Id.ToString(),
                                ImageURL = string.Format("{0}{1}", baseurl, null == s.x.Headline_picture ? "" : s.x.Headline_picture.stream_id.ToString()),
                                ThumbnailURL = string.Format("{0}{1}", baseurl, null == s.x.Thumbnail ? "" : s.x.Thumbnail.stream_id.ToString()),
                                Title1 = s.y.Title1,
                                Title2 = s.y.Title2 ?? "",
                                Subtitle = s.y.Subtitle ?? "",
                                Body = s.y.Body_text,
                                //Tags = s.x.TagsGlobal
                                //            .SelectMany(v => context.TagsLocal.Where(z =>
                                //                                                        z.TagsGlobal.Id == v.Id
                                //                                                        && z.Language.ShortName == lang)
                                //                , (v, z) => new TagsBindingModel { Id = v.Id.ToString(), Name = z.Name }),
                                Tags = s.x.TagsGlobal.Select(v => v.Id.ToString()).ToArray(),
                                Date = null == s.x.PublishingDate ? "" : TimestampHelpers.GetTimestamp((DateTime)s.x.PublishingDate)
                            }
                            )
                            .OrderByDescending(o => o.Date)
                            .AsQueryable()
                            .Skip(skipNum).Take(pagesizeNum);

                //foreach (var item in res) { Debug.WriteLine(item); }

                return res;
            }
        }
    
    }
}
