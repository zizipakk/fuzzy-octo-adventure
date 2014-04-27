using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tax.WebAPI.Models;
using Tax.WebAPI.Service;
using log4net;
using Tax.Data.Models;
using System.Web.Http.Description;
using Tax.WebAPI.Results;
using Newtonsoft.Json;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;

namespace Tax.WebAPI.Controllers
{
    public class ArticlesController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ArticlesController));

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("api/Article")]
        //[ResponseType(typeof(ArticlesBindingModel))]
        //public IHttpActionResult Article(string id, string lang)
        //{
        //    try
        //    {
        //        log.Info(string.Format("Get an article by id: {0}, on language: {1}", id, lang));

        //        var articlesService = new ArticlesService(context);
        //        var article = articlesService.GetArticle(id, lang, Url.Content("~"));

        //        if (article == null)
        //        {
        //            log.Info("Not found, end");                
        //            return NotFound();
        //        }
        //        log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(article)));
        //        log.Info("OK, end");
        //        return Ok(article);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("An article access by id + language error: ", ex);
        //        throw;
        //    }
        //}

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Articles")]
        [ResponseType(typeof(IEnumerable<ArticlesBindingModel>))]
        public IHttpActionResult Articles(string query, [ModelBinder(typeof(ArrayModelBinderProvider))]string[] tags, string lang, int? page)
        {
            try
            {
                log.Info(string.Format("Get a page of Articles in order of PublishedDate by tags: {0}, on language: {1}, by query {2}, in pagenumber: {3}", JsonConvert.SerializeObject(tags), lang, query, page.ToString()));

                var articlesService = new ArticlesService(context);
                var articles = articlesService.GetArticles(query, tags, lang, page, Url.Content("~"));

                if (articles == null || articles.Count() == 0)
                {
                    log.Info("Not found, end");
                    return NotFound();
                }
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(articles)));
                log.Info("OK, end");
                return Ok(articles);
            }
            catch (Exception ex)
            {
                log.Error("A page of Articles access by query + tags + language + page error: ", ex);
                throw;
            }
        }
    }
}