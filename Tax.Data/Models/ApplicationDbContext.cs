using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;

namespace Tax.Data.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static ApplicationDbContext()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ApplicationDbContext (static)"))
            {
                log.Info("begin");
                //Database.SetInitializer<ApplicationDbContext>(new ValidateDatabase<ApplicationDbContext>());
                Database.SetInitializer<ApplicationDbContext>(null);
                log.Info("end");
            }
        }

        public ApplicationDbContext()
            : base("DefaultConnection"){}

        public ApplicationDbContext(string nameOrConnectionString)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ApplicationDbContext w parameter"))
            {
                log.Info("begin");
                log.Info(string.Format(CultureInfo.CurrentCulture, "nameOrConnectionString: {0}", nameOrConnectionString));
                log.Info("end");
            }
        }

        public static string version
        {
            get
            { 
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyname = assembly.GetName();
                //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                //string fileversion = fvi.FileVersion;
                return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", assemblyname.Name, assemblyname.Version);
            }
        }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<InterpreterCenter> InterpreterCenter { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Postcode> Postcode { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<SubMenu> SubMenu { get; set; }
        public DbSet<KontaktRole> KontaktRole { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<KontaktUserRole> KontaktUserRole { get; set; }
        public DbSet<PBXExtensionData> PBXExtensionData { get; set; }
        public DbSet<PhoneNumber> PhoneNumber { get; set; }
        public DbSet<SinoszUser> SinoszUser { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<HearingStatus> HearingStatus { get; set; }
        public DbSet<InjuryTime> InjuryTime { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Genus> Genus { get; set; }
        public DbSet<Nation> Nation { get; set; }
        public DbSet<Relationship> Relationship { get; set; }
        public DbSet<PensionType> PensionType { get; set; }
        public DbSet<SinoszUserStatus> SinoszUserStatus { get; set; }
        public DbSet<StatusToStatus> StatusToStatus { get; set; }
        public DbSet<AddressType> AddressType { get; set; }
        public DbSet<NewsType> NewsType { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<AccountingType> AccountingType { get; set; }
        public DbSet<AccountingStatus> AccountingStatus { get; set; }
        public DbSet<CardStatus> CardStatus { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<SinoszLog> SinoszLog { get; set; }
        public DbSet<AccountingDocument> AccountingDocument { get; set; }
        public DbSet<AttachedFile> AttachedFile { get; set; }
        public DbSet<SystemParameter> SystemParameter { get; set; }
        public DbSet<FileType> FileType { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<KontaktUser> KontaktUser { get; set; }
        public DbSet<PBXLog> PBXLog { get; set; }
        public DbSet<PBXCall> PBXCall { get; set; }
        public DbSet<PBXSession> PBXSession { get; set; }
        public DbSet<PBXTransfer> PBXTransfer { get; set; }
        public DbSet<DeviceUsage> DeviceUsage { get; set; }
        public DbSet<Devices> Devices { get; set; }
        public DbSet<DeviceTypes> DeviceTypes { get; set; }
        public DbSet<DeviceStatus> DeviceStatus { get; set; }
        public DbSet<DeviceLog> DeviceLog { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<PreregistrationData> PreregistrationData { get; set; }
        public DbSet<SocialPosition> SocialPosition { get; set; }
        public DbSet<SurveyAnswerCode> SurveyAnswerCodes { get; set; }
        public DbSet<SurveyQuestionCode> SurveyQuestionCodes { get; set; }
        public DbSet<DbTable> DbTables { get; set; }
        public DbSet<LogRecordChange> LogRecordChanges { get; set; }
        public DbSet<BugPost> BugPosts { get; set; }
        public DbSet<ReservationTime> ReservationTime { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<ScheduleItem> ScheduleItem { get; set; }
        public DbSet<ClosedSchedules> ClosedSchedules { get; set; }
        public DbSet<PBXRegistration> PBXRegistration { get; set; }
        public DbSet<PBXCallQueue> PBXCallQueue { get; set; }
        public DbSet<PBXInterpreterTerminateCallQueue> PBXInterpreterTerminateCallQueue { get; set; }
        public DbSet<PBXInterpreterStatus> PBXInterpreterStatus { get; set; }
        public DbSet<AccountPeriod> AccountPeriod { get; set; }
        public DbSet<AccountPeriodStatus> AccountPeriodStatus { get; set; }
        public DbSet<Price> Price { get; set; }

        public override int SaveChanges()
        {
            this.SaveLog();
            return base.SaveChanges();
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            this.SaveLog();
            return base.SaveChangesAsync();
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            this.SaveLog();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SaveLog()
        {
            //megállapítjuk a felhasználót
            string httpuser = null;
            ApplicationUser user = null;
            string userid = null;

            //Először megpróbálom a cookie-ből visszanyerni a felhasználót
            if (null != HttpContext.Current
                && null != HttpContext.Current.User
               )
            {
                var principal = HttpContext.Current.User as ClaimsPrincipal;
                if (null!=principal)
                {
                    var claim = principal.Claims
                                         .SingleOrDefault(x => 
                                             x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                    if (null != claim)
                    {
                        userid = claim.Value;
                    }
                }
            }

            if (null==userid
                && null != HttpContext.Current
                && null != HttpContext.Current.User
                && null != HttpContext.Current.User.Identity
               )
            { //ha nincs meg a cookie-ből, akkor megnézzük az adatbázisban
                httpuser = HttpContext.Current.User.Identity.Name;
                user = this.Users.SingleOrDefault(x => x.UserName == httpuser);
                if (null != user)
                {
                    userid = user.Id;
                }
            }
        
            foreach (var entry in this.ChangeTracker
                                      .Entries()
                                      .Where(
                                      //ezeket nem naplózzuk
                                      //Tokens, Logs, Menu, SubMenu, ChatMessages, SurveyAnswerCodes, SurveyQuestionCodes, SinoszLog, DbTables, LogRecordChanges
                                      //BugPosts, ScheduleItem, ClosedSchedules, PBXRegistration, PBXCallQueue, PBXLog, PBXCall, PBXSession, PBXTransfer , File

                                      //ezeket a táblákat naplózzuk
                                        x=>(x.Entity is InterpreterCenter
                                            || x.Entity is Area
                                            || x.Entity is Postcode 
                                            || x.Entity is KontaktRole 
                                            || x.Entity is ApplicationUserRole 
                                            || x.Entity is KontaktUserRole 
                                            || x.Entity is PBXExtensionData 
                                            || x.Entity is PhoneNumber 
                                            || x.Entity is SinoszUser 
                                            || x.Entity is Organization 
                                            || x.Entity is Position 
                                            || x.Entity is MaritalStatus 
                                            || x.Entity is HearingStatus 
                                            || x.Entity is InjuryTime 
                                            || x.Entity is Education 
                                            || x.Entity is Genus 
                                            || x.Entity is Nation 
                                            || x.Entity is Relationship 
                                            || x.Entity is PensionType 
                                            || x.Entity is SinoszUserStatus 
                                            || x.Entity is StatusToStatus 
                                            || x.Entity is AddressType 
                                            || x.Entity is NewsType 
                                            || x.Entity is News 
                                            || x.Entity is AccountingType 
                                            || x.Entity is AccountingStatus 
                                            || x.Entity is CardStatus 
                                            || x.Entity is Card 
                                            || x.Entity is Address 
                                            || x.Entity is AccountingDocument 
                                            || x.Entity is AttachedFile 
                                            || x.Entity is SystemParameter 
                                            || x.Entity is KontaktUser 
                                            || x.Entity is DeviceUsage 
                                            || x.Entity is Devices 
                                            || x.Entity is DeviceTypes 
                                            || x.Entity is DeviceStatus 
                                            || x.Entity is DeviceLog 
                                            || x.Entity is Comment
                                            || x.Entity is Survey 
                                            || x.Entity is PreregistrationData 
                                            || x.Entity is SocialPosition 
                                            || x.Entity is ReservationTime 
                                            || x.Entity is Reservation
                                            || x.Entity is FileType)
                                        && (x.State == EntityState.Added
                                        || x.State == EntityState.Deleted
                                        || x.State == EntityState.Modified)
                                        )
                    )
            {
                if (null != entry.Entity)
                {
                    var change = GetRecordChanges(entry);
                    if (null!=userid)
                    { //ha van felhasználónk, akkor kitöltjük
                        change.UserId = userid;
                    }
                    this.LogRecordChanges.Add(change);
                }
            }
            
        }

        private LogRecordChange GetRecordChanges(DbEntityEntry entry)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GetRecordChanges"))
            {
                log.Info("begin");

                //Megállapítjuk a tábla nevét
                //Először megnézzük, hogy van-e Table() dekoráció megadva
                var tableAttr = entry.Entity
                                     .GetType()
                                     .GetCustomAttributes(typeof(TableAttribute), false)
                                     .SingleOrDefault() as TableAttribute;


                var entityType = entry.Entity.GetType();
                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                { //Ha proxy objektum, akkor a név miatt az eredetit használjuk
                    entityType = entityType.BaseType;
                }

                //ha van Table attributum, akkor azt használjuk, ha nincs, akkor az objektum nevét
                var TableName = null != tableAttr
                                ? string.Format("{0}.{1}", string.IsNullOrWhiteSpace(tableAttr.Schema) ? "dbo" : tableAttr.Schema, tableAttr.Name)
                                : string.Format("dbo.{0}", entityType.Name);

                //Először keressük a local cache-ben
                var dbtable = this.DbTables.Local.SingleOrDefault(x => x.Name == TableName);

                if (null==dbtable)
                { //ha ott nincs, akkor az adatbázisban
                    dbtable = this.DbTables.SingleOrDefault(x => x.Name == TableName);
                }

                if (null == dbtable)
                {//és ha nincs ilyen tábla, rögzítjük
                    dbtable = new DbTable()
                    {
                        Name = TableName
                    };
                    this.Entry(dbtable).State = EntityState.Added;
                    this.DbTables.Add(dbtable);
                }

                //TODO: minden táblának az Id-je Id.
                var keyName = "Id";
                string RecordId = null;
                switch (entry.State)
                { //OriginalValues cannot be used for entities in the Added state.
                    case EntityState.Added:
                        if (null != entry.CurrentValues && entry.CurrentValues.PropertyNames.Any(x => x == keyName))
                        { // ha van Id mező a currenvalues-ek között
                            RecordId = string.Format("{0}", entry.CurrentValues.GetValue<object>(keyName));
                        }
                        break;
                    case EntityState.Deleted:
                        if (null!=entry.OriginalValues && entry.OriginalValues.PropertyNames.Any(x => x == keyName))
                        {//ha van Id mező az originalvaluesek között, akkor az az azonosító
                            RecordId = string.Format("{0}", entry.OriginalValues.GetValue<object>(keyName));
                        }
                        break;
                    case EntityState.Modified:
                        if (null != entry.CurrentValues && entry.CurrentValues.PropertyNames.Any(x => x == keyName))
                        { // ha van Id mező a currenvalues-ek között
                            RecordId = string.Format("{0}", entry.CurrentValues.GetValue<object>(keyName));
                        }
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    default:
                        throw new ArgumentException(string.Format("Ezt az állapotot nem naplózzuk: {0}", entry.State.ToString()));
                }

                var retval = new LogRecordChange()
                {
                    ChangeType = entry.State,
                    Date = DateTime.Now,
                    DbTable = dbtable,
                    LogColumnChanges = GetColumnChanges(entry),
                    RecordId = RecordId,
                    UserId = null,
                };

                //log.Info(string.Format("ez megy a naplóba: {0}", JsonConvert.SerializeObject(retval)));
                log.Info("end");

                return retval;
            }
        }

        private ICollection<LogColumnChange> GetColumnChanges(DbEntityEntry entry)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("GetColumnChanges"))
            {
                log.Info("begin");
                var retval = new HashSet<LogColumnChange>();

                switch (entry.State)
                {
                    case EntityState.Added:
                        //Az új értékeket rögzítjük
                        for (int i = 0; i < entry.CurrentValues.PropertyNames.Count(); i++)
                        {
                            var Name = entry.CurrentValues.PropertyNames.Skip(i).Take(1).Single();
                            retval.Add(new LogColumnChange()
                                {
                                    Name = Name,
                                    OldValue = null,
                                    NewValue = string.Format("{0}", entry.CurrentValues.GetValue<object>(Name))
                                });
                        }
                        break;
                    case EntityState.Deleted:
                        //Az utolsó értékeket rögzítjük
                        for (int i = 0; i < entry.OriginalValues.PropertyNames.Count(); i++)
                        {
                            var Name = entry.OriginalValues.PropertyNames.Skip(i).Take(1).Single();
                            retval.Add(new LogColumnChange()
                            {
                                Name = Name,
                                OldValue = string.Format("{0}", entry.OriginalValues.GetValue<object>(Name)),
                                NewValue = null
                            });
                        }
                        break;
                    case EntityState.Modified:
                        //valamennyi értéket rögzítünk
                        for (int i = 0; i < entry.CurrentValues.PropertyNames.Count(); i++)
                        {
                            var Name = entry.CurrentValues.PropertyNames.Skip(i).Take(1).Single();
                            retval.Add(new LogColumnChange()
                            {
                                Name = Name,
                                OldValue = string.Format("{0}", entry.OriginalValues.GetValue<object>(Name)),
                                NewValue = string.Format("{0}", entry.CurrentValues.GetValue<object>(Name))
                            });
                        }
                        break;
                    case EntityState.Detached:
                        //Ezt nem naplózzuk, ide nem is kerülhetünk
                        throw new ArgumentException(string.Format("Ezt az állapotot nem naplózzuk: {0}", entry.State.ToString()));
                    case EntityState.Unchanged:
                        //Ezt nem naplózzuk, ide nem is kerülhetünk
                        throw new ArgumentException(string.Format("Ezt az állapotot nem naplózzuk: {0}", entry.State.ToString()));
                    default:
                        throw new ArgumentException(string.Format("Ezt az állapotot nem naplózzuk: {0}", entry.State.ToString()));
                }

                //log.Info(string.Format("ez megy a naplóba: {0}", JsonConvert.SerializeObject(retval)));
                log.Info("end");
                return retval;
            }
        }

    }

    /// <summary>
    /// Ez az inicializátor biztosítja, hogy az alkalmazás csak létező és
    /// pontosan megfelelő verziószámú adatbázissal indul csak el.
    /// http://coding.abel.nu/2012/03/prevent-ef-migrations-from-creating-or-changing-the-database/
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class ValidateDatabase<TContext> : IDatabaseInitializer<TContext>
      where TContext : DbContext
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void InitializeDatabase(TContext context)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ValidateDatabase"))
            {
                log.Info("begin");
                if (!context.Database.Exists())
                {
                    var cee = new ConfigurationErrorsException(
                      "Database does not exist");
                    log.Error("end with error", cee);
                    throw cee;
                }
                else
                {
                    if (!context.Database.CompatibleWithModel(true))
                    {

                        var cfg = new Migrations.Configuration();
                        var migrator = new DbMigrator(cfg);

                        ///Érdekes:
                        ///utolsó az adatbázisban
                        var lastInDB = migrator.GetDatabaseMigrations().LastOrDefault();
                        ///utolsó az alkalmazásban
                        var lastInApp = migrator.GetLocalMigrations().LastOrDefault();
                        ///amit még be kéne játszani
                        var pendingMigrations = string.Join(",", migrator.GetPendingMigrations().Select(x => x).ToArray());

                        var ioe = new InvalidOperationException(
                          string.Format("The database ({0}) is not compatible with the entity model ({1}). Missing migrations: {2}",
                            lastInDB, lastInApp, pendingMigrations));
                        log.Error("end with error", ioe);
                        throw ioe;
                    }
                    else
                    {
                        log.Info("end");
                    }
                }
            }
        }
    }
}

