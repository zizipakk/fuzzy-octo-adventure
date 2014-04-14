using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using log4net;
using Tax.Data.Models;
using Tax.Portal.Models;
using System.Data.Entity;
using Tax.Portal.Mailers;
using Tax.Portal.Helpers;

namespace Tax.Portal.Controllers
{
    [Authorize(Roles = "Jeltolmácsok, Diszpécser")]
    public partial class ScheduleController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Diszpécser")]
        public virtual ActionResult Edit()
        {
            return View();
        }

        [Authorize(Roles = "Jeltolmácsok")]
        public virtual ActionResult Display()
        {
            return View();
        }

        [Authorize(Roles = "Diszpécser")]
        public virtual ActionResult ScheduleItems()
        {
            var result = db.ScheduleItem.Select(s => new ScheduleItemViewModel 
            {
                Id = s.Id,
                Start = s.Start,
                End = s.End,
                Activity = (int)s.Activity,
                Description = s.Description,
                UserId = s.User.Id,
                FirstName = s.User.KontaktUser.FirstName,
                LastName = s.User.KontaktUser.LastName
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Jeltolmácsok")]
        public virtual ActionResult ScheduleItemsPrivate()
        {
            var now = DateTime.Now;
            var schedules = db.ScheduleItem.Where(s => s.User.UserName == User.Identity.Name)
                .Select(s => new ScheduleItemViewModel
            {
                Id = s.Id,
                Start = s.Start,
                End = s.End,
                Activity = (int)s.Activity,
                UserId = s.User.Id,
                Description = s.Description,
                FirstName = s.User.KontaktUser.FirstName,
                LastName = s.User.KontaktUser.LastName
            });

            var firstClosedPeriodStart = now.AddDays(-8);
            var closedPeriods = db.ClosedSchedules.Where(p => p.SchedulePeriodStart >= firstClosedPeriodStart)
                .OrderBy(s => s.SchedulePeriodStart).ToList();

            var result = new List<ScheduleItemViewModel>();
            foreach (var s in schedules)
            {
                if (s.End < now || s.Activity > 1 )    // all from the past and the privates
                {
                    result.Add(s);
                }
                else                // and the closed from the future
                {
                    foreach(var p in closedPeriods) {
                        if (p.SchedulePeriodStart <= s.Start && s.End <= p.SchedulePeriodEnd)
                        {
                            result.Add(s);
                        }
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult SaveSchedule(ScheduleItemViewModel model)
        {
            var user = db.Users.Find(model.UserId);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (ConflictsWithUsersOwnSchedule(model))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }

            var scheduleItem = db.ScheduleItem.Find(model.Id);
            if (scheduleItem == null)
            {
                db.ScheduleItem.Add(new ScheduleItem
                    {
                        Id = model.Id,
                        User = user,
                        Start = model.Start,
                        End = model.End,
                        Description = model.Description,
                        Activity = (ScheduledActitvity)model.Activity
                    });
            }
            else
            {
                scheduleItem.User = user;
                scheduleItem.Start = model.Start;
                scheduleItem.End = model.End;
                scheduleItem.Description = model.Description;
            }

            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private bool ConflictsWithUsersOwnSchedule(ScheduleItemViewModel model)
        {
            return db.ScheduleItem.GetSchedulesInPeriod(model.Start, model.End, model.UserId)
                        .Where(s => s.Id != model.Id)
                        .Any();
        }

        public virtual ActionResult DeleteSchedule(Guid id)
        {
            var scheduleItem = db.ScheduleItem.Find(id);
            if (scheduleItem == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            db.ScheduleItem.Remove(scheduleItem);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Jeltolmácsok")]
        public virtual ActionResult IntereterProfile()
        {
            var model = getCurrentUserInterpreterViewModel();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Interpreters()
        {
            var model = getInterpreterViewModels().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult CloseSchedule(DateTime start, DateTime end)
        {
            if(db.ClosedSchedules.IsPeriodInvalid(start, end))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var closedSchedule = db.ClosedSchedules.Add(new ClosedSchedules
            {
                ClosureTimestamp = DateTime.Now,
                SchedulePeriodStart = start,
                SchedulePeriodEnd = end
            });

            SendScheduleClosedEmail(closedSchedule);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public virtual ActionResult OpenSchedule(DateTime start, DateTime end)
        {
            var schedule = db.ClosedSchedules.Where(s => 
                s.SchedulePeriodStart == start && s.SchedulePeriodEnd == end)
                .FirstOrDefault();

            if(schedule == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            db.ClosedSchedules.Remove(schedule);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public virtual ActionResult GenerateSchedule(GenerateScheduleRequest generationParams)
        {
            var interpreters = getInterpreterViewModels().ToList();
            var privateSchedulesOnWeek = db.ScheduleItem.GetSchedulesInPeriod(generationParams.Start, generationParams.End)
                                                .Where(s => s.Activity == ScheduledActitvity.InterpreterUnavailable ||
                                                            s.Activity == ScheduledActitvity.InterpreterExternalDuty)
                                                .Include(s => s.User)
                                                .ToList().AsQueryable();

            string ErrorReason = String.Empty;
            var generator = new ScheduleGenerator(generationParams, interpreters, privateSchedulesOnWeek);
            if(generator.ValidateInitialParameters(out ErrorReason))
            {
                try
                {
                    generator.GenerateSchedule();

                    generator.RemoveOldSchedules(db);
                    generator.InsertScheduleToDB(db);

                    db.SaveChanges();
                }
                catch(ScheduleGenerationException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ezekkel a praméterekkel nem sikerült a beosztás generálás");
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ErrorReason);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public virtual ActionResult ClosedSchedulePeriods()
        {
            var result = db.ClosedSchedules.OrderBy(s => s.SchedulePeriodStart).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) 
            {
                db.Dispose(); 
            }
            base.Dispose(disposing);
        }

        private void SendScheduleClosedEmail(ClosedSchedules closedSchedule)
        {
            var interpreters = db.ScheduleItem
                .Where(s => closedSchedule.SchedulePeriodStart <= s.Start && s.End <= closedSchedule.SchedulePeriodEnd)
                .Select(s => s.User)
                .Include(s => s.KontaktUser)
                .Distinct()
                .ToList();

            var addresses = interpreters.Select(i => new Addressee
            {
                Email = i.Email,
                FullName = i.KontaktUser.FirstName + i.KontaktUser.LastName,
                Position = Addressee.Positions.Bcc
            });

            var message = new Message<ScheduleClosedEmailData>
            {
                Subject = "Heti beosztás elkészült",
                Data = new ScheduleClosedEmailData
                    {
                        WeekStart = closedSchedule.SchedulePeriodStart,
                        WeekEnd = closedSchedule.SchedulePeriodEnd
                    }
            };

            MessageHelper<ScheduleClosedEmailData>.SendMessageToQueue(message, addresses, Url);
        }

        private IQueryable<InterpreterViewModel> getInterpreterViewModels()
        {
            return db.ApplicationUserRole.Where(x =>
                                x.Role.Name == "Jeltolmácsok" &&
                                x.KontaktUserRole != null &&
                                x.KontaktUserRole.InterpreterCenter != null)
                            .Join(db.Users.Where(u => u.KontaktUser.FirstName != null), // ez csak azért kell mert tele van a db szeméttel
                                r => r.UserId, u => u.Id, (r, u) => new InterpreterViewModel
                                {
                                    Id = r.UserId,
                                    FirstName = u.KontaktUser.FirstName,
                                    LastName = u.KontaktUser.LastName,
                                    AreaName = r.KontaktUserRole.InterpreterCenter.Postcode.City
                                });
        }

        private InterpreterViewModel getCurrentUserInterpreterViewModel()
        {
            return db.ApplicationUserRole.Where(x =>
                                x.User.UserName == User.Identity.Name &&
                                x.Role.Name == "Jeltolmácsok" &&
                                x.KontaktUserRole != null &&
                                x.KontaktUserRole.InterpreterCenter != null)
                            .Join(db.Users.Where(u => u.KontaktUser.FirstName != null), // ez csak azért kell mert tele van a db szeméttel
                                r => r.UserId, u => u.Id, (r, u) => new InterpreterViewModel
                                {
                                    Id = r.UserId,
                                    FirstName = u.KontaktUser.FirstName,
                                    LastName = u.KontaktUser.LastName,
                                    AreaName = r.KontaktUserRole.InterpreterCenter.Postcode.City
                                })
                            .Single();
        }
    }

    public static class ClosedSchedulesExtension
    {
        public static bool IsPeriodInvalid(this DbSet<ClosedSchedules> dbSchedules, DateTime start, DateTime end)
        {
            return dbSchedules.Any(s => (start < s.SchedulePeriodEnd && s.SchedulePeriodStart < end));
        }
    }

    public static class ScheduleItemExtension
    {
        public static IQueryable<ScheduleItem> GetSchedulesInPeriod(this IQueryable<ScheduleItem> dbSchedules, DateTime start, DateTime end, string userId = null)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return dbSchedules.Where(s => (start < s.End && s.Start < end) ||
                                          (start < s.End && s.End <= end));
            }
            else
            {
                return dbSchedules.Where(s => ((start <= s.Start && s.Start < end) || (start < s.End && s.End <= end)) &&
                                            s.User.Id == userId);
            }
        }
    }

    public static class ListExtension
    {
        static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;  
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void MoveToEnd<T>(this IList<T> list, int index)
        {
            var lastIndex = list.Count - 1;

            T item = list[index];
            list.RemoveAt(index);
            list.Insert(lastIndex, item);
        }
    }

    public class ScheduleGenerator
    {
        readonly int daysInWeek = 7;
        readonly int workingHours = 8;
        readonly int firstWorkingHour = 8;
        int parallelism;

        GenerateScheduleRequest generationParams;
        IList<InterpreterViewModel> interpereters;

        public static InterpreterViewModel[, ,] Schedule { get; set; }
        public static HashSet<string>[,] PrivateScheduleMatrix { get; set; }

        public ScheduleGenerator(GenerateScheduleRequest generationParams, IList<InterpreterViewModel> interpereters, IQueryable<ScheduleItem> privateSchedules)
        {
            parallelism = generationParams.SelectedNumber;
            this.generationParams = generationParams;
            this.interpereters = interpereters;

            PrivateScheduleMatrix = new HashSet<string>[daysInWeek, workingHours];
            for (int d = 0; d < daysInWeek; d++)
            {
                for (int h = 0; h < workingHours; h++)
                {
                    var periodStart = generationParams.Start.AddDays(d).AddHours(firstWorkingHour + h);
                    var periodEnd = periodStart.AddHours(1);

                    PrivateScheduleMatrix[d, h] = new HashSet<string>();
                    var usersWithPrivateSchedule = privateSchedules.GetSchedulesInPeriod(periodStart, periodEnd).Select(s => s.User.Id);
                    PrivateScheduleMatrix[d, h].UnionWith(usersWithPrivateSchedule);
                }
            }
        }

        public bool ValidateInitialParameters(out string errorReason)
        {
            if (interpereters.Count() < parallelism)
            {
                errorReason = "Nincs elég tomlács a beosztáshoz";
                return false;
            }

            if (interpereters.GroupBy(i => i.AreaName).Count() < parallelism)
            {
                errorReason = "Nincs elég különböző tolmácsközpontban dolgozó tolmács a beosztáshoz";
                return false;
            }

            errorReason = String.Empty;
            return true;
        }

        public void GenerateSchedule()
        {
            interpereters.Shuffle();

            Schedule = new InterpreterViewModel[daysInWeek, workingHours, parallelism];
            for(int d = 0; d < daysInWeek; d++)
            {
                if(generationParams.SelectedDays[d])
                {
                    for (int h = 0; h < workingHours; h++)
                    {
                        for (int p = 0; p < parallelism; p++)
                        {
                            for (int i = 0; i < interpereters.Count; i++)
                            {
                                Schedule[d,h,p] = interpereters[i];
                                if (IsPeriodValid(d, h))
                                {
                                    interpereters.MoveToEnd(i);
                                    break;
                                }
                                else if(i == interpereters.Count - 1)
                                {
                                    throw new ScheduleGenerationException();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RemoveOldSchedules(ApplicationDbContext db)
        {
            var oldParams = db.ScheduleItem.Where(s => generationParams.Start <= s.Start && s.End <= generationParams.End && s.Activity == ScheduledActitvity.InterpreterWork).ToList();
            db.ScheduleItem.RemoveRange(oldParams);
        }

        public void InsertScheduleToDB(ApplicationDbContext db)
        {
            var interpreterIds = interpereters.Select(i => i.Id);
            var interpreterUsers = db.Users.Where(u => interpreterIds.Contains(u.Id)).ToDictionary(u => u.Id);

            for (int d = 0; d < daysInWeek; d++)
            {
                for (int h = 0; h < workingHours; h++)
                {
                    var periodStart = generationParams.Start.AddDays(d).AddHours(firstWorkingHour + h);
                    var periodEnd = periodStart.AddHours(1);
                    for (int p = 0; p < parallelism; p++)
                    {
                        if(Schedule[d,h,p] != null)
                        {
                            db.ScheduleItem.Add(new ScheduleItem
                            {
                                Activity = ScheduledActitvity.InterpreterWork,
                                User = interpreterUsers[ Schedule[d,h,p].Id ],
                                Start = periodStart,
                                End = periodEnd
                            });
                        }
                    }
                }
            }
        }

        private bool IsPeriodValid(int d, int h)
        {
            var filledSlots = 0;
            var areas = new HashSet<string>();
            for (int p = 0; p < parallelism; p++)
            {
                if (Schedule[d, h, p] != null)
                {
                    filledSlots++;
                    areas.Add(Schedule[d, h, p].AreaName);      // prepare area confilict check

                    if (PrivateScheduleMatrix[d, h].Contains(Schedule[d, h, p].Id))     // check personal schedule conflict
                    {
                        return false;
                    }
                }
            }
            return areas.Count == filledSlots;          // check area conflict
        }
    }

    public class ScheduleGenerationException : Exception
    {
    }

}