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
    public class ArticlesService
    {
        ApplicationDbContext context;

        public ArticlesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Only one article after search
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ArticlesBindingModel GetArticle(string id, string lang, string url)
        {
            Guid gId; 
            if (null == id)
            {
                return null;
            }
            else
            {
                gId = Guid.Parse(id);
                var resg = context.NewsGlobal.Where(x => x.Id == gId
                                                        && x.NewsStatus.NameGlobal == "Published")
                                                        .Include(x => x.TagsGlobal)
                                                        .SingleOrDefault();

                var resl = context.NewsLocal.Where(x => x.NewsGlobalId == gId 
                                                        && x.Language.ShortName == lang)
                                                        .SingleOrDefault();

                if (null == resg || null == resl)
                {
                    return null;
                }
                else
                {
                    string baseurl = url;//ebben a környezetben nem jóHtmlHelpers.AppBaseUrl(url);
                    string imageURL = null == resg.Headline_picture ?
                        "" :
                        string.Format("{0}api/Image?id={1}", baseurl, resg.Headline_picture.stream_id.ToString());
                    string thumbnailURL = null == resg.Thumbnail ?
                        "" :
                        string.Format("{0}api/Image?id={1}", baseurl, resg.Thumbnail.stream_id.ToString());

                    return new ArticlesBindingModel
                    {
                        Id = resg.Id.ToString(),
                        ImageURL = imageURL,
                        ThumbnailURL = thumbnailURL,
                        Title1 = resl.Title1,
                        Title2 = resl.Title2,
                        Subtitle = resl.Subtitle,
                        Body = resl.Body_text,
                        Tags = resg.TagsGlobal
                                .SelectMany(v => context.TagsLocal.Where(z => 
                                                                            z.TagsGlobal.Id == v.Id
                                                                            && z.Language.ShortName == lang)
                                    , (v, z) => new TagsBindingModel { Id = v.Id.ToString(), Name = z.Name }),
                        Date = TimestampHelpers.GetTimestamp((DateTime)resg.PublishingDate),
                    };
                }
            }
        }

        /// <summary>
        /// All articles on given page
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="lang"></param>
        /// <param name="page"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public IEnumerable<ArticlesBindingModel> GetArticles(IEnumerable<string> tags, string lang, int? page, string url)
        {
            if (null == page || page <= 0)
            {
                return null;
            }
            else
            {
                string pagesize = null;
                if (page == 1)
                {
                    pagesize = context.SystemParameter.FirstOrDefault(x => x.Name == "FirstPageSize" && x.Public).Value;
                }
                else
                {
                    pagesize = context.SystemParameter.FirstOrDefault(x => x.Name == "NextPageSize" && x.Public).Value;
                }

                if (null == pagesize)
                {
                    return null;
                }
                else
                {

                    //List<string> tagsList = new List<string>(tags);
                    int pageNum = (int)page;
                    int pagesizeNum;
                    if (!int.TryParse(pagesize, out pagesizeNum))
                    {
                        return null;
                    }

                    string baseurl = url;//ebben a környezetben nem jóHtmlHelpers.AppBaseUrl(url);

                    var res = context.NewsGlobal
                                .Where(x => x.NewsStatus.NameGlobal == "Published"
                                        && x.TagsGlobal.Select(y => y.Id.ToString()).ToArray().Intersect(tags).Any()
                                )
                                .Include(x => x.TagsGlobal)
                                .SelectMany(x => x.NewsLocal.Where(y => 
                                                                    y.NewsGlobalId == x.Id
                                                                    && y.Language.ShortName == lang), (x, y) => new { x, y })
                                .OrderByDescending(o => o.x.PublishingDate)
                                .Select(s => new ArticlesBindingModel
                                    {
                                        Id = s.x.Id.ToString(),
                                        ImageURL = string.Format("{0}api/Image?id={1}", baseurl, null == s.x.Headline_picture ? "" : s.x.Headline_picture.stream_id.ToString()),
                                        ThumbnailURL = string.Format("{0}api/Image?id={1}", baseurl, null == s.x.Thumbnail ? "" : s.x.Thumbnail.stream_id.ToString()),
                                        Title1 = s.y.Title1,
                                        Title2 = s.y.Title2,
                                        Subtitle = s.y.Subtitle,
                                        Body = s.y.Body_text,
                                        Tags = s.x.TagsGlobal
                                                    .SelectMany(v => context.TagsLocal.Where(z =>
                                                                                                z.TagsGlobal.Id == v.Id
                                                                                                && z.Language.ShortName == lang)
                                                        , (v, z) => new TagsBindingModel { Id = v.Id.ToString(), Name = z.Name }),
                                        Date = TimestampHelpers.GetTimestamp((DateTime)s.x.PublishingDate),
                                    }
                                )
                                .AsQueryable()
                                .Skip((pageNum - 1) * pagesizeNum).Take(pagesizeNum);

                    return res; 
                }
            }
        }

        /// <summary>
        /// Searching for articles by tags & language
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="lang"></param>
        /// <param name="search"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public IEnumerable<ArticlesBindingModel> SearchArticles(IEnumerable<string> tags, string lang, string search, string url)
        {
            string pagesize = context.SystemParameter.FirstOrDefault(x => x.Name == "SearchPageSize" && x.Public).Value;

            if (null == pagesize)
            {
                return null;
            }
            else
            {
                int pagesizeNum;
                if (!int.TryParse(pagesize, out pagesizeNum))
                {
                    return null;
                }

                string baseurl = url;//ebben a környezetben nem jóHtmlHelpers.AppBaseUrl(url);
                var searchList = SearchHelpers.GetSaerchList(search, lang);

                var res = context.NewsGlobal
                            .Where(x => x.NewsStatus.NameGlobal == "Published"
                                    && x.TagsGlobal.Select(y => y.Id.ToString()).ToArray().Intersect(tags).Any()
                            )
                            .Include(x => x.TagsGlobal)
                            .SelectMany(x => x.NewsLocal.Where(y =>
                                                                y.NewsGlobalId == x.Id
                                                                && y.Language.ShortName == lang), (x, y) => new { x, y })
                            .Where(s => searchList.Any(ss => s.y.Title1.Contains(ss))
                                    || searchList.Any(ss => s.y.Title2.Contains(ss))
                                    || searchList.Any(ss => s.y.Subtitle.Contains(ss))
                                    || searchList.Any(ss => s.y.Body_text.Contains(ss))
                            )
                            .OrderByDescending(o => o.x.PublishingDate)
                            .Select(s => new ArticlesBindingModel
                            {
                                Id = s.x.Id.ToString(),
                                ImageURL = string.Format("{0}api/Image?id={1}", baseurl, null == s.x.Headline_picture ? "" : s.x.Headline_picture.stream_id.ToString()),
                                ThumbnailURL = string.Format("{0}api/Image?id={1}", baseurl, null == s.x.Thumbnail ? "" : s.x.Thumbnail.stream_id.ToString()),
                                Title1 = s.y.Title1,
                                Title2 = s.y.Title2,
                                Subtitle = s.y.Subtitle,
                                Body = s.y.Body_text,
                                Tags = s.x.TagsGlobal
                                            .SelectMany(v => context.TagsLocal.Where(z =>
                                                                                        z.TagsGlobal.Id == v.Id
                                                                                        && z.Language.ShortName == lang)
                                                , (v, z) => new TagsBindingModel { Id = v.Id.ToString(), Name = z.Name }),
                                Date = TimestampHelpers.GetTimestamp((DateTime)s.x.PublishingDate),
                            }
                            )
                            .AsQueryable()
                            .Take(pagesizeNum);

                return res;
            }
        }
    
    }
}
