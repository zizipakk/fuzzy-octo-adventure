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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Tax.Portal.Controllers
{
    public partial class SinoszController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

#region Index

        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult Index(string SinoszId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/Index()"))
            {
                log.Info("begin");
                ViewBag.SinoszId = SinoszId;
                log.Info("end");
                return View("");
            }
        }

        /// <summary>
        /// A fő keresési grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual System.Web.Mvc.JsonResult ListSinoszUsers(GridSettings grid, bool? isInactivator, string SinoszId)
        {
            JQGrid.Helpers.JsonResult result;

            string userId = User.Identity.GetUserId();
            Guid? orgId = null;
            bool isSinoszUser = false;
            var ur = db.ApplicationUserRole.FirstOrDefault(x =>
                                                                x.UserId == userId
                                                                && x.Role.Name == "SinoszUser");
            if (null != ur)
            {
                isSinoszUser = true;
                var kur = ur.KontaktUserRole;
                if (null != kur)
                {
                    var org = kur.Organization;
                    if (null != org)
                    {
                        orgId = org.Id;
                    }
                }
            }
            var rs0 = db.SinoszUser
                .Where( x => 
                        (
                            "" == SinoszId 
                            || x.SinoszId == SinoszId
                        )
                        &&
                        (
                            !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
                            || (null != x.Organization && x.Organization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
                            || (null != x.Organization.UpperOrganization && x.Organization.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
                        )
                      )
                .SelectMany(x => db.AccountingDocument.Where(y => y.Id == x.LastAccountingDocument).DefaultIfEmpty(), (s, a) => new { s, a })
                .SelectMany(v => db.Card.Where(z => z.Id == v.s.LastCard).DefaultIfEmpty(), (c, d) => new
                {
                    Id = c.s.Id,
                    StatusName = c.s.SinoszUserStatus.StatusName ?? "",
                    SinoszUserName = c.s.SinoszUserName,
                    SinoszId = c.s.SinoszId,
                    OrganizationName = c.s.Organization.OrganizationName ?? "",
                    DocumentDate = (DateTime?)c.a.DocumentDate ?? (DateTime?)null,
                    AccountingTypeName = null != c.a.AccountingType ? c.a.AccountingType.AccountingTypeName : "",
                    DocumnetNumber = c.a.DocumnetNumber ?? "",
                    BirthName = c.s.BirthName,
                    PostCode = c.s.Postcode.Code + " - " + c.s.Postcode.City ?? "",
                    HomeAddress = c.s.HomeAddress,
                    RelationshipName = c.s.Relationship.RelationshipName ?? "",
                    CardStatusName = d.CardStatus.CardStatusName,
                    BirthDate = c.s.BirthDate,
                    Qualification = c.s.Qualification,
                    BirthPlace = c.s.BirthPlace,
                    MothersName = c.s.MothersName,
                    Telephone = db.Address.FirstOrDefault(x => x.SinoszUser == c.s && x.AddressType.AddressTypeName == "Otthoni").AddressText,
                    GenusName = c.s.Genus.GenusName ?? "",
                    EducationName = c.s.Education.EducationName ?? "",
                    MaritalStatusName = c.s.MaritalStatus.MaritalStatusName ?? "",
                    EnterDate = c.s.EnterDate,
                    Remark = c.s.Remark,
                    HearingStatusName = c.s.HearingStatus.HearingStatusName ?? "",
                    InjuryTimeText = c.s.InjuryTime.InjuryTimeText ?? "",
                    PensionTypeName = c.s.PensionType.PensionTypeName ?? "",
                    DecreeNumber = c.s.DecreeNumber,
                    NationText = c.s.Nation.NationText ?? "",
                    Sum = (float?)c.a.Sum ?? 0,
                    Email = db.Address.FirstOrDefault(x => x.SinoszUser == c.s && x.AddressType.AddressTypeName == "E-mail").AddressText,
                    PositionName = c.s.Position.PositionName,
                    Barcode = c.s.Barcode
                })
                .OrderBy(x => x.StatusName)
                .ThenByDescending(x => x.EnterDate)
                .ThenBy(x => x.SinoszUserName);
                    
            //foreach (var item in rs0) { Debug.WriteLine(item); };

            if (null == isInactivator || false == isInactivator)
            {
                var rs = rs0.AsQueryable().GridPage(grid, out result).ToList();

                result.rows = (from r in rs
                               select new JsonRow
                               {
                                   id = r.Id.ToString(),
                                   cell = new string[] 
                               { 
                                r.Id.ToString()
                                ,r.StatusName
                                ,r.SinoszUserName
                                ,r.SinoszId
                                ,r.OrganizationName
                                ,r.DocumentDate.ToString()
                                ,r.AccountingTypeName
                                ,r.DocumnetNumber
                                ,r.BirthName
                                ,r.PostCode
                                ,r.HomeAddress
                                ,r.RelationshipName
                                ,r.CardStatusName
                                ,r.BirthDate.ToString()
                                ,r.Qualification
                                ,r.BirthPlace
                                ,r.MothersName
                                ,r.Telephone
                                ,r.GenusName
                                ,r.EducationName
                                ,r.MaritalStatusName
                                ,r.EnterDate.ToString()
                                ,r.Remark
                                ,r.HearingStatusName
                                ,r.InjuryTimeText
                                ,r.PensionTypeName
                                ,r.DecreeNumber
                                ,r.NationText
                                ,r.Sum.ToString()
                                ,r.Email
                                ,r.PositionName
                                ,r.Barcode
                               }
                               }).ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //csak szűrök, nem lapozok
                grid.PageIndex = 1;
                grid.PageSize = rs0.AsQueryable().Count();
                var rs = rs0.AsQueryable().GridPage(grid, out result);

                //inaktiválok
                SinoszUserStatus sus = db.SinoszUserStatus.SingleOrDefault(x => x.StatusName == "Inaktív");
                foreach (var r in rs.AsQueryable().ToList()) 
                {
                    var suser = db.SinoszUser.Find(r.Id);
                    if (suser.SinoszUserStatus != sus) { suser.SinoszUserStatus = sus; }
                }
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { success = false });
                }                
            }
        }

        /// <summary>
        /// Lenyílók a gridhez
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public virtual string ListOrganizations(string UserId, string RoleId)
        {
            string rolename = null;
            var role = db.Roles.Find(RoleId);
            if (null != role)
                rolename = role.Name;
            string userId = User.Identity.GetUserId();
            Guid? orgId = null;
            bool isSinoszUser = false;
            var ur = db.ApplicationUserRole.FirstOrDefault(x =>
                                                                x.UserId == userId
                                                                && x.Role.Name == "SinoszUser");
            if (null != ur)
            {
                isSinoszUser = true;
                var kur = ur.KontaktUserRole;
                if (null != kur)
                {
                    var org = kur.Organization;
                    if (null != org)
                    {
                        orgId = org.Id;
                    }
                }
            }

            string result = string.Empty;
            
            result = "<select>";

            foreach (var r in (from r in db.Organization
                               where 
                                    (null == RoleId //rendszerparameteren vagyok
                                    || rolename == "SinoszUser") //vagy rule-ban ezt a szerepet osztom
                                    &&
                                    (
                                        !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
                                        || (null != orgId && r.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
                                        || (null != r.UpperOrganization && r.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
                                    )
                               select new { value = r.Id, text = r.OrganizationName }
                               )
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListRelationships(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.Relationship
                               select new { value = r.Id, text = r.RelationshipName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListGenus(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.Genus
                               select new { value = r.Id, text = r.GenusName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListEducations(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.Education
                               select new { value = r.Id, text = r.EducationName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListMaritalStatus(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.MaritalStatus
                               select new { value = r.Id, text = r.MaritalStatusName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListHearingStatus(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.HearingStatus
                               select new { value = r.Id, text = r.HearingStatusName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListInjuryTimes(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.InjuryTime
                               select new { value = r.Id, text = r.InjuryTimeText })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListPensionTypes(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.PensionType
                               select new { value = r.Id, text = r.PensionTypeName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual string ListNations(string UserId)
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.Nation
                               select new { value = r.Id, text = r.NationText })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oper"></param>
        /// <param name="SinoszUserName"></param>
        /// <param name="OrganizationName"></param>
        /// <param name="PostCode"></param>
        /// <param name="HomeAddress"></param>
        /// <param name="BirthDate"></param>
        /// <param name="BirthPlace"></param>
        /// <param name="MothersName"></param>
        /// <param name="GenusName"></param>
        /// <param name="EnterDate"></param>
        /// <param name="HearingStatusName"></param>
        /// <param name="PensionTypeName"></param>
        /// <param name="DecreeNumber"></param>
        /// <param name="NationText"></param>
        /// <returns></returns>
        public virtual ActionResult UpdateSinoszUsers(string id, string oper, //SinoszUser r : a dátumkonverzió miatt inkább kifejtem
            string SinoszUserName,
            string OrganizationName,
            string PostCode,
            string HomeAddress,
            DateTime BirthDate,
            string BirthPlace,
            string MothersName,
            string GenusName,
            DateTime EnterDate,
            string HearingStatusName,
            string PensionTypeName,
            string DecreeNumber,
            string NationText)
        {
            Guid oGuid = null == OrganizationName ? Guid.Empty : Guid.Parse(OrganizationName);
            Guid pGuid = null == PostCode ? Guid.Empty : Guid.Parse(PostCode);
            Guid gGuid = null == GenusName ? Guid.Empty : Guid.Parse(GenusName);
            Guid hGuid = null == HearingStatusName ? Guid.Empty : Guid.Parse(HearingStatusName);
            Guid peGuid = null == PensionTypeName ? Guid.Empty : Guid.Parse(PensionTypeName);
            Guid nGuid = null == NationText ? Guid.Empty : Guid.Parse(NationText);

            SinoszUser res;

            switch (oper)
            {
                case "add":
                    res = db.SinoszUser.Create();
                    res.Id = Guid.NewGuid();
                    res.SinoszUserName = SinoszUserName;
                    res.Organization = db.Organization.SingleOrDefault(x => x.Id == oGuid);
                    res.Postcode = db.Postcode.SingleOrDefault(x => x.Id == pGuid);
                    res.HomeAddress = HomeAddress;
                    res.BirthDate = BirthDate;
                    res.BirthPlace = BirthPlace;
                    res.MothersName = MothersName;
                    res.Genus = db.Genus.SingleOrDefault(x => x.Id == gGuid);
                    res.EnterDate = EnterDate;
                    res.HearingStatus = db.HearingStatus.SingleOrDefault(x => x.Id == hGuid);
                    res.PensionType = db.PensionType.SingleOrDefault(x => x.Id == peGuid);
                    res.DecreeNumber = DecreeNumber;
                    res.Nation = db.Nation.SingleOrDefault(x => x.Id == nGuid);
//TODO : ez változott, külön action lett belőle
                    //res.SinoszId = (Int64.Parse(db.SinoszUser
                    //                            .Where(x => 
                    //                                x.SinoszId != ""
                    //                                && x.SinoszId != "függőben"
                    //                                && x.SinoszId != "nem vehető fel"
                    //                                && x.SinoszId != "nem vehető fel!"
                    //                                && x.SinoszId != "téves"
                    //                                && x.SinoszId != "téves rögzítés"
                    //                             )
                    //                             .Max(x => x.SinoszId)
                    //               ) + 1).ToString();
                    res.SinoszUserStatus = db.SinoszUserStatus.Single(x => x.StatusName == "Várakozó");
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion Index

#region Edit

        /// <summary>
        /// Sinosz tagi azonosító
        /// </summary>
        /// <param name="suserId"></param>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual System.Web.Mvc.JsonResult CreateSinoszId(Guid userId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/CreateSinoszId"))
            {
                log.Info("begin");
                var res = db.SinoszUser.Find(userId);

                if (null == res)
                {
                    log.Info("end: No user");
                    return Json(new { success = false });
                }
                else
                {
                    string first2 = DateTime.Now.Year.ToString("0000").Substring(2, 2); //az adott évszám vége
                    string second2 = res.BirthDate.Year.ToString("0000").Substring(2, 2); //a születési évszám vége
                    string third2 = res.BirthDate.Month.ToString("00"); //a születési évszám hónapja
                    Random rnd = new Random();
                    string forth1 = rnd.Next(0, 9).ToString(); //véletszerű szám 0-9
                    string fifth4 = "0001";
                    var filt = db.SinoszUser.Where(x =>
                                                    x.SinoszId.Length == 11
                                                    && x.SinoszId.Substring(0, 2) == first2)
                                            .AsQueryable();
                    if (null != filt)
                    {
                        //TODO: túlcsordulásnál mindig 0000 lesz a maximumkeresés miatt, vagyis évente csak 10000 új azonosítót tudunk kiadni, de pontosan ezt kérték
                        string temp = filt.Max(x => x.SinoszId.Substring(7, 4));
                        fifth4 = (Int32.Parse(filt.Max(x => x.SinoszId.Substring(7, 4))) + 1).ToString("00000").Substring(1, 4);
                    }
                    var data = new string[2];
                    data[0] = string.Format("{0}{1}{2}{3}{4}", first2, second2, third2, forth1, fifth4);

                    if (null == res.SinoszId && res.SinoszUserStatus.StatusName == "Várakozó")//még új és várakozó
                    {
                        res.SinoszUserStatus = db.SinoszUserStatus.FirstOrDefault(x => x.StatusName == "Aktív");
                        data[1] = res.SinoszUserStatus.StatusName;
                    }

                    res.SinoszId = data[0];
                    db.Entry(res).State = EntityState.Modified;
                    db.SaveChanges();

                    log.Info("end: OK");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult Edit(Guid? UserId, Guid? KontaktUserId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/Edit()"))
            {
                log.Info("begin");
                SinoszUser su;
                if (null != UserId)
                {
                    su = db.SinoszUser.Find(UserId);
                    var user = db.Users.FirstOrDefault(x => x.SinoszUser.Id == su.Id);
                    if (null != user)
                    {
                        if (null != user.KontaktUser)
                        {
                            if (user.KontaktUser.isElected)
                            {
                                ViewBag.isCommunication = user.KontaktUser.isCommunicationRequested;
                                ViewBag.isDevice = user.KontaktUser.isDeviceReqested;
                            }
                        }
                    }
                }
                else if (null != KontaktUserId)
                {
                    var u = db.Users.FirstOrDefault(x => x.KontaktUser.Id == KontaktUserId);
                    if (null != u)
                    {
                        if (null != u.SinoszUser)
                        {
                            su = u.SinoszUser;
                            ViewBag.isCommunication = u.KontaktUser.isCommunicationRequested;
                            ViewBag.isDevice = u.KontaktUser.isDeviceReqested;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                
                SinoszUserViewModel suvm = new SinoszUserViewModel(su);

                string userId = User.Identity.GetUserId();
                Guid? orgId = null;
                bool isSinoszUser = false;
                var ur = db.ApplicationUserRole.FirstOrDefault(x =>
                                                                    x.UserId == userId
                                                                    && x.Role.Name == "SinoszUser");
                if (null != ur)
                {
                    isSinoszUser = true;
                    var kur = ur.KontaktUserRole;
                    if (null != kur)
                    {
                        var org = kur.Organization;
                        if (null != org)
                        {
                            orgId = org.Id;
                        }
                    }
                }                
                suvm.OrganizationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.Organization
                                                    .Where(x => !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
                                                    || (null != orgId && x.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
                                                    || (null != x.UpperOrganization && x.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
                                                    )
                                                    .Select(x => new MyListItem { Value = x.Id, Text = x.OrganizationName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.PostcodeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.Postcode.Select(x => new MyListItem { Value = x.Id, Text = x.Code + "-" + x.City }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.GenusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.Genus.Select(x => new MyListItem { Value = x.Id, Text = x.GenusName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.HearingStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.HearingStatus.Select(x => new MyListItem { Value = x.Id, Text = x.HearingStatusName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.PensionTypeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.PensionType.Select(x => new MyListItem { Value = x.Id, Text = x.PensionTypeName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.NationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.Nation.Select(x => new MyListItem { Value = x.Id, Text = x.NationText }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.PositionList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.Position.Select(x => new MyListItem { Value = x.Id, Text = x.PositionName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.MaritalStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.MaritalStatus.Select(x => new MyListItem { Value = x.Id, Text = x.MaritalStatusName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.InjuryTimeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.InjuryTime.Select(x => new MyListItem { Value = x.Id, Text = x.InjuryTimeText }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                suvm.EducationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.Education.Select(x => new MyListItem { Value = x.Id, Text = x.EducationName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                //suvm.RelationshipList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                //                            .Union(db.Relationship.Select(x => new MyListItem { Value = x.Id, Text = x.RelationshipName }))
                //                            .OrderBy(x => x.Text)
                //                            .ToList();
                suvm.SinoszUserStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.SinoszUserStatus.Select(x => new MyListItem { Value = x.Id, Text = x.StatusName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                //egyszerűbb lita az állománytípusokhoz
                suvm.FileTypeList = db.FileType.Select(x => new MyListItem { Value = x.Id, Text = x.FileTypeName })
                                            .OrderBy(x => x.Text)
                                            .ToList();                
                var af = db.AttachedFile.FirstOrDefault(x => x.SinoszUser.Id == UserId && x.FileType == null);
                suvm.fileId = null == af ? null : (Guid?)af.FileId;
                
                log.Info("end");
                return View(suvm);
            }
        }


        [HttpPost]
        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult Edit(SinoszUserViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/Edit"))
            {
                log.Info("begin");

                if (ModelState.IsValid)
                {
                    SinoszUser resm = model.SinoszUserModel;
                    //csak így lehet módosítani a nav. prop-ot
                    var res = db.SinoszUser
                        .Include(o => o.Organization)
                        .Include(o => o.Postcode)
                        .Include(g => g.Genus)
                        .Include(h => h.HearingStatus)
                        .Include(p => p.PensionType)
                        .Include(n => n.Nation)
                        .Include(po => po.Position)
                        .Include(m => m.MaritalStatus)
                        .Include(i => i.InjuryTime)
                        .Include(e => e.Education)
                        .Include(r => r.Relationship)
                        .Include(s => s.SinoszUserStatus)                        
                        .Where(x => x.Id == resm.Id)
                        .FirstOrDefault();
                    if (null == res.Organization ?
                        Guid.Empty != resm.Organization.Id :
                        res.Organization.Id != resm.Organization.Id)  //csak itt van adat a modellben
                    { 
                        //SinoszLog-írás
                        var org = db.Organization.Find(resm.Organization.Id);
                        var slog = db.SinoszLog.Create();
                        slog.ApplicationUser = db.Users.SingleOrDefault(x => x.UserName == User.Identity.Name); //aki éppen be van lépve
                        slog.SinoszUser = res;
                        slog.ActionName = string.Format("Módosítás: {0} => {1}", null == res.Organization ? "" : res.Organization.OrganizationName, null == org ? "" : org.OrganizationName);
                        slog.ActionTime = DateTime.Now;
                        db.Entry(slog).State = EntityState.Added;
                        //nav prop beállítása
                        res.Organization = org; 
                    }
                    if (null == res.Postcode ?
                        Guid.Empty != resm.Postcode.Id :
                        res.Postcode.Id != resm.Postcode.Id) { res.Postcode = db.Postcode.Find(resm.Postcode.Id); }
                    if (null == res.Genus ?
                        Guid.Empty != resm.Genus.Id :
                        res.Genus.Id != resm.Genus.Id) { res.Genus = db.Genus.Find(resm.Genus.Id); }
                    if (null == res.HearingStatus ?
                        Guid.Empty != resm.HearingStatus.Id :
                        res.HearingStatus.Id != resm.HearingStatus.Id) { res.HearingStatus = db.HearingStatus.Find(resm.HearingStatus.Id); }
                    if (null == res.PensionType ?
                        Guid.Empty != resm.PensionType.Id :
                        res.PensionType.Id != resm.PensionType.Id) { res.PensionType = db.PensionType.Find(resm.PensionType.Id); }
                    if (null == res.Nation ?
                        Guid.Empty != resm.Nation.Id :
                        res.Nation.Id != resm.Nation.Id) { res.Nation = db.Nation.Find(resm.Nation.Id); }
                    if (null == res.Position ?
                        Guid.Empty != resm.Position.Id :
                        res.Position.Id != resm.Position.Id) { res.Position = db.Position.Find(resm.Position.Id); }
                    if (null == res.MaritalStatus ?
                        Guid.Empty != resm.MaritalStatus.Id :
                        res.MaritalStatus.Id != resm.MaritalStatus.Id) { res.MaritalStatus = db.MaritalStatus.Find(resm.MaritalStatus.Id); }
                    if (null == res.InjuryTime ?
                        Guid.Empty != resm.InjuryTime.Id :
                        res.InjuryTime.Id != resm.InjuryTime.Id) { res.InjuryTime = db.InjuryTime.Find(resm.InjuryTime.Id); }
                    if (null == res.Education ?
                        Guid.Empty != resm.Education.Id :
                        res.Education.Id != resm.Education.Id) { res.Education = db.Education.Find(resm.Education.Id); }
                    //if (null == res.Relationship ?
                    //    Guid.Empty != resm.Relationship.Id :
                    //    res.Relationship.Id != resm.Relationship.Id) { res.Relationship = db.Relationship.Find(resm.Relationship.Id); }
                    if (null == res.SinoszUserStatus ?
                        Guid.Empty != resm.SinoszUserStatus.Id :
                        res.SinoszUserStatus.Id != resm.SinoszUserStatus.Id) { res.SinoszUserStatus = db.SinoszUserStatus.Find(resm.SinoszUserStatus.Id); }
                    //már ez is módosulhat
                    if (res.SinoszId != resm.SinoszId) { res.SinoszId = resm.SinoszId; }
                    //egyéb
                    if (res.BirthDate != resm.BirthDate) { res.BirthDate = resm.BirthDate; }
                    if (res.BirthName != resm.BirthName) { res.BirthName = resm.BirthName; }
                    if (res.BirthPlace != resm.BirthPlace) { res.BirthPlace = resm.BirthPlace; }
                    if (res.DecreeNumber != resm.DecreeNumber) { res.DecreeNumber = resm.DecreeNumber; }
                    if (res.EnterDate != resm.EnterDate) { res.EnterDate = resm.EnterDate; }
                    if (res.HomeAddress != resm.HomeAddress) { res.HomeAddress = resm.HomeAddress; }
                    if (res.isHearingAid != resm.isHearingAid) { res.isHearingAid = resm.isHearingAid; }
                    if (res.isImplant != resm.isImplant) { res.isImplant = resm.isImplant; }
                    if (res.MothersName != resm.MothersName) { res.MothersName = resm.MothersName; }
                    if (res.Remark != resm.Remark) { res.Remark = resm.Remark; }
                    if (res.SinoszId != resm.SinoszId) { res.SinoszId = resm.SinoszId; }
                    if (res.SinoszUserName != resm.SinoszUserName) { res.SinoszUserName = resm.SinoszUserName; }
                    if (res.Barcode != resm.Barcode) { res.Barcode = resm.Barcode; }

                    db.SaveChanges();
                    log.Info("end with ok");

                    return RedirectToAction(MVC.Sinosz.Edit(model.SinoszUserModel.Id, null));                    
                }
                else
                {
//TODO béna megoldás átmenetileg, mert nem tudom különben átírni a hibaüzenetet, ami angol
                    if (ModelState["SinoszUserModel.BirthDate"].Errors.Count() > 0
                        && !ModelState["SinoszUserModel.BirthDate"].Errors.Select(x => x.ErrorMessage).Contains("A(z) [Születési idő] mezőt kötelező kitölteni"))
                    {
                        ModelState["SinoszUserModel.BirthDate"].Errors.Clear();
                        ModelState["SinoszUserModel.BirthDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Születési dátum] mezőben!"));
                    }
                    if (ModelState["SinoszUserModel.EnterDate"].Errors.Count() > 0
                        && !ModelState["SinoszUserModel.EnterDate"].Errors.Select(x => x.ErrorMessage).Contains("A(z) [Belépés ideje] mezőt kötelező kitölteni"))
                    {
                        ModelState["SinoszUserModel.EnterDate"].Errors.Clear();
                        ModelState["SinoszUserModel.EnterDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Belépési dátum] mezőben!"));
                    }
//

                    // If we got this far, something failed, redisplay form
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                    log.Info("end with validation error");
                    model.Refresh(ModelState);

                    //lenyílókat megint inicializálni kell
                    string userId = User.Identity.GetUserId();
                    Guid? orgId = null;
                    bool isSinoszUser = false;
                    var ur = db.ApplicationUserRole.FirstOrDefault(x =>
                                                                        x.UserId == userId
                                                                        && x.Role.Name == "SinoszUser");
                    if (null != ur)
                    {
                        isSinoszUser = true;
                        var kur = ur.KontaktUserRole;
                        if (null != kur)
                        {
                            var org = kur.Organization;
                            if (null != org)
                            {
                                orgId = org.Id;
                            }
                        }
                    }
                    model.OrganizationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.Organization
                                                        .Where(x => !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
                                                        || (null != orgId && x.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
                                                        || (null != x.UpperOrganization && x.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
                                                        )
                                                        .Select(x => new MyListItem { Value = x.Id, Text = x.OrganizationName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.PostcodeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.Postcode.Select(x => new MyListItem { Value = x.Id, Text = x.Code + "-" + x.City }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.GenusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.Genus.Select(x => new MyListItem { Value = x.Id, Text = x.GenusName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.HearingStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.HearingStatus.Select(x => new MyListItem { Value = x.Id, Text = x.HearingStatusName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.PensionTypeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.PensionType.Select(x => new MyListItem { Value = x.Id, Text = x.PensionTypeName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.NationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.Nation.Select(x => new MyListItem { Value = x.Id, Text = x.NationText }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.PositionList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.Position.Select(x => new MyListItem { Value = x.Id, Text = x.PositionName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.MaritalStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.MaritalStatus.Select(x => new MyListItem { Value = x.Id, Text = x.MaritalStatusName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.InjuryTimeList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.InjuryTime.Select(x => new MyListItem { Value = x.Id, Text = x.InjuryTimeText }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    model.EducationList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.Education.Select(x => new MyListItem { Value = x.Id, Text = x.EducationName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    //model.RelationshipList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                    //                            .Union(db.Relationship.Select(x => new MyListItem { Value = x.Id, Text = x.RelationshipName }))
                    //                            .OrderBy(x => x.Text)
                    //                            .ToList();
                    model.SinoszUserStatusList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                                .Union(db.SinoszUserStatus.Select(x => new MyListItem { Value = x.Id, Text = x.StatusName }))
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    //egyszerűbb lsita az állománytípusokhoz
                    model.FileTypeList = db.FileType.Select(x => new MyListItem { Value = x.Id, Text = x.FileTypeName })
                                                .OrderBy(x => x.Text)
                                                .ToList();
                    //ez action-ön kívül változhat
                    var af = db.AttachedFile.FirstOrDefault(x => x.SinoszUser.Id == model.SinoszUserModel.Id && x.FileType == null);
                    model.fileId = null == af ? null : (Guid?)af.FileId;

                    return View(model);
                }
            }
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListAddresses(GridSettings grid, Guid uid)
        {
            JQGrid.Helpers.JsonResult result;

            //a típus egy az egyes
            var rs0 = db.Address
                       .Include(t => t.AddressType)
                       .Where(x => x.SinoszUser.Id == uid)
                       .Select(
                            x => new
                            {
                                Id = x.Id,
                                AddressTypeName = null == x.AddressType ? "" : x.AddressType.AddressTypeName,
                                AddressText = x.AddressText
                            }
                        );

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                string.Empty
                                ,r.Id.ToString()
                                ,r.AddressTypeName
                                ,r.AddressText
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateAddresses(string id, string oper, Address r, string AddressTypeName, string uid)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
            if (null != AddressTypeName) //törlésnél
            {
                Guid typeId = Guid.Parse(AddressTypeName);
                r.AddressType = db.AddressType.Find(typeId);
            }

            if (null != uid) //törlésnél
            {
                Guid userId = Guid.Parse(uid);
                r.SinoszUser = db.SinoszUser.Find(userId);
            }


            Address res;

            switch (oper)
            {
                case "edit":
                    res = db.Address.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.AddressType != r.AddressType) { res.AddressType = r.AddressType; }
                    if (res.AddressText != r.AddressText) { res.AddressText = r.AddressText; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.Address.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.Address.Create();
                    res.Id = r.Id;
                    res.AddressType = r.AddressType;
                    res.SinoszUser = r.SinoszUser;
                    res.AddressText = r.AddressText;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        public virtual string ListAddressTypes()
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in db.AddressType
                                .Select(x => new { value = x.Id, text = x.AddressTypeName })
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
        public virtual System.Web.Mvc.JsonResult ListLogs(GridSettings grid, Guid uid)
        {
            JQGrid.Helpers.JsonResult result;

            //a típus egy az egyes
            var rs0 = db.SinoszLog
                       .Where(x => x.SinoszUser.Id == uid)
                       .Select(
                            x => new
                            {
                                Id = x.Id,
                                ActionTime = x.ActionTime,
                                UserName = x.ApplicationUser.UserName,
                                ActionName = x.ActionName
                           }
                        )
                        .OrderByDescending(x => x.ActionTime);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                r.Id.ToString()
                                ,r.ActionTime.ToString()
                                ,r.UserName
                                ,r.ActionName
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListDocuments(GridSettings grid, Guid uid)
        {
            JQGrid.Helpers.JsonResult result;

            //a típus egy az egyes
            var rs0 = db.AccountingDocument
                       .Where(x => x.SinoszUser.Id == uid)
                       .Select(
                            x => new
                            {
                                adId = x.Id,
                                DocumentDate = x.DocumentDate,
                                AccountingTypeName = x.AccountingType.AccountingTypeName,
                                Sum = x.Sum,
                                DocumnetNumber = x.DocumnetNumber,
                                AccountingStatusName = null == x.AccountingStatus ? true : "Tény" == x.AccountingStatus.AccountingStatusName ? false : true
                            }
                        )
                        .OrderByDescending(x => x.DocumentDate);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.adId.ToString(),
                               cell = new string[] 
                               {
                                r.adId.ToString()
                                ,r.DocumentDate.ToString()
                                ,r.AccountingTypeName
                                ,r.Sum.ToString()
                                ,r.DocumnetNumber
                                ,r.AccountingStatusName.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public virtual string ListAccountingTypes(Guid userId)
        //{
        //    string result = string.Empty;

        //    result = "<select>";

        //    foreach (var r in db.AccountingType
        //                        .Join(db.SinoszUser, a => a.Relationship, b => b.Relationship, (a, b) => new { a, suserId = b.Id })
        //                        .Where(x => x.suserId == userId)
        //                        .Select(x => new { value = x.a.Id, text = x.a.AccountingTypeName })
        //                        .OrderBy(x => x.text)
        //                        .ToList())
        //    {
        //        result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
        //    }

        //    result += "</select>";

        //    return result;
        //}

        public virtual string ListAccountingTypes()
        {
            string result = string.Empty;

            result = "<select>";

            //foreach (var r in db.AccountingType
            //                    .Join(db.SinoszUser, a => a.Relationship, b => b.Relationship, (a, b) => new { a, suserId = b.Id })
            //                    .Where(x => x.suserId == userId)
            //                    .Select(x => new { value = x.a.Id, text = x.a.AccountingTypeName })
            //                    .OrderBy(x => x.text)
            //                    .ToList())

            result += string.Format("<option value='{0}'>{1}</option>", "", ""); //első sor

            foreach (var r in db.AccountingType.Where(x => x.isEnabled).Select(x => new { value = x.Id, text = x.AccountingTypeName })
                    .OrderBy(x => x.text)
                    .ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual System.Web.Mvc.JsonResult UpdateDocuments(string id, string oper, AccountingDocument a, string AccountingTypeName, string uid)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/UpdateDocuments"))
            {
                log.Info("begin");
                a.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
                if (null != AccountingTypeName) //törlésnél
                {
                    Guid typeId = Guid.Parse(AccountingTypeName);
                    a.AccountingType = db.AccountingType.Find(typeId);
                }
                if (null != uid) //törlésnél
                {
                    Guid userId = Guid.Parse(uid);
                    a.SinoszUser = db.SinoszUser.Find(userId);
                }

                AccountingDocument res;

                var response = new string[3];

                switch (oper)
                {
                    case "add":
    //TODO: a típusadatokból kellene kitölteni egy csomó mindent
    //TODO: Card is fel kell vinni, ha kell!
                        res = db.AccountingDocument.Create();
                        res.Id = a.Id;
                        res.DocumentDate = a.DocumentDate;
                        res.AccountingType = a.AccountingType;
                        res.Sum = a.Sum;
                        res.DocumnetNumber = a.DocumnetNumber;
                        res.AccountingStatus = db.AccountingStatus.Single(x => x.AccountingStatusName == "Tény");
                        res.SinoszUser = a.SinoszUser;
                        db.Entry(res).State = EntityState.Added;
                        SinoszUser su = a.SinoszUser;
                        //Gyorskereső mező beállítása
                        var old = db.AccountingDocument
                                            .Where(y => y.SinoszUser.Id == a.SinoszUser.Id
                                                && y.AccountingType.AccountingTypeName != "Visszafizetés"
                                                && y.AccountingStatus.AccountingStatusName == "Tény")
                                            .OrderByDescending(y => y.DocumentDate)
                                            .FirstOrDefault();
                        if (null != old)
                        {
                            if (old.DocumentDate <= a.DocumentDate)
                            {
                                su.LastAccountingDocument = a.Id;
                                db.Entry(su).State = EntityState.Modified;
                            }
                        }
                        //ha tagdíj, akkor átállítom tagviszonyt, és ha nem várakozó, bebillentem Aktívvá
                        if (a.AccountingType.isMembershipCost)
                        {
                            if (su.Relationship != a.AccountingType.Relationship)
                            {
                                su.Relationship = a.AccountingType.Relationship;
                                response[0] = a.AccountingType.Relationship.RelationshipName;
                            }                            
                            if (su.SinoszUserStatus.StatusName != "Várakozó"
                                && su.SinoszUserStatus.StatusName != "Aktív")
                            {
                                su.SinoszUserStatus = db.SinoszUserStatus.FirstOrDefault(x => x.StatusName == "Aktív");
                                response[1] = su.SinoszUserStatus.StatusName;
                            }
                            db.Entry(su).State = EntityState.Modified;
                        }
                        response[2] = a.AccountingType.isCardCost.ToString();
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
            log.Info("end");
            //return null;
            return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        public virtual System.Web.Mvc.JsonResult GetDocumentTypeParams(Guid typeId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/GetDocumentTypeParams"))
            {
                log.Info("begin");
                var response = new string[2];
                var res = db.AccountingType.Find(typeId);
                if (null != res)
                {
                    response[0] = res.Presum.ToString();
                    response[1] = res.isFixsum.ToString();                
                }
                log.Info("end");
                //return null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public virtual ActionResult UpdateDocumnetStatus(string docid, bool status)
        {
            if (null != docid)
            {
                Guid docGuid = Guid.Parse(docid);
                AccountingDocument a = db.AccountingDocument.Find(docGuid);
                //Gyorskereső mező beállítása
                AccountingDocument old = db.AccountingDocument
                                    .Where(y => y.SinoszUser.Id == a.SinoszUser.Id
                                        && y.AccountingType.AccountingTypeName != "Visszafizetés"
                                        && y.AccountingStatus.AccountingStatusName == "Tény")
                                    .OrderByDescending(y => y.DocumentDate)
                                    .FirstOrDefault(y => y.SinoszUser != null);
                SinoszUser su;
                if (status) 
                {
                    a.AccountingStatus = db.AccountingStatus.Single(x => x.AccountingStatusName == "Törölve");
                    //Gyorskereső mező beállítása
                    if (null != old)
                    {
                        su = db.SinoszUser.Find(old.SinoszUser.Id);
                        su.LastAccountingDocument = a.Id;                        
                    }
                    else
                    {
                        su = db.SinoszUser.Find(a.SinoszUser.Id);
                        su.LastAccountingDocument = null;
                    }
                    //db.Entry(su).State = EntityState.Modified;
                }
                else
                {
                    a.AccountingStatus = db.AccountingStatus.Single(x => x.AccountingStatusName == "Tény");
                    //Gyorskereső mező beállítása
                    if (null != old)
                    {
                        if (old.DocumentDate <= a.DocumentDate)
                        {
                            su = db.SinoszUser.Find(a.SinoszUser.Id);
                            su.LastAccountingDocument = a.Id;
                            //db.Entry(su).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        su = db.SinoszUser.Find(a.SinoszUser.Id);
                        su.LastAccountingDocument = a.Id;
                        //db.Entry(su).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }
            return null;
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListCards(GridSettings grid, string uid)
        {
            Guid uGuid = "null" == uid ? Guid.Empty : Guid.Parse(uid);

            JQGrid.Helpers.JsonResult result;

            //a típus egy az egyes
            var rs0 = db.Card
                       .Where(x => x.SinoszUser.Id == uGuid)
                       .Select(
                            x => new
                            {
                                Id = x.Id,
                                CreateDate = x.CreateDate,
                                CardStatusName = x.CardStatus.CardStatusName,
                                Remark = x.Remark
                            }
                        )
                        .OrderByDescending(x => x.CreateDate);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.CreateDate.ToString()
                                ,r.CardStatusName
                                ,r.Remark
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual string ListCardStatuses()
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in db.CardStatus
                                .Select(x => new { value = x.Id, text = x.CardStatusName })
                                .OrderBy(x => x.text)
                                .ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual ActionResult UpdateCards(string id, string oper, Card r, string CardStatusName, string uid)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
            if (null != CardStatusName) //törlésnél
            {
                Guid cardGuid = Guid.Parse(CardStatusName);
                r.CardStatus = db.CardStatus.Find(cardGuid);
            }
            else
            {
                r.CardStatus = db.CardStatus.FirstOrDefault(x => x.CardStatusName == "Regisztrálva");
            }
            if (null != uid) //törlésnél
            {
                Guid uGuid = Guid.Parse(uid);
                r.SinoszUser = db.SinoszUser.Find(uGuid);
            }

            Card res;

            switch (oper)
            {
                case "edit":
                    res = db.Card.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.CardStatus != r.CardStatus) { res.CardStatus = r.CardStatus; }
                    if (res.Remark != r.Remark) { res.Remark = r.Remark; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.Card.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.Card.Create();
                    res.Id = r.Id;
                    res.CardStatus = r.CardStatus;
                    res.CreateDate = DateTime.Now;
                    res.Remark = r.Remark;
                    res.SinoszUser = r.SinoszUser;
                    db.Entry(res).State = EntityState.Added;
                    //a gyorskereső mező
                    SinoszUser su = r.SinoszUser;
                    su.LastCard = r.Id;
                    db.Entry(su).State = EntityState.Modified;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListFiles(GridSettings grid, string uid)
        {
            Guid uGuid = "null" == uid ? Guid.Empty : Guid.Parse(uid);

            JQGrid.Helpers.JsonResult result;

            var rs0 = db.AttachedFile
                       .Where(x => x.SinoszUser.Id == uGuid && x.FileType != null)//a fénykép nem kell
                       .Join(db.File, a => a.FileId, b => b.stream_id, (a, b) => new { a, b})
                       .Select(
                            x => new
                            {
                                Id = x.a.Id,
                                FileTypeName = x.a.FileType.FileTypeName,
                                Name = x.b.name.Substring(x.b.name.IndexOf("_")+1),
                                FileId = x.a.FileId
                            }
                        )
                        .OrderBy(x => x.FileTypeName);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.FileTypeName
                                ,r.Name
                                ,string.Empty
                                ,r.FileId.ToString()
                               }
                           }).ToArray();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual string ListFileTypes()
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in db.FileType
                                .Select(x => new { value = x.Id, text = x.FileTypeName })
                                .OrderBy(x => x.text)
                                .ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        public virtual ActionResult UpdateFiles(string id, string oper, AttachedFile r, string FileTypeName, string uid)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
            if (null != FileTypeName) //törlésnél
            {
                Guid ftGuid = Guid.Parse(FileTypeName);
                r.FileType = db.FileType.Find(ftGuid);
            }
            if (null != uid) //törlésnél
            {
                Guid uGuid = Guid.Parse(uid);
                r.SinoszUser = db.SinoszUser.Find(uGuid);
            }

            AttachedFile res;

            switch (oper)
            {
                case "edit":
                    res = db.AttachedFile.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.FileType != r.FileType) { res.FileType = r.FileType; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.AttachedFile.Find(r.Id);
                    File fl = db.File.Find(res.FileId);
                    db.Entry(res).State = EntityState.Deleted;
                    db.Entry(fl).State = EntityState.Deleted;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

        /// <summary>
        /// Az előírt sorrendben átstátuszolja a delikvenst
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual ActionResult DeleteSinoszUser(Guid userId)
        {
            var suser = db.SinoszUser.Find(userId);
            if (null != suser)
            {
//TODO: megváltozott
                //var nextSta = db.StatusToStatus.SingleOrDefault(x => x.FromStatus.Id == suser.SinoszUserStatus.Id);
                //if (null != nextSta) //egyirányú út
                //{
                //    suser.SinoszUserStatus = nextSta.ToStatus;
                //    db.Entry(suser).State = EntityState.Modified;
                //    db.SaveChanges();
                //}
                switch (suser.SinoszUserStatus.StatusName)
                {
                    case "Aktív":
                        suser.SinoszUserStatus = db.SinoszUserStatus.FirstOrDefault(x => x.StatusName == "Inaktív");
                        break;
                    case "Inaktív":
                        suser.SinoszUserStatus = db.SinoszUserStatus.FirstOrDefault(x => x.StatusName == "Törölt");
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { UserId = suser.Id });
        }

#endregion Edit

#region Parameter

        [Authorize(Roles = "SysAdmin, SinoszAdmin")]
        public virtual ActionResult Parameter()
        {
            log.Info("begin");
            log.Info("end");
            return View();
        }

#region DocumentType

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListDocumenttypes(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.AccountingType
                       .Select(
                            x => new
                            {
                                Id = x.Id,
                                AccountingTypeName = x.AccountingTypeName,
                                isFixsum = x.isFixsum,
                                Presum = x.Presum,
                                isMembershipCost = x.isMembershipCost,
                                isCardCost = x.isCardCost,
                                RelationshipName = x.Relationship.RelationshipName,
                                isEnabled = x.isEnabled,
                                AccountingDocumentCount = x.AccountingDocument.Count()
                            }
                        )
                        .OrderByDescending(x => x.AccountingTypeName);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.AccountingTypeName
                                ,r.isFixsum.ToString()
                                ,r.Presum.ToString()
                                ,r.isMembershipCost.ToString()
                                ,r.isCardCost.ToString()
                                ,r.RelationshipName
                                ,r.isEnabled.ToString()
                                ,r.AccountingDocumentCount.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateisFixsum(string doctypeid, bool value)
        {
            if (null != doctypeid)
            {
                Guid doctypeGuid = Guid.Parse(doctypeid);
                var a = db.AccountingType.Find(doctypeGuid);
                if (a.isFixsum != value)
                {
                    a.isFixsum = value;
                    db.SaveChanges();
                }
            }
            return null;
        }

        public virtual ActionResult UpdateisMembershipCost(string doctypeid, bool value)
        {
            if (null != doctypeid)
            {
                Guid doctypeGuid = Guid.Parse(doctypeid);
                var a = db.AccountingType.Find(doctypeGuid);
                if (a.isMembershipCost != value)
                {
                    a.isMembershipCost = value;
                    db.SaveChanges();
                }
            }
            return null;
        }

        public virtual ActionResult UpdateisCardCost(string doctypeid, bool value)
        {
            if (null != doctypeid)
            {
                Guid doctypeGuid = Guid.Parse(doctypeid);
                var a = db.AccountingType.Find(doctypeGuid);
                if (a.isCardCost != value)
                {
                    a.isCardCost = value;
                    db.SaveChanges();
                }
            }
            return null;
        }

        public virtual ActionResult UpdateisEnabled(string doctypeid, bool value)
        {
            if (null != doctypeid)
            {
                Guid doctypeGuid = Guid.Parse(doctypeid);
                var a = db.AccountingType.Find(doctypeGuid);
                if (a.isEnabled != value)
                {
                    a.isEnabled = value;
                    db.SaveChanges();
                }
            }
            return null;
        }

        public virtual ActionResult UpdateDocumenttypes(string id, string oper, AccountingType r, string RelationshipName)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
            if (null != RelationshipName) //törlésnél
            {
                Guid relId = Guid.Parse(RelationshipName);
                r.Relationship = db.Relationship.Find(relId);
            }

            AccountingType res;

            switch (oper)
            {
                case "edit":
                    res = db.AccountingType.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.AccountingTypeName != r.AccountingTypeName) { res.AccountingTypeName = r.AccountingTypeName; }
                    if (res.isCardCost != r.isCardCost) { res.isCardCost = r.isCardCost; }
                    if (res.isEnabled != r.isEnabled) { res.isEnabled = r.isEnabled; }
                    if (res.isFixsum != r.isFixsum) { res.isFixsum = r.isFixsum; }
                    if (res.isMembershipCost != r.isMembershipCost) { res.isMembershipCost = r.isMembershipCost; }
                    if (res.Presum != r.Presum) { res.Presum = r.Presum; }
                    if (res.Relationship != r.Relationship) { res.Relationship = r.Relationship; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.AccountingType.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.AccountingType.Create();
                    res.Id = r.Id;
                    res.AccountingTypeName = r.AccountingTypeName;
                    res.isCardCost = r.isCardCost;
                    res.isEnabled = r.isEnabled;
                    res.isFixsum = r.isFixsum;
                    res.isMembershipCost = r.isMembershipCost;
                    res.Presum = r.Presum;
                    res.Relationship = r.Relationship;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion DocumentType

#region Organization

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListOrganizationParams(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            //amik szereplenek sinoszuseren jelzem
            var rs0 = db.Organization
                          .Select(x => new
                          {
                              Id = x.Id,
                              OrganizationName = x.OrganizationName,
                              Country = null == x.Postcode ? "" : x.Postcode.Country,
                              Postcode = null == x.Postcode ? "" : x.Postcode.Code,
                              City = null == x.Postcode ? "" : x.Postcode.City,
                              Address = x.Address,
                              UpperOrganization = null == x.UpperOrganization ? "" : x.UpperOrganization.OrganizationName,
                              OrganizationCount = x.SinoszUser.Count(),
                              upperId = null == x.UpperOrganization ? Guid.Empty : x.UpperOrganization.Id
                          }).AsEnumerable();

            //amik felsőbb szervek jelzem
            var rs1 = rs0
                        .Where(x => x.upperId != Guid.Empty)
                        .GroupBy(x => x.upperId)
                        .Select(x => new
                        {
                            upperId = x.FirstOrDefault().upperId,
                            Count = x.Count()
                        }).AsEnumerable().ToList(); //a tolist() nélkül nem fűz

            //foreach (var item in rs1) { Debug.WriteLine(item); }

            var rs2 = (from a in rs0
                       from b in rs1.Where(x => x.upperId == a.Id).DefaultIfEmpty()
                       select new
                       {
                           Id = a.Id,
                           OrganizationName = a.OrganizationName,
                           Country = a.Country,
                           Postcode = a.Postcode,
                           City = a.City,
                           Address = a.Address,
                           UpperOrganization = a.UpperOrganization,
                           OrganizationCount = a.OrganizationCount + (null == b ? 0 : b.Count) //vagy sinoszuseren szerepel vagy felsőbb szervként szerepel
                       }).OrderBy(x => x.OrganizationName).AsEnumerable();

            //foreach (var item in rs2) { Debug.WriteLine(item); }

            var rs = rs2.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.OrganizationName
                                ,r.Country
                                ,r.Postcode
                                ,r.City
                                ,r.Address
                                ,r.UpperOrganization
                                ,r.OrganizationCount.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateOrganizations(string id, string oper, Organization r, string Postcode, string UpperOrganization)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();
            if (null != Postcode) //törlésnél
            {
                Guid postId = Guid.Parse(Postcode);
                r.Postcode = db.Postcode.Find(postId);
            }
            if (null != UpperOrganization) //törlésnél
            {
                Guid uorgId = Guid.Parse(UpperOrganization);
                r.UpperOrganization = db.Organization.Find(uorgId);
            }

            Organization res;

            switch (oper)
            {
                case "edit":
                    res = db.Organization.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.OrganizationName != r.OrganizationName) { res.OrganizationName = r.OrganizationName; }
                    if (res.Postcode != r.Postcode) { res.Postcode = r.Postcode; }
                    if (res.Address != r.Address) { res.Address = r.Address; }
                    if (res.UpperOrganization != r.UpperOrganization) { res.UpperOrganization = r.UpperOrganization; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.Organization.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.Organization.Create();
                    res.Id = r.Id;
                    res.OrganizationName = r.OrganizationName;
                    res.Postcode = r.Postcode;
                    res.Address = r.Address;
                    res.UpperOrganization = r.UpperOrganization;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion Organization

#region AddressType

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListAddressTypeParams(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.AddressType
                          .Select(x => new
                          {
                              Id = x.Id,
                              AddressTypeName = x.AddressTypeName,
                              AddressTypeCount = x.Address.Count(),
                          }).OrderBy(x => x.AddressTypeName);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.AddressTypeName
                                ,r.AddressTypeCount.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateAddressTypes(string id, string oper, AddressType r)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            AddressType res;

            switch (oper)
            {
                case "edit":
                    res = db.AddressType.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.AddressTypeName != r.AddressTypeName) { res.AddressTypeName = r.AddressTypeName; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.AddressType.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.AddressType.Create();
                    res.Id = r.Id;
                    res.AddressTypeName = r.AddressTypeName;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion AddressType

#region FileType

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListFileTypeParams(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.FileType
                          .Select(x => new
                          {
                              Id = x.Id,
                              FileTypeName = x.FileTypeName,
                              AttachedFileCount = x.AttachedFile.Count(),
                          }).OrderBy(x => x.FileTypeName);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.FileTypeName
                                ,r.AttachedFileCount.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateFileTypes(string id, string oper, FileType r)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            FileType res;

            switch (oper)
            {
                case "edit":
                    res = db.FileType.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.FileTypeName != r.FileTypeName) { res.FileTypeName = r.FileTypeName; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.FileType.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.FileType.Create();
                    res.Id = r.Id;
                    res.FileTypeName = r.FileTypeName;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion FileType

#region PensionType

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListPensionTypeParams(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.PensionType
                          .Select(x => new
                          {
                              Id = x.Id,
                              PensionTypeName = x.PensionTypeName,
                              SinoszUserCount = x.SinoszUser.Count(),
                          }).OrderBy(x => x.PensionTypeName);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.PensionTypeName
                                ,r.SinoszUserCount.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdatePensionTypes(string id, string oper, PensionType r)
        {
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            PensionType res;

            switch (oper)
            {
                case "edit":
                    res = db.PensionType.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    if (res.PensionTypeName != r.PensionTypeName) { res.PensionTypeName = r.PensionTypeName; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.PensionType.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.PensionType.Create();
                    res.Id = r.Id;
                    res.PensionTypeName = r.PensionTypeName;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion PensionType

#region NewsCreation

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        public virtual System.Web.Mvc.JsonResult ListNewsParams(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.News
                          .Select(x => new
                          {
                              Id = x.Id,
                              PublishingDate = x.PublishingDate,
                              NewsTitle = x.NewsTitle,
                              NewsText = x.NewsText,
                              NewsTypeName = x.NewsType.NewsTypeName,
                          }).OrderByDescending(x => x.PublishingDate).ThenBy(y => y.NewsTypeName);

            var rs = rs0.AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                r.Id.ToString()
                                ,r.PublishingDate.ToString()
                                ,r.NewsTitle
                                ,r.NewsText
                                ,r.NewsTypeName
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lenyílók a gridhez
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public virtual string ListNewsTypes()
        {
            string result = string.Empty;

            result = "<select>";

            foreach (var r in (from r in db.NewsType
                               select new { value = r.Id, text = r.NewsTypeName })
                               .OrderBy(x => x.text).ToList())
            {
                result += string.Format("<option value='{0}'>{1}</option>", r.value.ToString(), r.text);
            }

            result += "</select>";

            return result;
        }

        //TODO: nem szép, de gridnél nincs viewmodel, ezért nem megy az AllowHtml, XSS-sel mehetünk később
        [HttpPost, ValidateInput(false)]
        public virtual ActionResult UpdateNews(string id, string oper, News r, string NewsTypeName)
        {
            //lapozáskor az id a pillanatnyi oldal értékét adja vissza
            r.Id = "_empty" != id ? Guid.Parse(id) : Guid.NewGuid();

            if (null != NewsTypeName) //törlésnél
            {
                Guid newId = Guid.Parse(NewsTypeName);
                r.NewsType = db.NewsType.Find(newId);
            }

            News res;

            switch (oper)
            {
                case "edit":
                    res = db.News.Find(r.Id);
                    if (res.Id != r.Id) { res.Id = r.Id; }
                    //TODO: most még nem fogjukhasználni
                    //                    if (res.PublishingDate != r.PublishingDate) { res.PublishingDate = r.PublishingDate; }
                    if (res.NewsTitle != r.NewsTitle) { res.NewsTitle = r.NewsTitle; }
                    if (res.NewsText != r.NewsText) { res.NewsText = r.NewsText; }
                    if (res.NewsType != r.NewsType) { res.NewsType = r.NewsType; }
                    db.Entry(res).State = EntityState.Modified;
                    break;
                case "del":
                    res = db.News.Find(r.Id);
                    db.Entry(res).State = EntityState.Deleted;
                    break;
                case "add":
                    res = db.News.Create();
                    res.Id = r.Id;
                    //TODO: most még nem fogjuk használni.
                    res.PublishingDate = DateTime.Now;//r.PublishingDate;
                    res.NewsTitle = r.NewsTitle;
                    res.NewsText = r.NewsText;
                    res.NewsType = r.NewsType;
                    db.Entry(res).State = EntityState.Added;
                    break;
                default:
                    break;
            }
            db.SaveChanges();
            return null;
        }

#endregion NewsCreation

#endregion Parameter

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
                AttachedFile af = db.AttachedFile.Create();
                af.FileId = id;
                af.FileType = db.FileType.SingleOrDefault(x => x.Id == filetypeId);
                af.SinoszUser = db.SinoszUser.SingleOrDefault(x => x.Id == suserId);
                db.Entry(af).State = EntityState.Added;
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
        public async Task<System.Web.Mvc.JsonResult> DeleteAttachement(Guid id)
        {
            try
            {
                AttachedFile af = db.AttachedFile.SingleOrDefault(x => x.FileId == id);
                db.Entry(af).State = EntityState.Deleted;
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
        public async Task<FileResult> DownloadFirst(Guid fileId)
        {
            try
            {
                //AttachedFile af = db.AttachedFile.FirstOrDefault(x => (x.FileId == fileId) || (x.SinoszUser.Id == suserId && x.FileType.Id == filetypeId));
                AttachedFile af = db.AttachedFile.FirstOrDefault(x => x.FileId == fileId);
                if (null != af)
                {
                    var downloadTask = DownloadTask(af.FileId);
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
                return null;
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }
        }

        /// <summary>
        /// feltöltés után vissza kell adni a guid-ot az esetleges törléshez
        /// </summary>
        /// <param name="suserId"></param>
        /// <returns></returns>
        public virtual System.Web.Mvc.JsonResult GetUserPictId(Guid suserId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/GetUserPictId"))
            {
                log.Info("begin");
                string userpictId = db.AttachedFile.FirstOrDefault(x => x.SinoszUser.Id == suserId && null == x.FileType).FileId.ToString();
                log.Info("end");

                if (null == userpictId)
                {
                    return Json(new { success = false });
                }
                else
                {
                    return Json(userpictId, JsonRequestBehavior.AllowGet);
                }
            }
        }

#endregion filestream

#region NewsView

        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult New()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/New"))
            {
                log.Info("begin");

                var resWellcome = db.News.Where(n => n.NewsType.NewsTypeName == "Üdvözlet").OrderByDescending(y => y.PublishingDate).Take(1).AsEnumerable();

                var resNews = db.News.Where(y => y.NewsType.NewsTypeName == "Hír").OrderByDescending(y => y.PublishingDate).Take(5).AsEnumerable();

                var resFull = resWellcome.Union(resNews);

                string txtFull = null;
                foreach (var item in resFull) 
                { 
                    Debug.WriteLine(item);
                    txtFull = txtFull
                                + item.NewsTitle + "</br></br>" //cím után enter és erre is kellne editor
                                + item.NewsText; //a szerző adatait is iderakhatnám
                }

                NewViewModel nvm = new NewViewModel()
                {
                    FullText = txtFull
                };

                log.Info("end");

                return View(nvm);
            }
        }


#endregion NewsView

#region List

        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult List()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Sinosz/List"))
            {
                log.Info("begin");
                log.Info("end");
                return View("");
            }
        }

        /// <summary>
        /// Alaplista grid adatforrása 
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult ListSinoszUsersForList(GridSettings grid, string Output)
        {
            JQGrid.Helpers.JsonResult result;

            string userId = User.Identity.GetUserId();
            Guid? orgId = null;
            bool isSinoszUser = false;
            var ur = db.ApplicationUserRole.FirstOrDefault(x =>
                                                                x.UserId == userId
                                                                && x.Role.Name == "SinoszUser");
            if (null != ur)
            {
                isSinoszUser = true;
                var kur = ur.KontaktUserRole;
                if (null != kur)
                {
                    var org = kur.Organization;
                    if (null != org)
                    {
                        orgId = org.Id;
                    }
                }
            }           

            //lehetne még gyorsítani az address "bekötésével"
            var rs0 = db.SinoszUser
                .Where(x => !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
                            || (null != orgId && x.Organization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
                            || (null != x.Organization.UpperOrganization && x.Organization.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
                            )
                .SelectMany(x => db.AccountingDocument.Where(y => y.Id == x.LastAccountingDocument).DefaultIfEmpty(), (a, b) => new { a, b })
                .SelectMany(v => db.Card.Where(z => z.Id == v.a.LastCard).DefaultIfEmpty(), (c, d) => new
                {
                    Id = c.a.Id,
                    SinoszUserName = c.a.SinoszUserName,
                    SinoszId = c.a.SinoszId,
                    OrganizationName = c.a.Organization.OrganizationName ?? "",
                    DocumentDate = (DateTime?)c.b.DocumentDate ?? (DateTime?)null,
                    AccountingTypeName = null != c.b.AccountingType ? c.b.AccountingType.AccountingTypeName : "",
                    BirthName = c.a.BirthName,
                    PostCode = c.a.Postcode.Code + " - " + c.a.Postcode.City ?? "",
                    HomeAddress = c.a.HomeAddress,
                    RelationshipName = c.a.Relationship.RelationshipName ?? "",
                    BirthDate = c.a.BirthDate,
                    Qualification = c.a.Qualification,
                    BirthPlace = c.a.BirthPlace,
                    MothersName = c.a.MothersName,
                    Telephone = db.Address.FirstOrDefault(x => x.SinoszUser == c.a && x.AddressType.AddressTypeName == "Otthoni").AddressText,
                    GenusName = c.a.Genus.GenusName ?? "",
                    EducationName = c.a.Education.EducationName ?? "",
                    MaritalStatusName = c.a.MaritalStatus.MaritalStatusName ?? "",
                    EnterDate = c.a.EnterDate,
                    Remark = c.a.Remark,
                    HearingStatusName = c.a.HearingStatus.HearingStatusName ?? "",
                    InjuryTimeText = c.a.InjuryTime.InjuryTimeText ?? "",
                    PensionTypeName = c.a.PensionType.PensionTypeName ?? "",
                    DecreeNumber = c.a.DecreeNumber,
                    NationText = c.a.Nation.NationText ?? "",
                    Sum = (float?)c.b.Sum ?? 0,
                    Email = db.Address.FirstOrDefault(x => x.SinoszUser == c.a && x.AddressType.AddressTypeName == "E-mail").AddressText,
                    //brandnew 
                    Code = c.a.Postcode.Code,
                    City = c.a.Postcode.City,
                    CardStatusName = d.CardStatus.CardStatusName,
                    StatusName = c.a.SinoszUserStatus.StatusName
                });
                //.OrderBy(x => x.StatusName)
                //.ThenByDescending(x => x.EnterDate)
                //.ThenBy(x => x.SinoszUserName);

            //foreach (var item in rs0) { Debug.WriteLine(item); }

            if (null == Output)
            {
                var rs = rs0.AsQueryable().GridPage(grid, out result);

                result.rows = (from r in rs
                               select new JsonRow
                               {
                                   id = r.Id.ToString(),
                                   cell = new string[] 
                               { 
                                r.Id.ToString()
                                ,r.SinoszId
                                ,r.SinoszUserName
                                ,r.BirthDate.ToString()
                                ,r.OrganizationName
                                ,r.City
                                ,r.Qualification
                                ,r.BirthPlace
                                ,r.MothersName
                                ,r.BirthName
                                ,r.Telephone
                                ,r.GenusName
                                ,r.Code
                                ,r.HomeAddress
                                ,r.EducationName
                                ,r.RelationshipName
                                ,r.MaritalStatusName
                                ,r.EnterDate.ToString()
                                ,r.Remark
                                ,r.HearingStatusName
                                ,r.InjuryTimeText
                                ,r.PensionTypeName
                                ,r.DecreeNumber
                                ,r.NationText
                                ,r.DocumentDate.ToString()
                                ,r.Sum.ToString()                                                                                                
                                ,r.Email
                                ,r.CardStatusName
                                ,r.StatusName
                               }
                               }).ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Fizetések lista grid adatforrása 
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
//TODO: Nem eszi meg ezt valamiért [Authorize(Roles = "SysAdmin, SinoszAdmin, SinoszUser")]
        public virtual ActionResult ListAccountingDocumentsForList(GridSettings grid, string Output)
        {
            JQGrid.Helpers.JsonResult result;

            string userId = User.Identity.GetUserId();
            Guid? orgId = null;
            bool isSinoszUser = false;
            var ur = db.ApplicationUserRole.FirstOrDefault(x =>
                                                                x.UserId == userId
                                                                && x.Role.Name == "SinoszUser");
            if (null != ur)
            {
                isSinoszUser = true;
                var kur = ur.KontaktUserRole;
                if (null != kur)
                {
                    var org = kur.Organization;
                    if (null != org)
                    {
                        orgId = org.Id;
                    }
                }
            }           

            var rs0 = db.AccountingDocument
                .Where(x => 
                    !isSinoszUser //SysAdmin vagy SinoszAdmin vagyok, mindent látok
                    || (null != orgId && x.SinoszUser.Organization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem szerinti tagok
                    || (null != x.SinoszUser.Organization.UpperOrganization && x.SinoszUser.Organization.UpperOrganization.Id == orgId) //vagy SinoszUser vagyok és tagszervezetem feletti szervezet tagjai
                    )
                .SelectMany(x => db.AccountingDocument.Where(y => y.Id == x.SinoszUser.LastAccountingDocument).DefaultIfEmpty(), (a, b) => new {
                    a, b, 
                    Telephone = db.Address.FirstOrDefault(x => x.SinoszUser.Id == a.SinoszUser.Id && x.AddressType.AddressTypeName == "Otthoni").AddressText,
                    Email = db.Address.FirstOrDefault(x => x.SinoszUser.Id == a.SinoszUser.Id && x.AddressType.AddressTypeName == "E-mail").AddressText
                });

            if (null == Output)
            {
                var rs = rs0.AsQueryable().GridPage(grid, out result).ToList();

                result.rows = (from r in rs
                               select new JsonRow
                               {
                                   id = r.a.Id.ToString(),
                                   cell = new string[] 
                               { 
                                r.a.Id.ToString()
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.SinoszUserStatus ? r.a.SinoszUser.SinoszUserStatus.StatusName : "" : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.SinoszUserName : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.SinoszId : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.Organization ? r.a.SinoszUser.Organization.OrganizationName : "" : ""
                                ,null != r.b ? r.b.DocumentDate.ToString() : ""
                                ,null != r.b ? null != r.b.AccountingType ? r.b.AccountingType.AccountingTypeName : "" : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.BirthName : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.Postcode ? r.a.SinoszUser.Postcode.Code : "" : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.Postcode ? r.a.SinoszUser.Postcode.City : "" : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.HomeAddress : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.Relationship ? r.a.SinoszUser.Relationship.RelationshipName : "" : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.BirthDate.ToString() : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.Qualification : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.BirthPlace : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.MothersName : ""
                                ,null != r.Telephone ? r.Telephone : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.Genus.GenusName : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.Education ? r.a.SinoszUser.Education.EducationName : "" : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.MaritalStatus ? r.a.SinoszUser.MaritalStatus.MaritalStatusName : "" : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.EnterDate.ToString() : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.Remark : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.HearingStatus ? r.a.SinoszUser.HearingStatus.HearingStatusName : "" : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.InjuryTime ? r.a.SinoszUser.InjuryTime.InjuryTimeText : "" : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.PensionType ? r.a.SinoszUser.PensionType.PensionTypeName : "" : ""
                                ,null != r.a.SinoszUser ? r.a.SinoszUser.DecreeNumber : ""
                                ,null != r.a.SinoszUser ? null != r.a.SinoszUser.Nation ? r.a.SinoszUser.Nation.NationText : "" : ""
                                ,null != r.b ? r.b.Sum.ToString() : "" 
                                ,null != r.Email ? r.Email : ""
                                ,r.a.DocumentDate.ToString()
                                ,null != r.a.AccountingType ? r.a.AccountingType.AccountingTypeName : ""
                                ,r.a.DocumnetNumber
                                ,r.a.Sum.ToString()
                                ,null != r.a.AccountingStatus ? r.a.AccountingStatus.AccountingStatusName : ""
                               }
                               }).ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return new EmptyResult();
            }
        }

#endregion List

#region PrintContract


#endregion PrintContract

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




