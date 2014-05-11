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

        #region list

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual System.Web.Mvc.JsonResult ListNews(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

            var rs0 = db.NewsGlobal
                                .SelectMany(x => x.NewsLocal.Where(y =>
                                                                    //y.NewsGlobalId == x.Id
                                                                    //&& 
                                                                    y.LanguageId == lguid), (x, y) => new { x, y })
                                .SelectMany(z => z.x.NewsStatus.NewsStatusesLocal.Where(v => v.LanguageId == lguid)
                                    , (z, v) => new { z, v })
                        .ToList()
                        .Select(s => new 
                        {
                            Id = s.z.x.Id,
                            Status = s.v.Name,
                            Title1 = s.z.y.Title1,
                            Title2 = s.z.y.Title2,
                            PublishingDate = null == s.z.x.PublishingDate ? DateTime.MinValue : (DateTime)s.z.x.PublishingDate.Value.Date,
                            Thumbnail = null == s.z.x.Thumbnail ? Guid.Empty : s.z.x.Thumbnail.stream_id
                        }).AsEnumerable();


            var rs00 = db.NewsGlobal
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

            var rs = rs0.SelectMany(a => rs00.Where(b => b.Id == a.Id).DefaultIfEmpty(), (a, b) => new
                            {
                                Id = a.Id,
                                Status = a.Status,
                                Title1 = a.Title1,
                                Title2 = a.Title2,
                                Tags = null == b ? "" : b.Tags,
                                PublishingDate = a.PublishingDate,
                                Thumbnail = a.Thumbnail
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
                                ,r.Title1
                                ,r.Title2
                                ,r.Tags
                                ,DateTime.MinValue == r.PublishingDate ? "" : r.PublishingDate.ToString()
                                ,r.Thumbnail.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lenyílók a gridhez
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual string ListNewsStatus(bool? isSearch)
        {
            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

            string result = string.Empty;
            
            result = "<select>";

            if (true == isSearch) //üres sor, ha kereséshez kell
            {
                result += string.Format("<option value=''></option>");
            }

            foreach (var r in db.NewsStatusesLocal
                                .Where(x => x.LanguageId == lguid)
                                .Select(x => new
                                    {
                                    value = x.NewsStatusGlobalId,
                                    text = x.Name
                                    }
                                ).OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", true == isSearch ? r.text : r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        #endregion list 

        #region edit

        [HttpGet]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(Guid id)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET New/Edit"))
            {
                log.Info("begin");
                
                var ng = db.NewsGlobal.Find(id);
                
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var nl = db.NewsLocal.FirstOrDefault(x => x.NewsGlobalId == id && x.LanguageId == lguid);
                
                NewViewModel suvm;
                if (null == nl)// ha még nincs ilyen lokalizáció, akkor csinálni kell neki
                {
                    var newnl = db.NewsLocal.Create();
                    newnl.NewsGlobalId = id;
                    newnl.LanguageId = lguid;
                    db.Entry(newnl).State = EntityState.Added;
                    db.SaveChanges();
                    suvm = new NewViewModel(ng, newnl);
                }
                else
                {
                    suvm = new NewViewModel(ng, nl);
                }

                suvm.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.TagsLocal.Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.TagToList = ng.TagsGlobal.Select(x => 
                                            new MyListItem { Value = x.Id, Text = x.TagsLocal.FirstOrDefault(z => z.LanguageId == lguid).Name })
                                            .OrderBy(x => x.Text)
                                            .ToList();

                log.Info("end");
                return View(suvm);
            }
        }


        //[HttpPost]
        //[Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        //public virtual ActionResult Edit(SinoszUserViewModel model)
        //{
        //    using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/Edit"))
        //    {
        //        log.Info("begin");

        //        if (ModelState.IsValid)
        //        {
        //            SinoszUser resm = model.SinoszUserModel;
        //            //csak így lehet módosítani a nav. prop-ot
        //            var res = db.SinoszUser
        //                .Include(o => o.Organization)
        //                .Include(o => o.Postcode)
        //                .Include(g => g.Genus)
        //                .Include(h => h.HearingStatus)
        //                .Include(p => p.PensionType)
        //                .Include(n => n.Nation)
        //                .Include(po => po.Position)
        //                .Include(m => m.MaritalStatus)
        //                .Include(i => i.InjuryTime)
        //                .Include(e => e.Education)
        //                .Include(r => r.Relationship)
        //                .Include(s => s.SinoszUserStatus)
        //                .Where(x => x.Id == resm.Id)
        //                .FirstOrDefault();
        //            if (null == res.Organization ?
        //                Guid.Empty != resm.Organization.Id :
        //                res.Organization.Id != resm.Organization.Id)  //csak itt van adat a modellben
        //            {
        //                //SinoszLog-írás
        //                var org = db.Organization.Find(resm.Organization.Id);
        //                var slog = db.SinoszLog.Create();
        //                slog.ApplicationUser = db.Users.SingleOrDefault(x => x.UserName == User.Identity.Name); //aki éppen be van lépve
        //                slog.SinoszUser = res;
        //                slog.ActionName = string.Format("Módosítás: {0} => {1}", null == res.Organization ? "" : res.Organization.OrganizationName, null == org ? "" : org.OrganizationName);
        //                slog.ActionTime = DateTime.Now;
        //                db.Entry(slog).State = EntityState.Added;
        //                //nav prop beállítása
        //                res.Organization = org;
        //            }
        //            if (null == res.Postcode ?
        //                Guid.Empty != resm.Postcode.Id :
        //                res.Postcode.Id != resm.Postcode.Id) { res.Postcode = db.Postcode.Find(resm.Postcode.Id); }
        //            if (null == res.Genus ?
        //                Guid.Empty != resm.Genus.Id :
        //                res.Genus.Id != resm.Genus.Id) { res.Genus = db.Genus.Find(resm.Genus.Id); }
        //            if (null == res.HearingStatus ?
        //                Guid.Empty != resm.HearingStatus.Id :
        //                res.HearingStatus.Id != resm.HearingStatus.Id) { res.HearingStatus = db.HearingStatus.Find(resm.HearingStatus.Id); }
        //            if (null == res.PensionType ?
        //                Guid.Empty != resm.PensionType.Id :
        //                res.PensionType.Id != resm.PensionType.Id) { res.PensionType = db.PensionType.Find(resm.PensionType.Id); }
        //            if (null == res.Nation ?
        //                Guid.Empty != resm.Nation.Id :
        //                res.Nation.Id != resm.Nation.Id) { res.Nation = db.Nation.Find(resm.Nation.Id); }
        //            if (null == res.Position ?
        //                Guid.Empty != resm.Position.Id :
        //                res.Position.Id != resm.Position.Id) { res.Position = db.Position.Find(resm.Position.Id); }
        //            if (null == res.MaritalStatus ?
        //                Guid.Empty != resm.MaritalStatus.Id :
        //                res.MaritalStatus.Id != resm.MaritalStatus.Id) { res.MaritalStatus = db.MaritalStatus.Find(resm.MaritalStatus.Id); }
        //            if (null == res.InjuryTime ?
        //                Guid.Empty != resm.InjuryTime.Id :
        //                res.InjuryTime.Id != resm.InjuryTime.Id) { res.InjuryTime = db.InjuryTime.Find(resm.InjuryTime.Id); }
        //            if (null == res.Education ?
        //                Guid.Empty != resm.Education.Id :
        //                res.Education.Id != resm.Education.Id) { res.Education = db.Education.Find(resm.Education.Id); }
        //            //if (null == res.Relationship ?
        //            //    Guid.Empty != resm.Relationship.Id :
        //            //    res.Relationship.Id != resm.Relationship.Id) { res.Relationship = db.Relationship.Find(resm.Relationship.Id); }
        //            if (null == res.SinoszUserStatus ?
        //                Guid.Empty != resm.SinoszUserStatus.Id :
        //                res.SinoszUserStatus.Id != resm.SinoszUserStatus.Id) { res.SinoszUserStatus = db.SinoszUserStatus.Find(resm.SinoszUserStatus.Id); }
        //            //már ez is módosulhat
        //            if (res.SinoszId != resm.SinoszId) { res.SinoszId = resm.SinoszId; }
        //            //egyéb
        //            if (res.BirthDate != resm.BirthDate) { res.BirthDate = resm.BirthDate; }
        //            if (res.BirthName != resm.BirthName) { res.BirthName = resm.BirthName; }
        //            if (res.BirthPlace != resm.BirthPlace) { res.BirthPlace = resm.BirthPlace; }
        //            if (res.DecreeNumber != resm.DecreeNumber) { res.DecreeNumber = resm.DecreeNumber; }
        //            if (res.EnterDate != resm.EnterDate) { res.EnterDate = resm.EnterDate; }
        //            if (res.HomeAddress != resm.HomeAddress) { res.HomeAddress = resm.HomeAddress; }
        //            if (res.isHearingAid != resm.isHearingAid) { res.isHearingAid = resm.isHearingAid; }
        //            if (res.isImplant != resm.isImplant) { res.isImplant = resm.isImplant; }
        //            if (res.MothersName != resm.MothersName) { res.MothersName = resm.MothersName; }
        //            if (res.Remark != resm.Remark) { res.Remark = resm.Remark; }
        //            if (res.SinoszId != resm.SinoszId) { res.SinoszId = resm.SinoszId; }
        //            if (res.SinoszUserName != resm.SinoszUserName) { res.SinoszUserName = resm.SinoszUserName; }
        //            if (res.Barcode != resm.Barcode) { res.Barcode = resm.Barcode; }

        //            db.SaveChanges();
        //            log.Info("end with ok");

        //            return RedirectToAction(MVC.Sinosz.Edit(model.SinoszUserModel.Id, null));
        //        }
        //        else
        //        {
        //            //TODO béna megoldás átmenetileg, mert nem tudom különben átírni a hibaüzenetet, ami angol
        //            if (ModelState["SinoszUserModel.BirthDate"].Errors.Count() > 0
        //                && !ModelState["SinoszUserModel.BirthDate"].Errors.Select(x => x.ErrorMessage).Contains("A(z) [Születési idő] mezőt kötelező kitölteni"))
        //            {
        //                ModelState["SinoszUserModel.BirthDate"].Errors.Clear();
        //                ModelState["SinoszUserModel.BirthDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Születési dátum] mezőben!"));
        //            }
        //            if (ModelState["SinoszUserModel.EnterDate"].Errors.Count() > 0
        //                && !ModelState["SinoszUserModel.EnterDate"].Errors.Select(x => x.ErrorMessage).Contains("A(z) [Belépés ideje] mezőt kötelező kitölteni"))
        //            {
        //                ModelState["SinoszUserModel.EnterDate"].Errors.Clear();
        //                ModelState["SinoszUserModel.EnterDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Belépési dátum] mezőben!"));
        //            }
        //            //

        //            // If we got this far, something failed, redisplay form
        //            log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
        //            log.Info("end with validation error");
        //            model.Refresh(ModelState);

        //            //lenyílókat megint inicializálni kell
        //            string userId = User.Identity.GetUserId();
        //            Guid? orgId = null;
        //            bool isSinoszUser = false;
        //            var ur = db.ApplicationUserRole.FirstOrDefault(x =>
        //                                                                x.UserId == userId
        //                                                                && x.Role.Name == "SinoszUser");
        //            if (null != ur)
        //            {
        //                isSinoszUser = true;
        //                var kur = ur.KontaktUserRole;
        //                if (null != kur)
        //                {
        //                    var org = kur.Organization;
        //                    if (null != org)
        //                    {
        //                        orgId = org.Id;
        //                    }
        //                }
        //            }
        //            model.OrganizationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.Organization
        //                                                .Where(x => !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
        //                                                || (null != orgId && x.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
        //                                                || (null != x.UpperOrganization && x.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
        //                                                )
        //                                                .Select(x => new MyListItem { Value = x.Id, Text = x.OrganizationName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.PostcodeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.Postcode.Select(x => new MyListItem { Value = x.Id, Text = x.Code + "-" + x.City }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.GenusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.Genus.Select(x => new MyListItem { Value = x.Id, Text = x.GenusName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.HearingStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.HearingStatus.Select(x => new MyListItem { Value = x.Id, Text = x.HearingStatusName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.PensionTypeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.PensionType.Select(x => new MyListItem { Value = x.Id, Text = x.PensionTypeName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.NationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.Nation.Select(x => new MyListItem { Value = x.Id, Text = x.NationText }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.PositionList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.Position.Select(x => new MyListItem { Value = x.Id, Text = x.PositionName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.MaritalStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.MaritalStatus.Select(x => new MyListItem { Value = x.Id, Text = x.MaritalStatusName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.InjuryTimeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.InjuryTime.Select(x => new MyListItem { Value = x.Id, Text = x.InjuryTimeText }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            model.EducationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.Education.Select(x => new MyListItem { Value = x.Id, Text = x.EducationName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            //model.RelationshipList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //            //                            .Union(db.Relationship.Select(x => new MyListItem { Value = x.Id, Text = x.RelationshipName }))
        //            //                            .OrderBy(x => x.Text)
        //            //                            .ToList();
        //            model.SinoszUserStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
        //                                        .Union(db.SinoszUserStatus.Select(x => new MyListItem { Value = x.Id, Text = x.StatusName }))
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            //egyszerűbb lsita az állománytípusokhoz
        //            model.FileTypeList = db.FileType.Select(x => new MyListItem { Value = x.Id, Text = x.FileTypeName })
        //                                        .OrderBy(x => x.Text)
        //                                        .ToList();
        //            //ez action-ön kívül változhat
        //            var af = db.AttachedFile.FirstOrDefault(x => x.SinoszUser.Id == model.SinoszUserModel.Id && x.FileType == null);
        //            model.fileId = null == af ? null : (Guid?)af.FileId;

        //            return View(model);
        //        }
        //    }
        //}

        #endregion edit

        #region delete

        /// <summary>
        /// Törlés
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult DeleteNew(Guid id)
        {
            var ng = db.NewsGlobal.Find(id);
            if (null != ng)
            {
                //először a lokális cucc a levesbe
                var nl = db.NewsLocal.Where(x => x.NewsGlobalId == id);
                foreach (var item in nl.ToList())
                { db.Entry(item).State = EntityState.Deleted; }                
                db.Entry(ng).State = EntityState.Deleted;
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
        public virtual ActionResult UpdateNewStatus(Guid? id, string to)
        {
            var sta = db.NewsStatusesGlobal.Where(x => x.NameGlobal == to).FirstOrDefault();
            if (null != id && null != sta)
            {
                var n = db.NewsGlobal.Find(id);
                if (null != n)
                {
                    n.NewsStatus = sta;
                    if (to == "Published")
                    { n.PublishingDate = DateTime.Now.Date; }
                    else
                    { n.PublishingDate = null; }
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = false, response = "Not found" });
            }
            return Json(new { success = false, error = true, response = "Bad request" });
        }

        #endregion statusbuttons

        #region filestream

        private Task<System.Web.Mvc.JsonResult> UploadTask(Guid id, HttpPostedFileBase hpf)
        {
            return Task.Run(() =>
            {
                //Az internet explorer miatt itt ügyeskedni kell
                var path = hpf.FileName.Split('\\');
                var info = path[path.Count() - 1];

                byte[] myBinary = new byte[hpf.InputStream.Length];
                hpf.InputStream.Read(myBinary, 0, myBinary.Length);

                //ideiglenes megoldás directory kezelés helyett
                info = string.Format("{0}_{1}", id.ToString(), info);

                var sf = new File() { stream_id = id, name = info, file_stream = myBinary };
                db.File.Add(sf);
                db.SaveChanges();

                return Json("OK");
            });
        }

        [Authorize(Roles = "SysAdmin, User")]
        public async Task<System.Web.Mvc.JsonResult> Upload(HttpPostedFileBase file, Guid? filetypeId, Guid? suserId)
        {
            try
            {
                //fájlméret validálás
                if (null != file)
                {
                    if (file.ContentLength == 0)
                    {
                        //TODO: a nem express IIS nem engedi át a server errort, de az 500-ast kinyitottuk a web.configban, ezért csak 500-ast adok vissza
                        Response.StatusCode = 500;// (int)HttpStatusCode.InternalServerError;
                        return Json("A fájlméret nem lehet 0!");
                    }
                    if (file.ContentLength > 10485760)
                    {
                        Response.StatusCode = 500;// (int)HttpStatusCode.InternalServerError;
                        return Json("A fájlméret nem lehet nagyobb mint 10 MB!");
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json("Nem sikerült a feltöltés!");
                }

                //csak fényképnél
                if (null == filetypeId)
                {
                    //a megadott kiterjesztést nézem
                    if (file.ContentType.ToLower() != "image/jpg"
                        &&
                        file.ContentType.ToLower() != "image/jpeg")
                    {
                        Response.StatusCode = 500;//(int)HttpStatusCode.InternalServerError;
                        return Json("A megadott fájl típust nem lehet feltölteni! (Elfogadott típusok: jpg, jpeg)!");
                    }

                    //megpróbjálom konvertálni képpé
                    System.Drawing.Image image = null;
                    try
                    {
                        //image = System.Drawing.Image.FromStream(filetemp.InputStream);
                        image = System.Drawing.Image.FromStream(file.InputStream);
                    }
                    catch (Exception e)
                    {
                        Response.StatusCode = 500;// (int)HttpStatusCode.InternalServerError;
                        return Json("A megadott fájl kiterjesztése nem felel meg a tartalmának! (Elfogadott típusok: jpg, jpeg)!");
                    }

                    //akkor ez kép, nézem a méretét
                    if (null != image)
                    {
                        if (image.Width < 195
                            || image.Width > 200
                            || image.Height < 255
                            || image.Height > 260)
                        {
                            Response.StatusCode = 500;// (int)HttpStatusCode.InternalServerError;
                            return Json("A feltölteni kívánt kép mérete eltér a kívánttól (200x260 pixel)!");
                        }
                    }
                    else
                    {
                        Response.StatusCode = 500;// (int)HttpStatusCode.InternalServerError;
                        return Json("A fénykép beolvasása nem sikerült!");
                    }
                    //a konverzió miatt vissza kell állni a filestream elejére
                    file.InputStream.Position = 0;
                }

                Guid id = Guid.NewGuid();//most én csinálom, mert a képról nem töröl a dropzone
                var uploadTask = UploadTask(id, file);
                await Task.WhenAll(uploadTask);
                var retval = await uploadTask;
                return retval;
            }
            catch (Exception e)
            {
                //Küldünk statuscode-ot a dropzone-nak
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(e.Message);
            }
        }

        private async Task<System.Web.Mvc.JsonResult> DeleteAttachementTask(Guid id)
        {
            return await Task.Run(async () =>
            {
                var sf = await db.File.FindAsync(id);
                if (null != sf)
                {
                    db.Entry(sf).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                }
                return Json(new { success = true });
            });
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public async Task<System.Web.Mvc.JsonResult> DeleteAttachement(Guid id)
        {
            try
            {
                var deleteTask = DeleteAttachementTask(id);
                await Task.WhenAll(deleteTask);
                var retval = await deleteTask;
                db.SaveChanges();
                return retval;
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { success = false });
            }
        }

        private async Task<FileData> DownloadTask(Guid id)
        {
            return await Task.Run(async () =>
            {
                var sf = await db.File.FindAsync(id);
                FileData _File;
                if (sf != null && sf.file_stream != null)
                {
                    _File.Bytes = sf.file_stream;
                    _File.Mime = sf.file_type;
                    //visszafelé kiveszem a névből a krix-kraxot
                    string sep = "_";
                    int first = sf.name.IndexOf(sep) + sep.Length;
                    int last = sf.name.Length;
                    string pure = sf.name.Substring(first);
                    _File.Name = pure;
                }
                else
                {
                    _File.Bytes = null;
                    _File.Mime = null;
                    _File.Name = null;
                }

                return _File;
            });
        }

        private struct FileData
        {
            public byte[] Bytes;
            public string Mime;
            public string Name;
        }


        //public async Task<FileResult> DownloadFirst(Guid suserId, Guid? filetypeId, Guid? fileId)
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [Authorize(Roles = "SysAdmin, User")]
        public async Task<FileResult> DownloadImage(Guid fileId)
        {
            try
            {
                var downloadTask = DownloadTask(fileId);
                await Task.WhenAll(downloadTask);
                var retval = await downloadTask;

                if (retval.Bytes == null)
                {
                    return null;
                }
                var mime = retval.Mime ?? "jpg"; //biztos, ami biztos
                var name = retval.Name ?? "";

                return File(retval.Bytes, mime, name);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }
        }

        #endregion filestream

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




