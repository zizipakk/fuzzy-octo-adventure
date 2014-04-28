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


namespace Tax.Portal.Controllers
{
    public partial class AdminController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

#region Log

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult Log()
        {
            log.Info("begin");
            log.Info("end");
            return View();
        }

#endregion Log


#region Rule
        
        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult Rule(Guid? KontaktUserId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("AccountController()"))
            {
                log.Info("begin");
                ViewBag.KontaktUserId = KontaktUserId;
                log.Info("end");
                return View();
            }
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListUsers(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = (from u in db.Users
                       select new
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                Email = u.Email,
                                isLocked = u.isLocked,
                            }
                        ).AsEnumerable();

            //foreach (var i in rs0.ToList()) { Debug.WriteLine(i); }

            var rs00 = db.Users
                        .Join(db.IdentityUserRole, x => x.Id, y => y.UserId, (x, y) => new { x, y.Role.Name })
                        .ToList()
                        .GroupBy(x => x.x.Id)
                        .Select(x => new { 
                            Id = x.FirstOrDefault().x.Id,
                            RoleNames = (x.Select(y => y.Name)).Aggregate((a, b) => (a == "" ? "" : a + ", ") + b)
                        })
                        .AsEnumerable();

            //foreach (var i in rs00.ToList()) { Debug.WriteLine(i); }

            var rs000 = rs0.SelectMany(x => rs00.Where(y => y.Id == x.Id), 
                            (x, y) => new {
                                Id = x.Id,
                                UserName = x.UserName,
                                Email = x.Email,
                                isLocked = x.isLocked,
                                RoleNames = y.RoleNames
                            });
                                
            //foreach (var i in rs000) { Debug.WriteLine(i); }

            var rs = rs000.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                string.Empty
                                ,r.Id.ToString()
                                ,r.UserName
                                ,r.Email.ToString()
                                ,r.isLocked.ToString()
                                ,r.RoleNames
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateUsers(string id, string oper, ApplicationUser u)
        {
            string response = "";
            switch (oper)
            {
                case "edit":
                    ApplicationUser res = db.Users.Find(id);
                        
                    if (null != res)
                    {
                        if (res.isLocked != u.isLocked) { res.isLocked = u.isLocked; }
                        if (res.Name != u.Name) { res.Name = u.Name; }
                        if (res.Email != u.Email) { res.Email = u.Email; }
                    }
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            if (response == "")
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = false, response = response });
            }
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListUserRoles(GridSettings grid, string uid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = from r in db.Roles
                      from ur in db.IdentityUserRole.Where(x => x.Role == r && x.UserId == uid).DefaultIfEmpty()
                      where null != uid && "" != uid
                      select new
                      {
                          RoleId = r.Id,
                          isInclude = null == ur ? false : true,
                          RoleName = r.Name,
                      };

            //foreach (var item in rs0) { Debug.WriteLine(item); }

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.RoleId.ToString(),
                               cell = new string[] 
                               {
                                    r.RoleId.ToString()
                                    ,r.isInclude.ToString()
                                    ,r.RoleName                                
                                    ,string.Empty
                                    //,r.ExtensionID
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateRolesIsInclude(string UserId, string RoleId, bool isInclude)
        {
            string limitwarning = "";
            if ("null" != UserId)//RoleId mindig van és az isInclude click-en vagyok
            {                
                var res0 = db.Users.Find(UserId);
                if (isInclude)
                {
                    var res = db.IdentityUserRole.Create();
                    res.UserId = UserId;
                    res.RoleId = RoleId;
                    db.Entry(res).State = EntityState.Added;
                }
                else
                {
                    var red = db.IdentityUserRole.Single(x => x.UserId == UserId && x.RoleId == RoleId);
                    db.Entry(red).State = EntityState.Deleted;
                }

                db.SaveChanges();
            }

            if ("" != limitwarning)
            {
                return Json(new { success = false, error = false, response = limitwarning });
            }
            else
            {
                return Json(new { success = true });
            }
        }


#endregion Rule

#region Setup

        [Authorize(Roles = "SysAdmin")]
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

#endregion Setup

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




