using JQGrid.Helpers;
using Tax.Data.Models;
using Tax.Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using Tax.Portal.Helpers;
using Rotativa;
using Tax.Portal.Mailers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Tax.Portal.Controllers
{
    public partial class StockController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

#region Devices

        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual ActionResult Devices(Guid? KontaktUserId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Stock/Devices()"))
            {
                log.Info("begin");
                ViewBag.KontaktUserId = KontaktUserId;
                log.Info("end");
                return View("");
            }
        }

        /// <summary>
        /// Eszközkeresés adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual System.Web.Mvc.JsonResult ListDevices(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.Devices
                        .Select(x => new
                        {
                            Id = x.Id,
                            DeviceId = x.DeviceId,
                            AccountingId = x.AccountingId,
                            DeviceTypeName = x.DeviceTypes.DeviceTypeName,
                            DeviceName = x.DeviceName,
                            DeviceStatusName = x.DeviceStatus.DeviceStatusName,
                            cntMax = x.DeviceLog.Count()
                        }).OrderBy(x => x.DeviceId)
                        .AsEnumerable();

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                            select new JsonRow
                            {
                                id = r.Id.ToString(),
                                cell = new string[] 
                            {                             
                            string.Empty
                            ,r.Id.ToString()
                            ,r.DeviceId
                            ,r.AccountingId
                            ,r.DeviceTypeName
                            ,r.DeviceName
                            ,r.DeviceStatusName
                            ,r.cntMax.ToString()
                            }
                            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lenyílók a gridhez
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public virtual string ListDeviceTypes(string devId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.DeviceTypes
                               select new { value = r.Id, text = r.DeviceTypeName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListDeviceStatus(string devId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.DeviceStatus
                               select new { value = r.Id, text = r.DeviceStatusName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual ActionResult UpdateDevices(string id, string oper, Devices d, string DeviceTypeName, string DeviceStatusName)
        {
            Guid tGuid = null == DeviceTypeName ? Guid.Empty : Guid.Parse(DeviceTypeName);
            Guid sGuid = null == DeviceStatusName ? Guid.Empty : Guid.Parse(DeviceStatusName);

            Devices res;

            switch (oper)
            {
                case "add":
                    res = db.Devices.Create();
                    res.Id = Guid.NewGuid();
                    res.DeviceId = d.DeviceId;
                    res.AccountingId = d.AccountingId;
                    res.DeviceTypes = db.DeviceTypes.Find(tGuid);
                    res.DeviceName = d.DeviceName;
                    res.DeviceStatus = db.DeviceStatus.Find(sGuid);
                    db.Entry(res).State = EntityState.Added;
                    break;
                case "edit":
                    res = db.Devices.Find(d.Id);
                    if (res.Id != d.Id) { res.Id = d.Id; }
                    if (res.DeviceId != d.DeviceId) { res.DeviceId = d.DeviceId; }
                    if (res.AccountingId != d.AccountingId) { res.AccountingId = d.AccountingId; }
                    if (res.DeviceTypes.Id != tGuid) { res.DeviceTypes = db.DeviceTypes.Find(tGuid); }
                    if (res.DeviceName != d.DeviceName) { res.DeviceName = d.DeviceName; }
                    if (res.DeviceStatus.Id != sGuid) { res.DeviceStatus = db.DeviceStatus.Find(sGuid); }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.Devices.Find(d.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual System.Web.Mvc.JsonResult ListDeviceLogFromDevice(GridSettings grid, Guid? devId)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.DeviceLog
                        .Where(x => x.Devices.Id == devId)
                        .Select(x => new
                        {
                            Id = x.Id,
                            DeviceLogDate = x.DeviceLogDate,
                            AreaName = x.Area.AreaName,
                            Name = (x.KontaktUser.FirstName ?? "") + " " + (x.KontaktUser.LastName ?? ""),
                            Remark = x.Remark
                        }).OrderBy(x => x.DeviceLogDate)
                        .AsEnumerable();

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                            {                             
                            string.Empty
                            ,r.Id.ToString()
                            ,r.DeviceLogDate.ToString()
                            ,r.AreaName
                            ,r.Name
                            ,r.Remark
                            }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual string ListAreas(string devId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.Area
                               select new { value = r.Id, text = r.AreaName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual ActionResult UpdateDeviceLogFromDevice(string id, string oper, DeviceLog d, string AreaName, Guid devId)
        {
            Guid aGuid = null == AreaName ? Guid.Empty : Guid.Parse(AreaName);
            DeviceLog res;

            switch (oper)
            {
                case "add":
                    res = db.DeviceLog.Create();
                    res.Id = Guid.NewGuid();
                    res.DeviceLogDate = d.DeviceLogDate;
                    res.Area = db.Area.Find(aGuid);
                    res.Devices = db.Devices.Find(devId);                    
                    res.Remark = d.Remark;
                    db.Entry(res).State = EntityState.Added;
                    break;
                case "edit":
                    res = db.DeviceLog.Find(d.Id);
                    if (res.Id != d.Id) { res.Id = d.Id; }
                    if (res.DeviceLogDate != d.DeviceLogDate) { res.DeviceLogDate = d.DeviceLogDate; }
                    if (res.Area.Id != aGuid) { res.Area = db.Area.Find(aGuid); }
                    if (res.Area.AreaName != "Ügyfél") { res.KontaktUser = null; } 
                    if (res.Devices.Id != devId) { res.Devices = db.Devices.Find(devId); }
                    if (res.Remark != d.Remark) { res.Remark = d.Remark; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.DeviceLog.Find(d.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual ActionResult UpdateDeviceLogUser(Guid dlogId, string usrId)
        {
            var res = db.DeviceLog.Find(dlogId);
            var usr = db.Users.Find(usrId);
            res.KontaktUser = usr.KontaktUser;

            db.Entry(res).State = EntityState.Modified;
            db.SaveChanges();

            //mindenképpen mentek, csak figyi
            if (null != usr.SinoszUser) //regisztrált sinoszuser, lesz tagszervezet, és megyéje
            {
                if (null != usr.SinoszUser.Organization && null != usr.SinoszUser.Organization.Postcode && null != usr.SinoszUser.Organization.Postcode.Area)
                {
                    int limit = usr.SinoszUser.Organization.Postcode.Area.DeviceNumberLimit;                    
                    var current = db.DeviceLog
                                        .Join(db.Users.Where(y => y.SinoszUser.Organization.Postcode.Area.Id == usr.SinoszUser.Organization.Postcode.Area.Id),
                                                x => x.KontaktUser.Id,
                                                y => y.KontaktUser.Id,
                                                (x, y) => new
                                                {
                                                    logid = x.Id,
                                                    deviceid = x.Devices.Id,
                                                    logdate = x.DeviceLogDate
                                                })
                                        .Join(db.DeviceLog.Where(v => v.KontaktUser != null).GroupBy(v => v.Devices.Id).Select(v => new
                                                                                                                        {
                                                                                                                            deviceid = v.FirstOrDefault().Devices.Id,
                                                                                                                            maxdate = v.Max(w => w.DeviceLogDate)
                                                                                                                        }),
                                                                                                                        a => a.deviceid,
                                                                                                                        b => b.deviceid,
                                                                                                                        (a, b) => a)
                        .Count();
                    if (limit < current)
                    {
                        string limitwarning = string.Format("A kiadott készülékek mennyisége túllépte a területileg megszabott értéket! {0}: maximum {1} db / jelenleg {2} db",
                                        usr.SinoszUser.Organization.Postcode.Area.AreaName,
                                        limit,
                                        current);
                        return Json(new { success = false, error = false, response = limitwarning });
                    }
                }
            }            
            return Json(new { success = true });
        }

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult InsertDeviceLogKontaktUser(Guid KontaktUserId, Guid DeviceId)
        {
            var res = db.DeviceLog.Create();
            res.Area = db.Area.Single(x => x.AreaName == "Ügyfél");
            res.DeviceLogDate = DateTime.Now;
            res.Devices = db.Devices.Find(DeviceId);
            res.KontaktUser = db.KontaktUser.Find(KontaktUserId);
            res.Remark = "Eszközátvétel";

            db.Entry(res).State = EntityState.Added;
            db.SaveChanges();

            var usr = db.Users.FirstOrDefault(x => x.KontaktUser.Id == KontaktUserId);
            //mindenképpen mentek, csak figyi
            if (null != usr.SinoszUser) //regisztrált sinoszuser, lesz tagszervezet, és megyéje
            {
                if (null != usr.SinoszUser.Organization && null != usr.SinoszUser.Organization.Postcode && null != usr.SinoszUser.Organization.Postcode.Area)
                {
                    int limit = usr.SinoszUser.Organization.Postcode.Area.DeviceNumberLimit;                    
                    var current = db.DeviceLog
                                        .Join(db.Users.Where(y => y.SinoszUser.Organization.Postcode.Area.Id == usr.SinoszUser.Organization.Postcode.Area.Id),
                                                x => x.KontaktUser.Id,
                                                y => y.KontaktUser.Id,
                                                (x, y) => new
                                                {
                                                    logid = x.Id,
                                                    deviceid = x.Devices.Id,
                                                    logdate = x.DeviceLogDate
                                                })
                                        .Join(db.DeviceLog.Where(v => v.KontaktUser != null).GroupBy(v => v.Devices.Id).Select(v => new
                                                                                                                        {
                                                                                                                            deviceid = v.FirstOrDefault().Devices.Id,
                                                                                                                            maxdate = v.Max(w => w.DeviceLogDate)
                                                                                                                        }),
                                                                                                                        a => a.deviceid,
                                                                                                                        b => b.deviceid,
                                                                                                                        (a, b) => a)
                        .Count();
                    if (limit < current)
                    {
                        string limitwarning = string.Format("A kiadott készülékek mennyisége túllépte a területileg megszabott értéket! {0}: maximum {1} db / jelenleg {2} db",
                                        usr.SinoszUser.Organization.Postcode.Area.AreaName,
                                        limit,
                                        current);
                        return Json(new { success = false, error = false, response = limitwarning });
                    }
                }
            }
            return Json(new { success = true });
        }

#endregion Devices

#region Users

        [Authorize(Roles = "SysAdmin, DeviceAdmin")]
        public virtual ActionResult Users()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Stock/Users()"))
            {
                log.Info("begin");
                log.Info("end");
                return View("");
            }
        }

        public virtual System.Web.Mvc.JsonResult ListDeviceLogFromUser(GridSettings grid, string usrId)
        {
            JQGrid.Helpers.JsonResult result;

            Guid? kntId = null;
            var knt = db.Users.Find(usrId);
            if (null != knt)
            {
                kntId = knt.KontaktUser.Id;
            }
            else
            {
                kntId = null;
            }

            var rs0 = db.DeviceLog
                        .Where(x => x.KontaktUser.Id == kntId && null != kntId)
                        .Select(x => new
                        {
                            Id = x.Id,
                            DeviceLogDate = x.DeviceLogDate,
                            //AreaName = x.Area.AreaName,
                            //Name = (x.KontaktUser.FirstName ?? "") + " " + (x.KontaktUser.LastName ?? ""),
                            //Remark = x.Remark,
                            DeviceId = x.Devices.DeviceId,
                            DeviceName = x.Devices.DeviceName,
                            DeviceStatusName = x.Devices.DeviceStatus.DeviceStatusName
                        }).OrderBy(x => x.DeviceLogDate)
                        .AsEnumerable();

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                            select new JsonRow
                            {
                                id = r.Id.ToString(),
                                cell = new string[] 
                                {                             
                                    r.Id.ToString()
                                    ,r.DeviceLogDate.ToString()
                                    //,r.AreaName
                                    //,r.Name
                                    //,r.Remark
                                    ,r.DeviceId
                                    ,r.DeviceName
                                    ,r.DeviceStatusName
                                }
                            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


#endregion Users

#region reservation

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult ReservationAdmin()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Stock/ReservationAdmin()"))
            {
                log.Info("begin");
                log.Info("end");
                return View("");
            }
        }

        /// <summary>
        /// Időpontok karbantartása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin")]
        public virtual System.Web.Mvc.JsonResult ListReservationTimes(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.ReservationTime.Select(x => x);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                            {                             
                            string.Empty
                            ,r.Id.ToString()
                            ,r.ReservationDate.ToString()
                            ,r.ReservationBegin.ToString().Substring(0,5)
                            ,r.ReservationMax.ToString()
                            ,r.ReservationCurrent.ToString()
                            ,r.isEnabled.ToString()
                            }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult UpdateReservationTimes(string id, string oper, ReservationTime r)
        {
            ReservationTime res;
            var oldres = db.ReservationTime.Where(x => x.ReservationDate == r.ReservationDate);
            TimeSpan timewindow = TimeSpan.Parse("00:00:30:00");

            switch (oper)
            {
                case "add":
                    res = db.ReservationTime.Create();                    
                    if (oldres.Count() > 0) //ha már van ezen a napon időpont
                    {
                        if (oldres.Max(x => x.ReservationBegin) >= TimeSpan.Parse("00:23:30:00")) //nincs több időpont ma
                        {
                            return Json(new { success = false, message = string.Format("{0} dátumon nincs több időpont!", r.ReservationDate.ToString()) });
                        }
                        else
                        {
                            res.ReservationBegin = oldres.Max(x => x.ReservationBegin) + timewindow;
                        }
                    }
                    else //ha még nincs, akkor legyen az első 8-kor
                    {
                        res.ReservationBegin = TimeSpan.Parse("00:08:00:00");
                    }                    
                    res.ReservationDate = r.ReservationDate;
                    res.ReservationMax = 4; //todo: ki lehetne tenni systemparametersbe
                    res.ReservationCurrent = 0;                    
                    res.isEnabled = true;
                    db.Entry(res).State = EntityState.Added;
                    break;
                case "edit":
                    res = db.ReservationTime.Find(r.Id);
                    TimeSpan beginzone = r.ReservationBegin.Subtract(timewindow);
                    TimeSpan endzone = r.ReservationBegin.Add(timewindow);
                    if (oldres.Any(x =>
                                    x.Id != r.Id
                                    && x.ReservationBegin > beginzone
                                    && x.ReservationBegin < endzone))
                        return Json(new { success = false, message = string.Format("{0} dátumon időpontütközés!", r.ReservationDate.ToString()) });
                    if (res.ReservationBegin != r.ReservationBegin) { res.ReservationBegin = r.ReservationBegin; }
                    if (res.ReservationDate != r.ReservationDate) { res.ReservationDate = r.ReservationDate; }
                    if (res.ReservationMax != r.ReservationMax) { res.ReservationMax = r.ReservationMax; }
                    if (res.isEnabled != r.isEnabled) { res.isEnabled = r.isEnabled; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.ReservationTime.Find(r.Id);
                    if (db.Reservation.Any(x => x.ReservationTime.Id == res.Id))
                        return Json(new { success = false, message = string.Format("Már foglalt időpont nem törölhető!") });
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return Json(new { success = true });
        }

        /// <summary>
        /// Időpontot foglalók
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin")]
        public virtual System.Web.Mvc.JsonResult ListReservatorUsers(GridSettings grid, Guid? Id)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.Reservation.Where(x => x.ReservationTime.Id == Id);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                            {                             
                            r.Id.ToString()
                            ,r.KontaktUser.FirstName
                            ,r.KontaktUser.LastName
                            ,r.KontaktUser.isElected.ToString()
                            ,r.KontaktUser.isCommunicationRequested.ToString()
                            ,r.KontaktUser.isDeviceReqested.ToString()
                            ,r.KontaktUser.Id.ToString()
                            ,r.KontaktUser.Id.ToString()
                            ,r.KontaktUser.Id.ToString()
                            ,r.isPresent.ToString()
                            }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "SysAdmin")]
        public virtual ActionResult UpdateisPresent(string reservid, bool value)
        {
            if (null != reservid)
            {
                Guid reservGuid = Guid.Parse(reservid);
                var a = db.Reservation.Find(reservGuid);
                if (a.isPresent != value)
                {
                    a.isPresent = value;
                    db.SaveChanges();
                }
            }
            return null;
        }

        [HttpGet]
        [Authorize]
        public virtual ActionResult Reservation()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Stock/Reservation()"))
            {
                log.Info("begin");
                //belép, ki vagyok, kiválasztottak-e, vagy már megjelentem az átvételen 
                string userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);

                if (!user.KontaktUser.isElected)
                {
                    log.Info("end: A kiválasztást törölték.");
                    return RedirectToAction(MVC.Home.IsNotElected());
                }

                if (user.KontaktUser.Reservation.Count() > 0 && !user.KontaktUser.Reservation.Any(x => x.isPresent == false ))
                {
                    log.Info("end: Nincs nyitott foglalása");
                    return RedirectToAction(MVC.Home.IsReservationClosed());
                }

                ReservationViewModel rvm = new ReservationViewModel();
                rvm.KontaktUserId = user.KontaktUser.Id;
                DateTime datenow = DateTime.Now.Date;
                TimeSpan timenowplus = DateTime.Now.AddHours(1).TimeOfDay;
                rvm.FreeTimes = db.ReservationTime
                                    .Where(x =>
                                        x.isEnabled //nincs felfüggesztve
                                        && x.ReservationMax > x.ReservationCurrent //még van üres hely
                                        && (
                                            x.ReservationDate > datenow //későbbi dátum
                                            || (x.ReservationDate == datenow && x.ReservationBegin >= timenowplus) //vagy a mai de min. egy óra műlva kezdődik
                                            )
                                    )
                                    .OrderBy(x => x.ReservationDate)
                                    .ThenBy(x => x.ReservationBegin)
                                    .ToList();
                var rs = db.Reservation
                                    .Where(x => x.KontaktUser.Id == user.KontaktUser.Id)
                                    .OrderBy(x => x.ReservationTime.ReservationDate)
                                    .ThenBy(x => x.ReservationTime.ReservationBegin)
                                    .ToList();
                rvm.isAdd = !rs.Any(x => !x.isPresent && x.ReservationTime.ReservationDate >= datenow);
                rvm.Reservations = new List<string[]>();
                rvm.Reservations.AddRange(rs.Select(r => new[] {                           
                            r.ReservationTime.ReservationDate.ToString("yyyy.MM.dd")
                            ,r.ReservationTime.ReservationBegin.ToString().Substring(0,5)
                            ,r.isPresent ? "Megjelent" : ""
                        }));

                log.Info("end OK");
                return View(rvm);            
            }
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Reservation(ReservationViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Stock/Reservation()"))
            {
                log.Info("begin");
                DateTime datenow = DateTime.Now.Date;
                if (model.isAdd)
                {
                    var ctr = db.ReservationTime.Find(model.SelectedReservationTimeId);

                    if (null != ctr)//ha nem választott, ne történjen semmi
                    {
                        if (ctr.ReservationMax <= ctr.ReservationCurrent) //időközben megelőzték, lockolni kellene, de most nincs idő
                            ModelState.AddModelError("SelectedReservationTimeId", "Sajnos az általad kijelölt időpont időközben betelt. Kérlek, válassz másikat!");

                        if (ModelState.IsValid)
                        {
                            Reservation r = db.Reservation.Create();
                            r.KontaktUser = db.KontaktUser.Find(model.KontaktUserId);
                            r.ReservationTime = db.ReservationTime.Find(model.SelectedReservationTimeId);
                            r.isPresent = false;
                            db.Entry(r).State = EntityState.Added;
                            r.ReservationTime.ReservationCurrent = r.ReservationTime.ReservationCurrent + 1;

                            db.SaveChanges();
                            log.Info("end with ok");
                            return RedirectToAction(MVC.Stock.Reservation());
                        }
                    }
                }
                else //akkor meg törlöm az utolsó nyitott foglalást
                {
                    Reservation r = db.Reservation.FirstOrDefault(x =>
                        !x.isPresent
                        && x.KontaktUser.Id == model.KontaktUserId
                        && x.ReservationTime.ReservationDate >= datenow);
                    r.ReservationTime.ReservationCurrent = r.ReservationTime.ReservationCurrent - 1;

                    db.Entry(r).State = EntityState.Deleted;
                    db.SaveChanges();
                    log.Info("end with ok");
                    return RedirectToAction(MVC.Stock.Reservation());

                }

                //lenyílókat megint inicializálni kell, megyek vissza a validációval                    
                TimeSpan timenowplus = DateTime.Now.AddHours(1).TimeOfDay;
                model.FreeTimes = db.ReservationTime
                                    .Where(x =>
                                        x.isEnabled //nincs felfüggesztve
                                        && x.ReservationMax > x.ReservationCurrent //még van üres hely
                                        && (
                                            x.ReservationDate > datenow //későbbi dátum
                                            || (x.ReservationDate == datenow && x.ReservationBegin >= timenowplus) //vagy a mai de min. egy óra műlva kezdődik
                                            )
                                    )
                                    .OrderBy(x => x.ReservationDate)
                                    .ThenBy(x => x.ReservationBegin)
                                    .ToList();
                var rs = db.Reservation
                                    .Where(x => x.KontaktUser.Id == model.KontaktUserId)
                                    .OrderBy(x => x.ReservationTime.ReservationDate)
                                    .ThenBy(x => x.ReservationTime.ReservationBegin)
                                    .ToList();
                model.isAdd = !rs.Any(x => !x.isPresent && x.ReservationTime.ReservationDate >= datenow);
                model.Reservations = new List<string[]>();
                model.Reservations.AddRange(rs.Select(r => new[] {                           
                        r.ReservationTime.ReservationDate.ToString("yyyy.MM.dd")
                        ,r.ReservationTime.ReservationBegin.ToString().Substring(0,5)
                        ,r.isPresent ? "Megjelent" : ""
                    }));
                   
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end with validation error");
                model.Refresh(ModelState);

                return View(model);
            }
        }


#endregion reservation

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




