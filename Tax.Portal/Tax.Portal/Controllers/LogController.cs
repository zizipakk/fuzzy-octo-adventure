using JQGrid.Helpers;
using Tax.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text;

namespace Tax.Portal.Controllers
{
    public partial class LogController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = new ApplicationDbContext();

        //A unit teszthez előkészítve
        public LogController() : this(new ApplicationDbContext()) { }
        public LogController(ApplicationDbContext db) { this.db = db; }

        //
        // GET: /Log/
        [Authorize(Roles="SysAdmin")]
        public virtual ActionResult Index()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Index (GET)"))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        /// <summary>
        /// A napló adatforrása
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public virtual System.Web.Mvc.JsonResult ListRecordChanges(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.LogRecordChanges
                        .Include(x => x.DbTable)
                        .Include(x => x.User)
                        .Include(x => x.User.KontaktUser)
                        .Select(x => x)
                        .AsEnumerable();

            var rs = rs0.AsQueryable()
                        .GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                r.Id.ToString(),
                                string.Format("{0} {1}", r.Date.ToShortDateString(), r.Date.ToLongTimeString()),
                                //r.RecordId,
                                null!=r.User
                                        ?string.Format("{0} {1} ({2})", 
                                                null!=r.User.KontaktUser
                                                    ?r.User.KontaktUser.FirstName
                                                    :string.Empty, 
                                                null!=r.User.KontaktUser
                                                    ?r.User.KontaktUser.LastName
                                                    :string.Empty, 
                                                r.User.UserName)
                                        :string.Empty,
                                null!=r.DbTable
                                        ?r.DbTable.Name.ToString()
                                        :string.Empty,
                                r.ChangeType.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// A napló adatforrása
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public virtual System.Web.Mvc.JsonResult ListColumnChanges(Guid Id, GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            var rs0 = db.LogRecordChanges
                        .Find(Id)
                        .LogColumnChanges
                        .Select(x => 
                                    new 
                                    {
                                        x.Id,
                                        x.Name,
                                        x.OldValue,
                                        x.NewValue,
                                        IsChanged = null!=x.NewValue
                                                        ?!x.NewValue.Equals(x.OldValue,StringComparison.CurrentCulture)
                                                        :null!=x.OldValue?
                                                            !x.OldValue.Equals(x.NewValue,StringComparison.CurrentCulture)
                                                            :false
                                    } )
                        .AsEnumerable();

            var rs = rs0.AsQueryable()
                        .GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               { 
                                r.Id.ToString(),
                                r.Name,
                                r.OldValue,
                                r.NewValue,
                                r.IsChanged.ToString()
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string SelectListUsers()
        {
            var sb = new StringBuilder("<select><option value=''> Minden felhasználó</option>");
            //ToList, Hogy egyszerre betöltsük a 
            //lenyílóhoz szükséges adatokat
            foreach (var user in db.Users
                                   .Include(x => x.KontaktUser)
                                   .OrderBy(x => x.KontaktUser.FirstName)
                                   .OrderBy(x => x.KontaktUser.LastName)
                                   .OrderBy(x => x.UserName)
                                   .ToList())
            {
                sb.Append(
                    string.Format("<option value='{0}'>{1} {2} ({3})</option>",
                            user.Id.ToString(),
                            null != user.KontaktUser
                                ? user.KontaktUser.FirstName
                                : string.Empty,
                            null != user.KontaktUser
                                ? user.KontaktUser.LastName
                                : string.Empty,
                            user.UserName));
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        public string SelectListDbTables()
        {
            var sb = new StringBuilder("<select><option value=''> Minden tábla</option>");
            //ToList, Hogy egyszerre betöltsük a 
            //lenyílóhoz szükséges adatokat
            foreach (var table in db.DbTables
                                    .OrderBy(x=>x.Name)
                                    .ToList())
            {
                sb.Append(string.Format("<option value='{0}'>{1}</option>", table.Id.ToString(), table.Name));
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        public string SelectListChangeTypes()
        {
            var sb = new StringBuilder("<select><option value=''> Minden típus</option>");
            //ToList, Hogy egyszerre betöltsük a 
            //lenyílóhoz szükséges adatokat
            foreach (EntityState state in ((EntityState[])Enum.GetValues(typeof(EntityState)))
                                            .ToList()
                                            .OrderBy(x=>x))
            {
                sb.Append(string.Format("<option value='{0}'>{1}</option>", (int)state, state.ToString()));
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        [Authorize(Roles="SysAdmin")]
        public virtual ActionResult BugSendTest()
        {
            throw new ApplicationException("Tax.Portal teszt error");
        }

        protected override void Dispose(bool disposing)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Dispose"))
            {
                log.Info("begin");
                log.Info(string.Format("disposing: {0}", disposing));
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
                log.Info("end");
            }
        }
	}
}