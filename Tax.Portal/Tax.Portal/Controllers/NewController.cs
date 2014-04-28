using JQGrid.Helpers;
using Tax.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Tax.Portal.Helpers;
using System.Threading;


namespace Tax.Portal.Controllers
{
    public partial class NewController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Index()
        {
            log.Info("begin");
            log.Info("end");
            return View();
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListNews(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            var rs0 = db.NewsGlobal
                                .Include(x => x.TagsGlobal)
                                .SelectMany(x => x.NewsLocal.Where(y =>
                                                                    y.NewsGlobalId == x.Id
                                                                    && y.Language.ShortName == lid), (x, y) => new { x, y })
                       .ToList()
                       .Select(s => new 
                        {
                            Id = s.x.Id,
                            Status = s.x.NewsStatus.NewsStatusesLocal.FirstOrDefault(v => v.Language.ShortName == lid).Name,
                            Title1 = s.y.Title1,
                            Title2 = s.y.Title2,                            
                            Tags = s.x.TagsGlobal
                                        .SelectMany(v => db.TagsLocal.Where(z =>
                                                                z.TagsGlobal.Id == v.Id
                                                                && z.Language.ShortName == lid)
                                                                .ToList()
                                                                .GroupBy(g => g.TagsGlobalId)
                                                                .Select(g => new { 
                                                                    Tags = (g.Select(ss => ss.Name)).Aggregate((a, b) => (a == "" ? "" : a + ", ") + b)
                                                                }),
                            PublishingDate = (DateTime)s.x.PublishingDate,
                            Thumbnail = s.x.Thumbnail.stream_id
                        }).AsEnumerable();

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                string.Empty
                                ,r.Id.ToString()
                                ,r.Status
                                ,r.Title1
                                ,r.Title2
                                ,r.Tags
                                ,r.PublishingDate.ToString(),
                                ,r.Thumbnail.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateSystemParameters(string id, string oper, SystemParameter r)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            SystemParameter res;

            switch (oper)
            {
                case "edit":
                    res = db.SystemParameter.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.Name != r.Name) { res.Name = r.Name; }
                    if (res.Value != r.Value) { res.Value = r.Value; }
                    if (res.Description != r.Description) { res.Description = r.Description; }
                    if (res.Public != r.Public) { res.Public = r.Public; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.SystemParameter.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.SystemParameter.Create();
                    res.Id = r.Id;
                    res.Name = r.Name;
                    res.Value = r.Value;
                    res.Description = r.Description;
                    res.Public = r.Public;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        public virtual ActionResult UpdateSetupIspublic(Guid Id, bool isPublic)
        {
            if (null != Id)
            {
                var a = db.SystemParameter.Find(Id);
                if (a.Public != isPublic)
                {
                    a.Public = isPublic;
                    db.SaveChanges();
                }
            }
            return null;
        }
        
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Setup()
        {
            log.Info("begin");
            log.Info("end");
            return View();
        }


        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListSystemParameters(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.SystemParameter
                       .Select(a => new
                       {
                           Id = a.Id,
                           Name = a.Name,
                           Value = a.Value,
                           Description = a.Description,
                           Public = a.Public
                       }).AsEnumerable();

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                string.Empty
                                ,r.Id.ToString()
                                ,r.Name
                                ,r.Value
                                ,r.Description
                                ,r.Public.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateSystemParameters(string id, string oper, SystemParameter r)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            SystemParameter res;

            switch (oper)
            {
                case "edit":
                    res = db.SystemParameter.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.Name != r.Name) { res.Name = r.Name; }
                    if (res.Value != r.Value) { res.Value = r.Value; }
                    if (res.Description != r.Description) { res.Description = r.Description; }
                    if (res.Public != r.Public) { res.Public = r.Public; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.SystemParameter.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.SystemParameter.Create();
                    res.Id = r.Id;
                    res.Name = r.Name;
                    res.Value = r.Value;
                    res.Description = r.Description;
                    res.Public = r.Public;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        public virtual ActionResult UpdateSetupIspublic(Guid Id, bool isPublic)
        {
            if (null != Id)
            {
                var a = db.SystemParameter.Find(Id);
                if (a.Public != isPublic)
                {
                    a.Public = isPublic;
                    db.SaveChanges();
                }
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




