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
using Microsoft.AspNet.Identity.EntityFramework;


namespace Tax.Portal.Controllers
{
    public partial class AdminController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

#region User
        
        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult Index()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Index/Admin"))
            {
                log.Info("begin");
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

            var rs = (db.Users
                       .Select(u => new
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                Password = "******",
                                Name = u.Name,
                                Email = u.Email,
                                isLocked = u.isLocked,
                            }
                        ).AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                string.Empty
                                ,r.Id.ToString()
                                ,r.UserName
                                ,r.Password
                                ,r.Name
                                ,r.Email.ToString()
                                ,r.isLocked.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult CreateUser(ApplicationUser u, string Password)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Admin/CreateUser"))
            {
                log.Info("begin");
                //kézi validálás
                if ("" == u.UserName || "" == u.Name || "" == u.Email)
                {
                    return Json(new { success = false, error = false, response = "[UsrName] and [Name] and [Email] is required" });
                }
                int minlenght = 3;
                if (u.UserName.Length < minlenght || u.Name.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The UserName and Name must be at least {0} characters long.", minlenght) });
                }
                //barátunk a usermanager
                var user = new ApplicationUser()
                {
                    UserName = u.UserName,
                    Name = u.Name,
                    Email = u.Email,
                    isLocked = false
                };

                //user.Roles readonly, ezért indirekt írása az ApplicationUserRole táblának
                IdentityRole role = db.Roles.SingleOrDefault(x => x.Name == "User"); //Egy ilyen kell, különben balhé
                if (null != role)
                {
                    user.Roles.Add(new IdentityUserRole() { Role = role });
                }

                AccountController ctr = new AccountController();
                var result = ctr.UserManager.CreateAsync(user, Password);//ez menti a modeleket
                log.Info("User létrehozva");
                if (!result.Result.Succeeded)
                {
                    log.Info("end with error");
                    return Json(new { success = false, error = true, response = result.Result.Errors });
                }

                log.Info("end with ok");
                return Json(new { success = true, error = false, response = "" });
            }
        }

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult EditUser(string id, string oper, ApplicationUser u)
        {
            //kézi validálás
            if ("" == u.UserName || "" == u.Name || "" == u.Email)
            {
                return Json(new { success = false, error = false, response = "[UsrName] and [Name] and [Email] is required" });
            }
            int minlenght = 3;
            if (u.UserName.Length < minlenght || u.Name.Length < minlenght)
            {
                return Json(new { success = false, error = true, response = string.Format("The UserName and Name must be at least {0} characters long.", minlenght) });
            }

            string response = "";
            switch (oper)
            {
                case "edit":
                    ApplicationUser res = db.Users.Find(id);
                        
                    if (null != res)
                    {
                        if (res.UserName != u.UserName) { res.UserName = u.UserName; }
                        if (res.Name != u.Name) { res.Name = u.Name; }
                        if (res.Email != u.Email) { res.Email = u.Email; }
                        if (res.isLocked != u.isLocked) { res.isLocked = u.isLocked; }
//Todo: itt felvesszük meg elveszszük az SysAdmin szerepet
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

#endregion User

#region Setup

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin")]
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

        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult CreateSetup(SystemParameter r)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Admin/CreateUser"))
            {
                log.Info("begin");
                //kézi validálás
                if ("" == r.Name || "" == r.Value)
                {
                    return Json(new { success = false, error = false, response = "[Name] and [Value] are required" });
                }
                int minlenght = 3;
                if (r.Name.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The Name must be at least {0} characters long.", minlenght) });
                }
                //barátunk a usermanager
                var sys = db.SystemParameter.Create();
                sys.Name = r.Name;
                sys.Value = r.Value;
                sys.Public = true;
                db.Entry(sys).State = EntityState.Added;
                db.SaveChanges();

                log.Info("end with ok");
                return Json(new { success = true, error = false, response = "" });
            }
        }


        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult EditSetup(string id, string oper, SystemParameter r)
        {
            //kézi validálás
            if ("" == u.UserName || "" == u.Name || "" == u.Email)
            {
                return Json(new { success = false, error = false, response = "[UsrName] and [Name] and [Email] is required" });
            }
            int minlenght = 3;
            if (u.UserName.Length < minlenght || u.Name.Length < minlenght)
            {
                return Json(new { success = false, error = true, response = string.Format("The UserName and Name must be at least {0} characters long.", minlenght) });
            }

            string response = "";            
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            SystemParameter res;

            switch (oper)
            {
                case "edit":
                    res = db.SystemParameter.Find(r.Id);
                    if (res.Name != r.Name) { res.Name = r.Name; }
                    if (res.Value != r.Value) { res.Value = r.Value; }
                    if (res.Description != r.Description) { res.Description = r.Description; }
                    if (res.Public != r.Public) { res.Public = r.Public; }
                    db.Entry(res).State = EntityState.Modified;
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


#endregion Setup

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




