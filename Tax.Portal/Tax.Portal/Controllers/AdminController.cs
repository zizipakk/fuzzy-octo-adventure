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

#region Trunk

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult Trunk()
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
        public virtual System.Web.Mvc.JsonResult ListInterpreterCenters(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.InterpreterCenter
                       .Select(a => new
                       {
                           Id = a.Id,
                           Code = null == a.Postcode ? "" : (null == a.Postcode.Country ? "" : a.Postcode.Country + ", ") 
                                                            + (null == a.Postcode.Code ? "" : a.Postcode.Code + " - ") 
                                                            + (null == a.Postcode.City ? "" : a.Postcode.City),
                           InterpreterCenterAddress = a.InterpreterCenterAddress,
                           ParcelNumber = a.ParcelNumber,
                           Count = a.KontaktUserRole.Count()
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
                                ,r.Code
                                ,r.InterpreterCenterAddress
                                ,r.ParcelNumber
                                ,r.Count.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateInterpreterCenters(string id, string oper, InterpreterCenter r, string Code)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
            if (null != Code) //törlésnél
            {
                Guid postcodeId = Guid.Parse(Code);
                r.Postcode = db.Postcode.SingleOrDefault(x => x.Id == postcodeId);
            }

            InterpreterCenter res;

            switch (oper)
            {
                case "edit":
                    res = db.InterpreterCenter.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.Postcode != r.Postcode) { res.Postcode = r.Postcode; }
                    if (res.InterpreterCenterAddress != r.InterpreterCenterAddress) { res.InterpreterCenterAddress = r.InterpreterCenterAddress; }
                    if (res.ParcelNumber != r.ParcelNumber) { res.ParcelNumber = r.ParcelNumber; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.InterpreterCenter.Find(r.Id);
                    if (null == res.KontaktUserRole || res.KontaktUserRole.Count() == 0) //noch eine Frage, aber schon handlet
                    {
                        db.Entry(res).State = EntityState.Deleted;
                    }
                    break;
                case "add":
                    res = db.InterpreterCenter.Create();
                    res.Id = r.Id;
                    res.Postcode = r.Postcode;
                    res.InterpreterCenterAddress = r.InterpreterCenterAddress;
                    res.ParcelNumber = r.ParcelNumber;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        public virtual string ListPostCode()
        {
            string result = string.Empty;

            result = "<select>";
            result += "<option value='NULL'></option>";
            foreach (var r in db.Postcode
                .Select(x => new { value = x.Id, text = x.Code + " - " + x.City + (null == x.Country || "" == x.Country ? "" : x.Country) })
                                .OrderBy(x => x.text)
                                .ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult Listpostcodes(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.Postcode
                       .Select(x => new
                       {
                           Id = x.Id,
                           Code = x.Code,
                           City = x.City,
                           Country = x.Country,
                           Count = x.InterpreterCenter.Count() + x.SinoszUser.Count()
                       })
                       .OrderBy(x => x.Code);

            //foreach (var item in rs0) { Debug.WriteLine(item); }

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                string.Empty
                                ,r.Id.ToString()
                                ,r.Code
                                ,r.City
                                ,r.Country
                                ,r.Count.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdatePostCodes(string id, string oper, Postcode r)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            Postcode res;

            switch (oper)
            {
                case "edit":
                    res = db.Postcode.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.Code != r.Code) { res.Code = r.Code; }
                    if (res.City != r.City) { res.City = r.City; }
                    if (res.Country != r.Country) { res.Country = r.Country; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.Postcode.Find(r.Id);
                    if (
                        (null == res.SinoszUser || res.SinoszUser.Count() == 0) && (null == res.InterpreterCenter || res.InterpreterCenter.Count() == 0)
                        ) //noch eine Frage, aber schon handlet
                    {
                        db.Entry(res).State = EntityState.Deleted;
                    }
                    break;
                case "add":
                    res = db.Postcode.Create();
                    res.Id = r.Id;
                    res.Code = r.Code;
                    res.City = r.City;
                    res.Country = r.Country;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }


#endregion Trunk

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
        public virtual System.Web.Mvc.JsonResult ListUsers(GridSettings grid, Guid? KontaktUserId)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = (from u in db.Users.Where(x => null == KontaktUserId || x.KontaktUser.Id == KontaktUserId)
                       from px in db.PBXExtensionData.Where(x => x.ApplicationUser.Id == u.Id && x.isDroped == false ).DefaultIfEmpty()//van élő pbx adata
                       from kur in db.KontaktUserRole.Where(x => null != x.PBXExtensionData && x.PBXExtensionData.Id == px.Id).DefaultIfEmpty()//számról a tolmácsbox
                       select new
                            {
                                Id = u.Id,
                                isFreshForPBX = null != u.KontaktUser && u.KontaktUser.isCommunicationRequested && null == px ? true : false,
                                UserName = u.UserName,
                                FirstName = u.KontaktUser.FirstName ?? "",
                                LastName = u.KontaktUser.LastName ?? "",
                                Email = u.Email,
                                isEmailValidated = u.isEmailValidated,
                                isLocked = u.isLocked,
                                isSinoszMember = null == u.KontaktUser.isSinoszMember ? false : u.KontaktUser.isSinoszMember,
                                SinoszId = null == u.KontaktUser.SinoszId ? "" : u.KontaktUser.SinoszId,
                                isCommunicationRequested = null == u.KontaktUser.isCommunicationRequested ? false : u.KontaktUser.isCommunicationRequested,
                                isDeviceReqested = null == u.KontaktUser.isDeviceReqested ? false : u.KontaktUser.isDeviceReqested,
                                isElected = null == u.KontaktUser.isElected ? false : u.KontaktUser.isElected,
                                AreaName = null == kur ? "" :
                                                null == kur.InterpreterCenter ? "" :
                                                    null == kur.InterpreterCenter.Postcode ? "" :
                                                        null == kur.InterpreterCenter.Postcode.Area ? "" :
                                                            kur.InterpreterCenter.Postcode.Area.AreaName,
                                InnerPhoneNumber = null == px ? "" : null == px.PhoneNumber ? "" : px.PhoneNumber.InnerPhoneNumber
                            }
                        ).AsEnumerable();

            //foreach (var i in rs0.ToList()) { Debug.WriteLine(i); }

            var rs00 = db.Users.Where(x => null == KontaktUserId || x.KontaktUser.Id == KontaktUserId)
                        .Join(db.ApplicationUserRole, x => x.Id, y => y.UserId, (x, y) => new { x, y.Role.Name })
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
                                isFreshForPBX = x.isFreshForPBX,
                                UserName = x.UserName,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.Email,
                                isEmailValidated = x.isEmailValidated,
                                isLocked = x.isLocked,
                                isSinoszMember = x.isSinoszMember,
                                SinoszId = x.SinoszId,
                                isCommunicationRequested = x.isCommunicationRequested,
                                isDeviceReqested = x.isDeviceReqested,
                                isElected = x.isElected,
                                AreaName = x.AreaName,
                                InnerPhoneNumber = x.InnerPhoneNumber,
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
                                ,r.isFreshForPBX.ToString()
                                ,r.UserName
                                ,r.FirstName.ToString()
                                ,r.LastName.ToString()
                                ,r.Email.ToString()
                                ,r.isEmailValidated.ToString()
                                ,r.isLocked.ToString()
                                ,r.isSinoszMember.ToString()
                                ,r.SinoszId.ToString()
                                ,r.isCommunicationRequested.ToString()
                                ,r.isDeviceReqested.ToString()
                                ,r.isElected.ToString()
                                ,r.AreaName
                                ,r.InnerPhoneNumber
                                ,r.RoleNames
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateUsers(string id, string oper, ApplicationUser u, KontaktUser k)
        {
            string response = "";
            switch (oper)
            {
                case "edit":
                    ApplicationUser res = db.Users.Find(id);
                    KontaktUser ku = db.KontaktUser.Find(res.KontaktUser.Id);                    
                        
                    if (null != res)
                    {
                        if (res.isEmailValidated != u.isEmailValidated) { 
                            res.isEmailValidated = u.isEmailValidated;
                            //kézinél is állítom a bekötést, ha ráutaló adatom van
                            if (res.isEmailValidated && null == res.SinoszUser && null != res.KontaktUser.SinoszId)
                            {
                                SinoszUser sinoszuser = db.SinoszUser.FirstOrDefault(x => x.SinoszId == res.KontaktUser.SinoszId && x.BirthDate == res.KontaktUser.BirthDate);
                                if (null == sinoszuser)
                                {
                                    response = "A regisztrációkor megadott tagsági szám a beírt születési dátummal együtt nem szerepel a tagnyilvántartásban! Az ügyfél nem került összekötésre a tagnyilvántartóval!";
                                }
                                else
                                {
                                    var usr = db.Users.Any(x => x.SinoszUser.Id == sinoszuser.Id && x.isLocked == false);
                                    if (usr)
                                        response = "Ezzel a SINOSZ tagsági számmal már korábban létrejött kapcsolat a tagnyilvántartóval! Az ügyfél nem került összekötésre a tagnyilvántartóval!";
                                }
                                res.SinoszUser = sinoszuser;
                            }
                            //kézinél törlöm a bekötést
                            if (!res.isEmailValidated && null != res.SinoszUser)
                            {
                                res.SinoszUser = null;
                            }
                        }
                        if (res.isLocked != u.isLocked) { res.isLocked = u.isLocked; }
                    }
                    if (null != ku)
                    {
                        if (ku.FirstName != k.FirstName) { ku.FirstName = k.FirstName; }
                        if (ku.LastName != k.LastName) { ku.LastName = k.LastName; }
                        if (ku.isCommunicationRequested != k.isCommunicationRequested) { ku.isCommunicationRequested = k.isCommunicationRequested; }
                        if (ku.isDeviceReqested != k.isDeviceReqested) { ku.isDeviceReqested = k.isDeviceReqested; }
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

        //public virtual ActionResult UpdateUsersIsEmailValidated(string UserId, bool isEmailValidated)
        //{
        //    if (null != UserId)
        //    {
        //        var a = db.Users.Single(x => x.Id == UserId);
        //        if (a.isEmailValidated != isEmailValidated)
        //        {
        //            a.isEmailValidated = isEmailValidated;
        //            //kézinél is állítom a bekötést
        //            if (isEmailValidated && null == a.SinoszUser)
        //            {
        //                SinoszUser sinoszuser = db.SinoszUser.FirstOrDefault(x => x.SinoszId == a.KontaktUser.SinoszId && x.BirthDate == a.KontaktUser.BirthDate);
        //                a.SinoszUser = sinoszuser;
        //            }
        //            //kézinél törlöm a bekötést
        //            if (!isEmailValidated && null != a.SinoszUser)
        //            {
        //                a.SinoszUser = null;
        //            }

        //            db.SaveChanges();
        //        }
        //    }
        //    return null;
        //}

        //public virtual ActionResult UpdateUsersIsLokced(string UserId, bool isLocked)
        //{
        //    if (null != UserId)
        //    {
        //        var a = db.Users.Single(x => x.Id == UserId);
        //        if (a.isLocked != isLocked)
        //        {
        //            a.isLocked = isLocked;
        //            db.SaveChanges();
        //        }
        //    }
        //    return null;
        //}


        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListUserRoles(GridSettings grid, string uid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = from r in db.Roles
                      from ur in db.ApplicationUserRole.Where(x => x.Role == r && x.UserId == uid).DefaultIfEmpty()
                      from i in db.InterpreterCenter.Where(x => x == ur.KontaktUserRole.InterpreterCenter).DefaultIfEmpty()
                      from po in db.Postcode.Where(x => x == i.Postcode).DefaultIfEmpty()
                      from a in db.Area.Where(x => x == i.Area).DefaultIfEmpty()
                      from px in db.PBXExtensionData.Where(x => x == ur.KontaktUserRole.PBXExtensionData && ur.KontaktUserRole.PBXExtensionData.isDroped == false).DefaultIfEmpty()
                      from pn in db.PhoneNumber.Where(x => x == px.PhoneNumber).DefaultIfEmpty()
                      from o in db.Organization.Where(x => x == ur.KontaktUserRole.Organization).DefaultIfEmpty()
                      where null != uid && "" != uid
                      select new
                      {
                          RoleId = r.Id,
                          isInclude = null == ur ? false : true,
                          RoleName = r.Name,
                          InterpreterCenter = null == po ? null : po.Code + "-" + po.City + ", " + i.InterpreterCenterAddress,
                          OrganizationName = null == o ? null : o.OrganizationName,
                          //ExtensionID = null == px ? null : px.ExtensionID,
                          InnerPhoneNumber = null == pn ? null : pn.InnerPhoneNumber,
                          ExternalPhoneNumber = null == pn ? null : pn.ExternalPhoneNumber,
                          getPBXNumber = null == pn ? false : true
                      };

            //foreach (var item in rs0) { Debug.WriteLine(item); }

            bool getPBXNumber = rs0.Any(x => x.getPBXNumber);

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
                                    ,r.InterpreterCenter
                                    ,r.OrganizationName
                                    //,r.ExtensionID
                                    ,r.InnerPhoneNumber
                                    ,r.ExternalPhoneNumber
                                    ,getPBXNumber.ToString()
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
                    var res = db.ApplicationUserRole.Create();
                    res.UserId = UserId;
                    res.RoleId = RoleId;
                    db.Entry(res).State = EntityState.Added;
                    var res1 = db.KontaktUserRole.Create();
                    res.KontaktUserRole = res1;
                    db.Entry(res1).State = EntityState.Added;
                    //csak telkós esetben
                    if (res.Role.Name == "PBXUser" || res.Role.Name == "Jeltolmácsok" || res.Role.Name == "Szakmai vezető" || res.Role.Name == "Diszpécser" || res.Role.Name == "Üzemeltető")
                    {
                        var res2 = db.PBXExtensionData.Create();
                        res1.PBXExtensionData = res2;
                        res2.ApplicationUser = db.Users.Single(x => x.Id == UserId);
                        var res3 = (from pn in db.PhoneNumber
                                    from px in db.PBXExtensionData.Where(x => x.PhoneNumber.Id == pn.Id && x.isDroped == false).DefaultIfEmpty()
                                    where (null == px.Id //nincs ilyen aktív szám
                                          && (res.Role.Name != "Jeltolmácsok" && res.Role.Name != "Diszpécser") //mindenki kap külső számot, csak jeltolmácsok és a diszpécserek nem
                                          && (null != pn.ExternalPhoneNumber) //kell külső szám + átállunk NULL-ra
//TODO, nincs típusozás, hanem hardbeat
                                          && pn.InnerPhoneNumber.Length > 4)//a 8880000-as számmezőből
                                          ||
                                          (null == px.Id //nincs ilyen aktív szám
                                          && (res.Role.Name == "Jeltolmácsok" || res.Role.Name == "Diszpécser") //nekik nem kell külső szám
                                          && (null == pn.ExternalPhoneNumber) //nem kell külső szám + átállunk NULL-ra
                                          && pn.InnerPhoneNumber.Length <= 4)//a 7000-es számmezőből
                                    select pn)
                                    .OrderBy(x => x.InnerPhoneNumber).FirstOrDefault();
                        //ha nincs vagy elfogy a számmező, a kliensnek visszabeszélek
                        if (null == res3)
                        {
                            return Json(new { success = false, error = true, response = "Nincs kiadható telefonszám a rendszerben!" }); //return Json(e.Message) 
                        }
                        res2.PhoneNumber = res3; //először kellenek a számok
                        //res2.ExtensionID = res2.PhoneNumber.InnerPhoneNumber + "@" + res2.ApplicationUser.UserName; //aztán belőle lesz az ID
                        //res2.Password = res2.Password;
                        res2.isDroped = false;
                        res2.isSynced = false; //egyelőre azonos az isDroped-del
                        res2.StartTime = DateTime.Now;
                        res2.EndTime = DateTime.MaxValue;
                        db.Entry(res2).State = EntityState.Added;
                        //ha a területileg elfogyott a limit, danger
                        if (null != res0.SinoszUser) //regisztrált sinoszuser, lesz tagszervezet, és megyéje
                        {
                            if (null != res0.SinoszUser.Organization && null != res0.SinoszUser.Organization.Postcode && null != res0.SinoszUser.Organization.Postcode.Area)
                            {
                                int limit = res0.SinoszUser.Organization.Postcode.Area.PhoneNumberLimit;
                                var current = db.PBXExtensionData.Where(x => !x.isDroped && x.ApplicationUser.SinoszUser.Organization.Postcode.Area.Id == res0.SinoszUser.Organization.Postcode.Area.Id).Count() + 1; //még a jelenlegit nem mentettem
                                if (limit < current)
                                {
                                    limitwarning = string.Format("A kiadott telefonszámok mennyisége túllépte a területileg megszabott értéket! {0}: maximum {1} db / jelenleg {2} db",
                                        res0.SinoszUser.Organization.Postcode.Area.AreaName,
                                        limit,
                                        current);
                                }
                                else
                                {
                                    limitwarning = "";
                                }
                            }
                        }
                    }
                }
                else
                {
                    var red = db.ApplicationUserRole.Single(x => x.UserId == UserId && x.RoleId == RoleId);
                    if (null != red.KontaktUserRole)
                    {
                        var red1 = red.KontaktUserRole;
                        if (null != red1.PBXExtensionData)
                        {
                            var red3 = red1.PBXExtensionData;
                            red3.isDroped = true; //nincs törlés
                            if (red3.isSynced) { red3.isSynced = false; } //isDroped true  miatt nem is fog látszani ez az inaktív rekord, de azért beállítom
                            red3.EndTime = DateTime.Now;
                            //ved.extId = red3.PhoneNumber.InnerPhoneNumber + "@" + red.User.UserName;
                        }
                        db.Entry(red1).State = EntityState.Deleted;
                    }

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

        public virtual string ListInterpreterCenter(string UserId, string RoleId)
        {
            string rolename = null;
            var role = db.Roles.Find(RoleId);
            if (null != role)
                rolename = role.Name;

            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.InterpreterCenter
                               where 
                                    (null == RoleId //rtörzsadaton vagyok
                                    || rolename == "Jeltolmácsok") //vagy rule-ban ezt a szerepet osztom
                               from p in db.Postcode.Where(x => x.Id == r.Postcode.Id)
                               //a jelenlegit így lehetne kihagyni a listából
                               //from ur in db.ApplicationUserRole.Where(x => x.KontaktUserRole.InterpreterCenter.Id == r.Id && x.UserId == UserId && x.RoleId == RoleId).DefaultIfEmpty()                              
                               //where ur.RoleId == null
                               select new { value = r.Id, text = p.Code + "-" + p.City + ", " + r.InterpreterCenterAddress })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual ActionResult UpdateUserRoles(string id, string oper, string UserId, string RoleId, string isIncludes, string InterpreterCenter, string OrganizationName)
        {
            Guid InterGuid = null == InterpreterCenter ? Guid.Empty : Guid.Parse(InterpreterCenter);
            Guid OrgGuid = null == OrganizationName ? Guid.Empty : Guid.Parse(OrganizationName);

            switch (oper)
            {
                case "edit":
                    ApplicationUserRole res = db.ApplicationUserRole.Single(x => x.UserId == UserId && x.RoleId == id);
                    if (null != res.KontaktUserRole)                       
                    {
                        if (null != InterpreterCenter)
                        {
                            var res1 = db.InterpreterCenter.Find(InterGuid);
                            res.KontaktUserRole.InterpreterCenter = res1;
                        }

                        if (null != OrganizationName)
                        {
                            var res2 = db.Organization.Find(OrgGuid);
                            res.KontaktUserRole.Organization = res2;
                        }
                    }
                    //db.Entry(res).State = EntityState.Modified;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
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




