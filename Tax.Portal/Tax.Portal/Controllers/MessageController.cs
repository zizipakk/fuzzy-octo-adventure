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
    public partial class MessageController : Controller
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
        public virtual System.Web.Mvc.JsonResult ListMessages(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
            Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
            Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);

            var rs = db.MessagesGlobal
                                .SelectMany(x => x.MessagesLocal.Where(y => y.LanguageId == lguidEn)
                                    , (x, y) => new { x, y })
                                .SelectMany(x => x.x.MessagesLocal.Where(y => y.LanguageId == lguidHu)
                                    , (x, y) => new { x, y })
                                .SelectMany(z => z.x.x.NewsStatus.NewsStatusesLocal.Where(v => v.LanguageId == lguid)
                                    , (z, v) => new { z, v })

                        .ToList()
                        .Select(s => new 
                        {
                            Id = s.z.x.x.Id,
                            Status = s.v.Name,
                            MessageEn = s.z.x.y.Message,
                            MessageHu = s.z.y.Message,
                            PublishingDate = s.z.x.x.PublishingDate,
                            Response = s.z.x.x.ServiceResponse
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
                                ,r.Status
                                ,r.MessageEn
                                ,r.MessageHu
                                ,r.PublishingDate.ToString()
                                ,r.Response
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion list 

        #region create

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create(string messageEn, string messageHu)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Message/Create"))
            {
                log.Info("begin");
                if ("" == messageEn)
                {
                    return Json(new { success = false, error = false, response = "[MessageENG] is required" });
                }
                if ("" == messageHu)
                {
                    return Json(new { success = false, error = false, response = "[MessageHUN] is required" });
                }
                int minlenght = 3;
                if (messageEn.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The MessageENG must be at least {0} characters long.", minlenght) });
                }
                if (messageHu.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The MessageHUN must be at least {0} characters long.", minlenght) });
                }

                MessagesGlobal resg = db.MessagesGlobal.Create();
                resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == "Editing");
                resg.PublishingDate = null;
                resg.ServiceResponse = null;
                db.Entry(resg).State = EntityState.Added;

                //string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
                MessagesLocal reslEn = db.MessagesLocal.Create();
                reslEn.MessagesGlobal = resg;
                reslEn.Language = db.Language.Find(lguidEn);
                reslEn.Message = messageEn;
                db.Entry(reslEn).State = EntityState.Added;
                Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);
                MessagesLocal reslHu = db.MessagesLocal.Create();
                reslHu.MessagesGlobal = resg;
                reslHu.Language = db.Language.Find(lguidHu);
                reslHu.Message = messageHu;
                db.Entry(reslHu).State = EntityState.Added;
                db.SaveChanges();
                log.Info("end with ok");

                return Json(new { success = true, error = false, response = "" });
            }
        }

        #endregion create

        #region edit

        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(string id, string oper, string MessageEn, string MessageHu)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Message/Edit"))
            {
                Guid gid = Guid.Parse(id);
                log.Info("begin");
                MessagesGlobal resg;

                switch (oper)
                {
                    case "edit":
                        resg = db.MessagesGlobal.Find(gid);
                        Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
                        MessagesLocal reslEn = db.MessagesLocal.FirstOrDefault(x => x.MessagesGlobalId == gid && x.LanguageId == lguidEn);
                        if (reslEn.Message != MessageEn) { reslEn.Message = MessageEn; }
                        Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);
                        MessagesLocal reslHu = db.MessagesLocal.FirstOrDefault(x => x.MessagesGlobalId == gid && x.LanguageId == lguidHu);
                        if (reslHu.Message != MessageHu) { reslHu.Message = MessageHu; }
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
        public virtual ActionResult DeleteMessage(Guid id)
        {
            var mg = db.MessagesGlobal.Find(id);
            if (null != mg)
            {
                //először a lokális cucc a levesbe
                var ml = db.MessagesLocal.Where(x => x.MessagesGlobalId == id);
                foreach (var item in ml.ToList())
                { db.Entry(item).State = EntityState.Deleted; }                
                db.Entry(mg).State = EntityState.Deleted;
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
        public virtual ActionResult UpdateMessageStatus(Guid? id, string to)
        {
            var sta = db.NewsStatusesGlobal.Where(x => x.NameGlobal == to).FirstOrDefault();
            if (null != id && null != sta)
            {
                var m = db.MessagesGlobal.Find(id);
                if (null != m)
                {
                    m.NewsStatus = sta;
                    if (to == "Published")
                    { 
//TODO: ide gyünnek a push hívások
                        m.ServiceResponse = "?OK?";
                        m.PublishingDate = DateTime.Now.Date; 
                    }
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




