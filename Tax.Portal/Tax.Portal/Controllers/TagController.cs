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
using System.Threading.Tasks;
using System.Net;
using Tax.Portal.Models;
using Newtonsoft.Json;


namespace Tax.Portal.Controllers
{
    public partial class TagController : Controller
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

        #region list

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual System.Web.Mvc.JsonResult ListTags(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            //string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
            Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);

            var rs = db.TagsGlobal
                                .SelectMany(x => x.TagsLocal.Where(y => y.LanguageId == lguidEn)
                                    , (x, y) => new { x, y })
                                .SelectMany(x => x.x.TagsLocal.Where(y => y.LanguageId == lguidHu)
                                    , (x, y) => new { x, y })
                        .ToList()
                        .Select(s => new 
                        {
                            Id = s.x.x.Id,
                            NameEn = s.x.y.Name,
                            NameHu = s.y.Name,
                        })
                        .AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.NameEn
                                ,r.NameHu
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion list 

        #region create

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create(string NameEn, string NameHu)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Tag/Create"))
            {
                log.Info("begin");
                if ("" == nameEn)
                {
                    return Json(new { success = false, error = false, response = "[ENG] is required" });
                }
                if ("" == nameHu)
                {
                    return Json(new { success = false, error = false, response = "[HUN] is required" });
                }
                int minlenght = 3;
                if (nameEn.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The ENG must be at least {0} characters long.", minlenght) });
                }
                if (nameHu.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The ENG must be at least {0} characters long.", minlenght) });
                }
                TagsGlobal resg = db.TagsGlobal.Create();
                db.Entry(resg).State = EntityState.Added;

                //string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
                TagsLocal reslEn = db.TagsLocal.Create();
                reslEn.TagsGlobal = resg;
                reslEn.Language = db.Language.Find(lguidEn);
                reslEn.Name = nameEn;
                db.Entry(reslEn).State = EntityState.Added;
                Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);
                TagsLocal reslHu = db.TagsLocal.Create();
                reslHu.TagsGlobal = resg;
                reslHu.Language = db.Language.Find(lguidHu);
                reslHu.Name = nameHu;
                db.Entry(reslHu).State = EntityState.Added;
                db.SaveChanges();
                log.Info("end with ok");

                return Json(new { success = true, error = false, response = "" });
            }
        }

        #endregion create

        #region edit

        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(string id, string oper, string nameEn, string nameHu)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Contact/Edit"))
            {
                Guid Id = Guid.Parse(id);
                log.Info("begin");
                TagsGlobal resg;

                switch (oper)
                {
                    case "edit":
                        resg = db.TagsGlobal.Find(id);
                        Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
                        TagsLocal reslEn = db.TagsLocal.FirstOrDefault(x => x.TagsGlobalId == Id && x.LanguageId == lguidEn);
                        if (reslEn.Name != nameEn) { reslEn.Name = nameEn; }
                        Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);
                        TagsLocal reslHu = db.TagsLocal.FirstOrDefault(x => x.TagsGlobalId == Id && x.LanguageId == lguidHu);
                        if (reslHu.Name != nameHu) { reslHu.Name = nameHu; }
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
                log.Info("end with ok");
                return Json(new { success = true, error = false, response = "" });
            }
        }

        #endregion edit

        #region delete

        /// <summary>
        /// Törlés
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult DeleteTag(Guid id)
        {
            var tg = db.TagsGlobal.Find(id);
            if (null != tg)
            {
                //először a lokális cucc a levesbe
                var tl = db.TagsLocal.Where(x => x.TagsGlobalId == id);
                foreach (var item in tl.ToList())
                { db.Entry(item).State = EntityState.Deleted; }                
                db.Entry(tg).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = false, response = "Not found" });
        }

        #endregion delete

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




