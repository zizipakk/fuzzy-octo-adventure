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
        public virtual ActionResult Edit(Guid? id)
        {
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit()
        {
            return null;
        }

        #endregion edit

        #region statusbuttons

        /// <summary>
        /// státsuzgombok
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




