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
    public partial class ExtraController : Controller
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
        public virtual System.Web.Mvc.JsonResult ListExtras(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

            var rs = db.ExtrasGlobal
                                .SelectMany(x => x.ExtrasLocal.Where(y => y.LanguageId == lguid)
                                    , (x, y) => new { x, y })
                                .SelectMany(z => z.x.NewsStatus.NewsStatusesLocal.Where(v => v.LanguageId == lguid)
                                    , (z, v) => new { z, v })
                                .SelectMany(a => a.z.x.CategoriesGlobal.CategoriesLocal.Where(b => b.LanguageId == lguid)
                                    , (a, b) => new { a, b })
                        .ToList()
                        .Select(s => new 
                        {
                            Id = s.a.z.x.Id,
                            Status = s.a.v.Name,
                            Title1 = s.a.z.y.Title1,
                            Title2 = s.a.z.y.Title2,
                            Subtitle = s.a.z.y.Subtitle,
                            PublishingDate = s.a.z.x.PublishingDate,
                            Category = s.b.Name,
                            Order1 = s.b.CategoriesGlobal.Order,
                            Order2 = s.a.z.x.Order
                        })
                        .AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                r.Id.ToString()
                                ,r.Status
                                ,r.Title1
                                ,r.Title2
                                ,r.Subtitle
                                ,r.PublishingDate.ToString()
                                ,r.Category
                                ,r.Order1.ToString()
                                ,r.Order2.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult FlashExtra(Guid id)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: New/FlashNew"))
            {
                log.Info("begin");

                var eg = db.ExtrasGlobal.Find(id);
                ExtraViewModel evm = new ExtraViewModel()
                {
                    Id = eg.Id,
                    PublishingDate = eg.PublishingDate,
                    Order = eg.Order,
                    NewsStatusName = eg.NewsStatus.NameGlobal
                };

                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var el = db.ExtrasLocal.FirstOrDefault(x => x.ExtrasGlobalId == id && x.LanguageId == lguid);
                evm.Title1 = el.Title1;
                evm.Title2 = el.Title2;
                evm.Subtitle = el.Subtitle;
                evm.Body_text = el.Body_text;

                log.Info("end");
                return PartialView("_DetailPartial", evm);
            }
        }

        #endregion list 

        #region create

        [HttpGet]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: Extra/Create"))
            {
                log.Info("begin");
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                ExtraViewModel evm = new ExtraViewModel();

                evm.Mode = "create";
                evm.CategoryToList = (new List<MyListItem>() { new MyListItem { Value = null, Text = string.Empty } })
                                            .Union(db.CategoriesLocal
                                                    .Where(x => x.LanguageId == lguid)
                                                    .Select(x => new MyListItem { Value = x.CategoriesGlobalId, Text = x.Name }))
                                            .OrderBy(x => x.Text)
                                            .ToList();

                log.Info("end");
                return View("Edit", evm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create(ExtraViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Contact/Create"))
            {
                log.Info("begin");
                ExtrasGlobal resg = db.ExtrasGlobal.Create();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                if (ModelState.IsValid)
                {                    
                    resg.PublishingDate = null;
                    resg.CategoriesGlobal = db.CategoriesGlobal.Find(model.CategoryId);
                    resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == "Editing");
                    resg.Order = null == model.Order ? 0 : (int)model.Order;
                    db.Entry(resg).State = EntityState.Added;

                    ExtrasLocal resl = db.ExtrasLocal.Create();
                    resl.ExtrasGlobal = resg;
                    resl.Language = db.Language.Find(lguid);
                    resl.Title1 = model.Title1;
                    resl.Title2 = model.Title2;
                    resl.Subtitle = model.Subtitle;
                    resl.Body_text = model.Body_text;

                    db.Entry(resl).State = EntityState.Added;
                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Extra.Edit(resg.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);
                    //listák
                    model.CategoryToList = (new List<MyListItem>() { new MyListItem { Value = null, Text = string.Empty } })
                                                .Union(db.CategoriesLocal
                                                        .Where(x => x.LanguageId == lguid)
                                                        .Select(x => new MyListItem { Value = x.CategoriesGlobalId, Text = x.Name }))
                                                .OrderBy(x => x.Text)
                                                .ToList();

                    return View("Edit", model);
                }
            }
        }

        #endregion create

        #region edit

        [HttpGet]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(Guid id)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: Extra/Edit"))
            {
                log.Info("begin");

                var eg = db.ExtrasGlobal.Find(id);
                ExtraViewModel evm = new ExtraViewModel()
                {
                    Id = eg.Id,
                    //nem szerkeszthető: PublishingDate = eg.PublishingDate,
                    CategoryId = eg.CategoriesGlobal.Id,
                    NewsStatusName = eg.NewsStatus.NameGlobal,
                    Order = eg.Order
                };

                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var el = db.ExtrasLocal.FirstOrDefault(x => x.ExtrasGlobalId == id && x.LanguageId == lguid);
                if (null == el)// ha még nincs ilyen lokalizáció, akkor csinálni kell neki
                {
                    var newel = db.ExtrasLocal.Create();
                    newel.ExtrasGlobalId = id;
                    newel.LanguageId = lguid;
                    db.Entry(newel).State = EntityState.Added;
                    db.SaveChanges();
                }
                else
                {
                    evm.Title1 = el.Title1;
                    evm.Title2 = el.Title2;
                    evm.Subtitle = el.Subtitle;
                    evm.Body_text = el.Body_text;
                }

                evm.CategoryToList = (new List<MyListItem>() { new MyListItem { Value = null, Text = string.Empty } })
                                            .Union(db.CategoriesLocal
                                                    .Where(x => x.LanguageId == lguid)
                                                    .Select(x => new MyListItem { Value = x.CategoriesGlobalId, Text = x.Name }))
                                            .OrderBy(x => x.Text)
                                            .ToList();

                log.Info("end");
                return View(evm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(ExtraViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Extra/Edit"))
            {
                log.Info("begin");
                var resg = db.ExtrasGlobal
                        .Include(x => x.CategoriesGlobal)
                        .Include(x => x.NewsStatus)
                        .Where(x => x.Id == model.Id)
                        .FirstOrDefault();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                if (ModelState.IsValid)
                {                    
                    //if (resg.PublishingDate != model.PublishingDate) { resg.PublishingDate = model.PublishingDate; }
                    if (null == resg.CategoriesGlobal ?
                        null != model.CategoryId :
                        resg.CategoriesGlobal.Id != model.CategoryId) { resg.CategoriesGlobal = db.CategoriesGlobal.Find(model.CategoryId); }
                    //if (null == resg.NewsStatus ?
                    //    null != model.NewsStatusName :
                    //    resg.NewsStatus.NameGlobal != model.NewsStatusName) { resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == model.NewsStatusName); }
                    if (resg.Order != model.Order) { resg.Order = null == model.Order ? 0 : (int)model.Order; }

                    var resl = db.ExtrasLocal.FirstOrDefault(x => x.ExtrasGlobalId == model.Id && x.LanguageId == lguid);
                    if (resl.Title1 != model.Title1) { resl.Title1 = model.Title1; }
                    if (resl.Title2 != model.Title2) { resl.Title2 = model.Title2; }
                    if (resl.Subtitle != model.Subtitle) { resl.Subtitle = model.Subtitle; }
                    if (resl.Body_text != model.Body_text) { resl.Body_text = model.Body_text; }
                    
                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Extra.Edit(model.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);
                    //listák
                    model.CategoryToList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.CategoriesLocal
                                                        .Where(x => x.LanguageId == lguid)
                                                        .Select(x => new MyListItem { Value = x.CategoriesGlobalId, Text = x.Name }))
                                                .OrderBy(x => x.Text)
                                                .ToList();

                    return View(model);
                }
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
        public virtual ActionResult DeleteExtra(Guid id)
        {
            var eg = db.ExtrasGlobal.Find(id);
            if (null != eg)
            {
                //először a lokális cucc a levesbe
                var el = db.ExtrasLocal.Where(x => x.ExtrasGlobalId == id);
                foreach (var item in el.ToList())
                { db.Entry(item).State = EntityState.Deleted; }                
                db.Entry(eg).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = false, response = "Not found" });
        }

        #endregion delete

        #region statusbuttons

        /// <summary>
        /// státuszgombok
        /// </summary>
        /// <param name="id"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult UpdateExtraStatus(Guid? id, string to)
        {
            var sta = db.NewsStatusesGlobal.Where(x => x.NameGlobal == to).FirstOrDefault();
            if (null != id && null != sta)
            {
                var eg = db.ExtrasGlobal.Find(id);
                if (null != eg)
                {
                    eg.NewsStatus = sta;
                    if (to == "Published")
                    { eg.PublishingDate = DateTime.Now; }
                    else
                    { eg.PublishingDate = null; }
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = false, response = "Not found" });
            }
            return Json(new { success = false, error = true, response = "Bad request" });
        }

        #endregion statusbuttons

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




