﻿using JQGrid.Helpers;
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
            using (log4net.ThreadContext.Stacks["NDC"].Push("GET: New/Edit"))
            {
                log.Info("begin");

                var ng = db.NewsGlobal.Find(id);
                NewViewModel suvm = new NewViewModel(){
                    Id = ng.Id,
                    PublishingDate = ng.PublishingDate,
                    Headline_pictureId = null == ng.Headline_picture ? Guid.Empty : ng.Headline_picture.stream_id,
                    ThumbnailId = null == ng.Thumbnail ? Guid.Empty : ng.Thumbnail.stream_id,
                    NewsStatusName = ng.NewsStatus.NameGlobal
                };

                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
                var nl = db.NewsLocal.FirstOrDefault(x => x.NewsGlobalId == id && x.LanguageId == lguid);
                if (null == nl)// ha még nincs ilyen lokalizáció, akkor csinálni kell neki
                {
                    var newnl = db.NewsLocal.Create();
                    newnl.NewsGlobalId = id;
                    newnl.LanguageId = lguid;
                    db.Entry(newnl).State = EntityState.Added;
                    db.SaveChanges();
                    //úgyis null
                    //suvm.Title1 = newnl.Title1;
                    //suvm.Title2 = newnl.Title2;
                    //suvm.Subtitle = newnl.Subtitle;
                    //suvm.Body_text = newnl.Body_text;
                }
                else
                {
                    suvm.Title1 = nl.Title1;
                    suvm.Title2 = nl.Title2;
                    suvm.Subtitle = nl.Subtitle;
                    suvm.Body_text = nl.Body_text;
                }

                var ngList = ng.TagsGlobal.Select(v => v.Id).ToList();
                suvm.TagsOut = db.TagsGlobal
                                .Where(z => !ngList.Contains(z.Id))
                                .Select(x => x.Id)
                                .ToArray();
                suvm.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.TagsLocal
                                                    .Where(z => !ngList.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                                    .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.TagsIn = db.TagsGlobal
                                .Where(z => ngList.Contains(z.Id))
                                .Select(x => x.Id)
                                .ToArray();
                suvm.TagToList = db.TagsLocal
                                    .Where(z => ngList.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
                                    .Select(x => new MyListItem { Value = x.TagsGlobalId, Text = x.Name })
                                    .OrderBy(x => x.Text)
                                    .ToList();

                log.Info("end");
                return View(suvm);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(NewViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: New/Edit"))
            {
                log.Info("begin");
                var resg = db.NewsGlobal
                        .Include(x => x.Headline_picture)
                        .Include(x => x.Thumbnail)
                        .Include(x => x.NewsStatus)
                        .Where(x => x.Id == model.Id)
                        .FirstOrDefault();
                string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);

                if (ModelState.IsValid)
                {                    
                    //if (resg.PublishingDate != model.PublishingDate) { resg.PublishingDate = model.PublishingDate; }
                    if (null == resg.Headline_picture ?
                        Guid.Empty != model.Headline_pictureId :
                        resg.Headline_picture.stream_id != model.Headline_pictureId) { resg.Headline_picture.stream_id = model.Headline_pictureId; }
                    if (null == resg.Thumbnail ?
                        Guid.Empty != model.ThumbnailId :
                        resg.Thumbnail.stream_id != model.ThumbnailId) { resg.Thumbnail.stream_id = model.ThumbnailId; }
                    //if (null == resg.NewsStatus ?
                    //    null != model.NewsStatusName :
                    //    resg.NewsStatus.NameGlobal != model.NewsStatusName) { resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == model.NewsStatusName); }

                    var resl = db.NewsLocal.FirstOrDefault(x => x.NewsGlobalId == model.Id && x.LanguageId == lguid);
                    if (resl.Title1 != model.Title1) { resl.Title1 = model.Title1; }
                    if (resl.Title2 != model.Title2) { resl.Title2 = model.Title2; }
                    if (resl.Subtitle != model.Subtitle) { resl.Subtitle = model.Subtitle; }
                    if (resl.Body_text != model.Body_text) { resl.Body_text = model.Body_text; }

                    if (null == model.TagsIn)
                    {
                        model.TagsIn = new Guid[] { };
                    }

                    foreach (var n2t in db.TagsGlobalNewsGlobals
                                .Where(x =>
                                    x.NewsGlobal_Id == model.Id
                                    && !model.TagsIn.Contains(x.TagsGlobal_Id)
                                )
                                .ToList()
                            )
                    {
                        db.Entry(n2t).State = EntityState.Deleted;
                    }

                    foreach (Guid id in model.TagsIn)
                    {
                        if (
                            !db.TagsGlobalNewsGlobals
                                .Where(x => x.NewsGlobal_Id == model.Id)
                                .Select(x => x.TagsGlobal_Id)
                                .Contains(id)
                           )
                        {
                            TagsGlobalNewsGlobal n2t = db.TagsGlobalNewsGlobals.Create();
                            n2t.NewsGlobal_Id = model.Id;
                            n2t.TagsGlobal_Id = id;
                            db.Entry(n2t).State = EntityState.Added;
                        }
                    }

                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.New.Edit(model.Id));
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);

                    //lista vagyok a viewban karbantartani
                    model.TagsOut = db.TagsGlobal
                                    .Where(z => !model.TagsIn.Contains(z.Id))
                                    .Select(x => x.Id)
                                    .ToArray();
                    model.TagFromList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.TagsLocal
                                                        .Where(z => model.TagsOut.Contains(z.TagsGlobalId) && z.LanguageId == lguid)
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




