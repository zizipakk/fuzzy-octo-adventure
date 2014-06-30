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
using System.Net;
using Tax.Portal.Models;
using Newtonsoft.Json;
using System.Threading;
using Tax.Portal.Helpers;


namespace Tax.Portal.Controllers
{
    public partial class EventController : Controller
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
        public virtual System.Web.Mvc.JsonResult ListEvents(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

            var rs = db.EventsGlobal
                                .SelectMany(x => x.EventsLocal.Where(y => y.LanguageId == lguid)
                                    , (x, y) => new { x, y })
                                .SelectMany(z => z.x.NewsStatus.NewsStatusesLocal.Where(v => v.LanguageId == lguid)
                                    , (z, v) => new { z, v })
                        .Select(s => new 
                        {
                            Id = s.z.x.Id,
                            Status = s.v.Name,
                            Title1 = s.z.y.Title1,
                            Title2 = null == s.z.y.Title2 ? "" : s.z.y.Title2,
                            PublishingDate = s.z.x.PublishingDate,
                            Date = s.z.x.Date
                        }).AsQueryable().GridPage(grid, out result);

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
                                ,r.PublishingDate.ToString()
                                ,r.Date.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult FlashEvent(Guid id)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: New/FlashNew"))
            {
                log.Info("begin");

                var eg = db.EventsGlobal.Find(id);
                EventViewModel evm = new EventViewModel()
                {
                    Id = eg.Id,
                    PublishingDate = eg.PublishingDate,
                    Date = eg.Date,
                    NewsStatusName = eg.NewsStatus.NameGlobal
                };

                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var el = db.EventsLocal.FirstOrDefault(x => x.EventsGlobalId == id && x.LanguageId == lguid);
                evm.Title1 = el.Title1;
                evm.Title2 = el.Title2;
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
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: Event/Create"))
            {
                log.Info("begin");

                EventViewModel evm = new EventViewModel();

                evm.Mode = "create";

                log.Info("end");
                return View("Edit", evm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create(EventViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Event/Create"))
            {
                log.Info("begin");
                EventsGlobal resg = db.EventsGlobal.Create();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                if (ModelState.IsValid)
                {                    
                    resg.PublishingDate = null;
                    resg.Date = model.Date;
                    resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == "Editing");
                    db.Entry(resg).State = EntityState.Added;
                    
                    EventsLocal resl = db.EventsLocal.Create();
                    resl.EventsGlobal = resg;
                    resl.Language = db.Language.Find(lguid);
                    resl.Title1 = model.Title1;
                    resl.Title2 = model.Title2;
                    resl.Body_text = model.Body_text;

                    db.Entry(resl).State = EntityState.Added;
                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Event.Edit(resg.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);

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
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: Event/Edit"))
            {
                log.Info("begin");

                var eg = db.EventsGlobal.Find(id);
                EventViewModel evm = new EventViewModel()
                {
                    Id = eg.Id,
                    //nem módosítható PublishingDate = eg.PublishingDate,
                    Date = eg.Date,
                    NewsStatusName = eg.NewsStatus.NameGlobal
                };

                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var el = db.EventsLocal.FirstOrDefault(x => x.EventsGlobalId == id && x.LanguageId == lguid);
                //if (null == el)// ha még nincs ilyen lokalizáció, akkor csinálni kell neki
                //{
                //    var newel = db.EventsLocal.Create();
                //    newel.EventsGlobalId = id;
                //    newel.LanguageId = lguid;
                //    db.Entry(newel).State = EntityState.Added;
                //    db.SaveChanges();
                //}
                //else
                //{
                //    evm.Title1 = el.Title1;
                //    evm.Title2 = el.Title2;
                //    evm.Body_text = el.Body_text;
                //}
                if (null != el)// ha már van ilyen lokalizáció
                {
                    evm.Title1 = el.Title1;
                    evm.Title2 = el.Title2;
                    evm.Body_text = el.Body_text;
                }

                log.Info("end");
                return View(evm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(EventViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Event/Edit"))
            {
                log.Info("begin");
                var resg = db.EventsGlobal
                        .Include(x => x.NewsStatus)
                        .Where(x => x.Id == model.Id)
                        .FirstOrDefault();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                if (ModelState.IsValid)
                {                    
                    //if (resg.PublishingDate != model.PublishingDate) { resg.PublishingDate = model.PublishingDate; }
                    if (resg.Date != model.Date) { resg.Date = model.Date; }
                    //if (null == resg.NewsStatus ?
                    //    null != model.NewsStatusName :
                    //    resg.NewsStatus.NameGlobal != model.NewsStatusName) { resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == model.NewsStatusName); }

                    var resl = db.EventsLocal.FirstOrDefault(x => x.EventsGlobalId == model.Id && x.LanguageId == lguid);
                    //if (resl.Title1 != model.Title1) { resl.Title1 = model.Title1; }
                    //if (resl.Title2 != model.Title2) { resl.Title2 = model.Title2; }
                    //if (resl.Body_text != model.Body_text) { resl.Body_text = model.Body_text; }
                    if (null == resl)// ha még nincs ilyen lokalizáció, akkor csinálni kell neki
                    {
                        resl = db.EventsLocal.Create();
                        resl.EventsGlobalId = model.Id;
                        resl.LanguageId = lguid;
                        resl.Title1 = model.Title1;
                        resl.Title2 = model.Title2;
                        resl.Body_text = model.Body_text;
                        db.Entry(resl).State = EntityState.Added;
                    }
                    else
                    {
                        if (resl.Title1 != model.Title1) { resl.Title1 = model.Title1; }
                        if (resl.Title2 != model.Title2) { resl.Title2 = model.Title2; }
                        if (resl.Body_text != model.Body_text) { resl.Body_text = model.Body_text; }
                    }
                    
                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Event.Edit(model.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);

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
        public virtual ActionResult DeleteEvent(Guid id)
        {
            var eg = db.EventsGlobal.Find(id);
            if (null != eg)
            {
                //először a lokális cucc a levesbe
                var el = db.EventsLocal.Where(x => x.EventsGlobalId == id);
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
        public virtual ActionResult UpdateEventStatus(Guid? id, string to)
        {
            var sta = db.NewsStatusesGlobal.Where(x => x.NameGlobal == to).FirstOrDefault();
            if (null != id && null != sta)
            {
                var eg = db.EventsGlobal.Find(id);
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




