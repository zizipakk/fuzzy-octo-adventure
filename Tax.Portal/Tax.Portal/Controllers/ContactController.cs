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
    public partial class ContactController : Controller
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
        public virtual System.Web.Mvc.JsonResult ListContacts(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

            var rs0 = db.ContactsGlobal
                                .SelectMany(x => x.ContactsLocal.Where(y => y.LanguageId == lguid)
                                    , (x, y) => new { x, y })
                                .SelectMany(z => z.x.NewsStatus.NewsStatusesLocal.Where(v => v.LanguageId == lguid)
                                    , (z, v) => new { z, v })
                        .ToList()
                        .Select(s => new 
                        {
                            Id = s.z.x.Id,
                            Status = s.v.Name,
                            First_name = s.z.x.First_name,
                            Last_name = s.z.x.Last_name,
                            Department = s.z.y.Department,
                            Position = s.z.y.Position,
                            Phone = s.z.x.Phone,
                            Mobile = s.z.x.Mobile,
                            Email = s.z.x.Email
                        })
                        .AsEnumerable();

            var rs00 = db.ContactsGlobal
                            .SelectMany(v => db.TagsLocal.Where(z => v.TagsGlobal.Contains(z.TagsGlobal) && z.LanguageId == lguid)
                                , (v, z) => new { Id = v.Id, Name = z.Name })
                            .ToList()
                            .GroupBy(g => g.Id)
                            .Select(g => new 
                                {
                                    Id = g.FirstOrDefault().Id,
                                    Tags = (g.Select(ss => ss.Name)).Aggregate((a, b) => (a == "" ? "" : a + ", ") + b)
                                }
                            )
                            .AsEnumerable();

            var rs = rs0
                        .SelectMany(a => rs00.Where(b => b.Id == a.Id).DefaultIfEmpty(), (a, b) => new
                            {
                                Id = a.Id,
                                Status = a.Status,
                                First_name = a.First_name,
                                Last_name = a.Last_name,
                                Department = a.Department,
                                Position = a.Position,
                                Phone = a.Phone,
                                Mobile = a.Mobile,
                                Email = a.Email,
                                Tags = null == b ? "" : b.Tags
                            }
                        )
                        .AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                r.Id.ToString()
                                ,r.Status
                                ,r.First_name
                                ,r.Last_name
                                ,r.Department
                                ,r.Position
                                ,r.Phone
                                ,r.Mobile
                                ,r.Email
                                ,r.Tags
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion list 

        #region create

        [HttpGet]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: Contact/Create"))
            {
                log.Info("begin");
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                ContactViewModel cvm = new ContactViewModel();

                cvm.Mode = "create";
                cvm.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.TagsLocal
                                                    .Where(x => x.LanguageId == lguid)
                                                    .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                cvm.TagsIn = new Guid[] { };
                cvm.TagToList = new List<MyListItem>();

                log.Info("end");
                return View("Edit", cvm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create(ContactViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Contact/Create"))
            {
                log.Info("begin");
                ContactsGlobal resg = db.ContactsGlobal.Create();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                if (null == model.TagsIn)
                { model.TagsIn = new Guid[] { }; }

                if (ModelState.IsValid)
                {                    
                    resg.PublishingDate = null;
                    resg.Photo = db.File.Find(model.PhotoId);
                    resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == "Editing");
                    resg.First_name = model.First_name;
                    resg.Last_name = model.Last_name;
                    resg.Linkedin = model.Linkedin;
                    resg.Phone = model.Phone;
                    resg.Mobile = model.Mobile;
                    resg.Email = model.Email;

                    foreach (var tg in db.TagsGlobal.Where(x => model.TagsIn.Contains(x.Id)).ToList())
                    {
                        resg.TagsGlobal.Add(tg);
                    }
                    db.Entry(resg).State = EntityState.Added;

                    ContactsLocal resl = db.ContactsLocal.Create();
                    resl.ContactsGlobal = resg;
                    resl.Language = db.Language.Find(lguid);
                    resl.Department = model.Department;
                    resl.Position = model.Position;
                    resl.Profile = model.Profile;

                    db.Entry(resl).State = EntityState.Added;
                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Contact.Edit(resg.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);
                    //listák
                    //model.TagsOut = db.TagsGlobal
                    //                .Where(z => !model.TagsIn.Contains(z.Id))
                    //                .Select(x => x.Id)
                    //                .ToArray();
                    model.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.TagsLocal
                                                        .Where(z => !model.TagsIn.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                                        .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.TagToList = db.TagsLocal
                                        .Where(z => model.TagsIn.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                        .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name })
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
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: Contact/Edit"))
            {
                log.Info("begin");

                var cg = db.ContactsGlobal.Find(id);
                ContactViewModel cvm = new ContactViewModel()
                {
                    Id = cg.Id,
                    //nem módosítható PublishingDate = cg.PublishingDate,
                    PhotoId = null == cg.Photo ? null : (Guid?)cg.Photo.stream_id,
                    NewsStatusName = cg.NewsStatus.NameGlobal,
                    First_name = cg.First_name,
                    Last_name = cg.Last_name,
                    Linkedin = cg.Linkedin,
                    Phone = cg.Phone,
                    Mobile = cg.Mobile,
                    Email = cg.Email
                };

                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var cl = db.ContactsLocal.FirstOrDefault(x => x.ContactsGlobalId == id && x.LanguageId == lguid);
                if (null == cl)// ha még nincs ilyen lokalizáció, akkor csinálni kell neki
                {
                    var newcl = db.ContactsLocal.Create();
                    newcl.ContactsGlobalId = id;
                    newcl.LanguageId = lguid;
                    db.Entry(newcl).State = EntityState.Added;
                    db.SaveChanges();
                }
                else
                {
                    cvm.Department = cl.Department;
                    cvm.Position = cl.Position;
                    cvm.Profile = cl.Profile;
                }

                var ngList = cg.TagsGlobal.Select(v => v.Id).ToList();
                //cvm.TagsOut = db.TagsGlobal
                //                .Where(z => !ngList.Contains(z.Id))
                //                .Select(x => x.Id)
                //                .ToArray();
                cvm.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.TagsLocal
                                                    .Where(z => !ngList.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                                    .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                cvm.TagsIn = db.TagsGlobal
                                .Where(z => ngList.Contains(z.Id))
                                .Select(x => x.Id)
                                .ToArray();
                cvm.TagToList = db.TagsLocal
                                    .Where(z => ngList.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                    .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name })
                                    .OrderBy(x => x.Text)
                                    .ToList();

                log.Info("end");
                return View(cvm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(ContactViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Contact/Edit"))
            {
                log.Info("begin");
                var resg = db.ContactsGlobal
                        .Include(x => x.Photo)
                        .Include(x => x.NewsStatus)
                        .Where(x => x.Id == model.Id)
                        .FirstOrDefault();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                if (null == model.TagsIn)
                { model.TagsIn = new Guid[] { }; }

                if (ModelState.IsValid)
                {                    
                    //if (resg.PublishingDate != model.PublishingDate) { resg.PublishingDate = model.PublishingDate; }
                    if (null == resg.Photo ?
                        null != model.PhotoId :
                        resg.Photo.stream_id != model.PhotoId) 
                    {
                        if (null != resg.Photo && null == model.PhotoId)
                        {
                            var photo = db.File.Find(resg.Photo.stream_id);
                            db.Entry(photo).State = EntityState.Deleted;
                        }
                        resg.Photo = db.File.Find(model.PhotoId);
                    }
                    //if (null == resg.NewsStatus ?
                    //    null != model.NewsStatusName :
                    //    resg.NewsStatus.NameGlobal != model.NewsStatusName) { resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == model.NewsStatusName); }
                    if (resg.First_name != model.First_name) { resg.First_name = model.First_name; }
                    if (resg.Last_name != model.Last_name) { resg.Last_name = model.Last_name; }
                    if (resg.Linkedin != model.Linkedin) { resg.Linkedin = model.Linkedin; }
                    if (resg.Phone != model.Phone) { resg.Phone = model.Phone; }
                    if (resg.Mobile != model.Mobile) { resg.Mobile = model.Mobile; }
                    if (resg.Email != model.Email) { resg.Email = model.Email; }

                    var resl = db.ContactsLocal.FirstOrDefault(x => x.ContactsGlobalId == model.Id && x.LanguageId == lguid);
                    if (resl.Department != model.Department) { resl.Department = model.Department; }
                    if (resl.Position != model.Position) { resl.Position = model.Position; }
                    if (resl.Profile != model.Profile) { resl.Profile = model.Profile; }
                    
                    //nem szerepel a db contextben a many-to-many, ezért be kell tölteni
                    db.Entry(resg).Collection(t => t.TagsGlobal).Load();

                    IEnumerable<Guid> removeGuids = resg.TagsGlobal.Select(x => x.Id).Except(model.TagsIn);
                    foreach (var tg in db.TagsGlobal.Where(x => removeGuids.Contains(x.Id)).ToList())
                    {
                        resg.TagsGlobal.Remove(tg);
                    }

                    IEnumerable<Guid> addGuids = model.TagsIn.Except(resg.TagsGlobal.Select(x => x.Id));
                    foreach (var tg in db.TagsGlobal.Where(x => addGuids.Contains(x.Id)).ToList())
                    {
                        resg.TagsGlobal.Add(tg);
                    }

                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Contact.Edit(model.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);
                    //listák
                    //model.TagsOut = db.TagsGlobal
                    //                .Where(z => !model.TagsIn.Contains(z.Id))
                    //                .Select(x => x.Id)
                    //                .ToArray();
                    model.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.TagsLocal
                                                        .Where(z => !model.TagsIn.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                                        .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.TagToList = db.TagsLocal
                                        .Where(z => model.TagsIn.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                        .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name })
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
        public virtual ActionResult DeleteContact(Guid id)
        {
            var cg = db.ContactsGlobal.Find(id);
            if (null != cg)
            {
                //először a lokális cucc a levesbe
                var cl = db.ContactsLocal.Where(x => x.ContactsGlobalId == id);
                foreach (var item in cl.ToList())
                { db.Entry(item).State = EntityState.Deleted; }                
                db.Entry(cg).State = EntityState.Deleted;
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
        public virtual ActionResult UpdateContactStatus(Guid? id, string to)
        {
            var sta = db.NewsStatusesGlobal.Where(x => x.NameGlobal == to).FirstOrDefault();
            if (null != id && null != sta)
            {
                var cg = db.ContactsGlobal.Find(id);
                if (null != cg)
                {
                    cg.NewsStatus = sta;
                    if (to == "Published")
                    { cg.PublishingDate = DateTime.Now; }
                    else
                    { cg.PublishingDate = null; }
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




