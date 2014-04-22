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
                return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", assemblyname.Name, assemblyname.Version);
            }
        }

        public DbSet<SystemParameter> SystemParameter { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<SubMenu> SubMenu { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<CategoriesGlobal> CategoriesGlobal { get; set; }
        public DbSet<CategoriesLocal> CategoriesLocal { get; set; }
        public DbSet<ContactsGlobal> ContactsGlobal { get; set; }
        public DbSet<ContactsLocal> ContactsLocal { get; set; }
        public DbSet<EventsGlobal> EventsGlobal { get; set; }
        public DbSet<EventsLocal> EventsLocal { get; set; }
        public DbSet<ExtrasGlobal> ExtrasGlobal { get; set; }
        public DbSet<ExtrasLocal> ExtrasLocal { get; set; }
        public DbSet<MessagesGlobal> MessagesGlobal { get; set; }
        public DbSet<MessagesLocal> MessagesLocal { get; set; }
        public DbSet<NewsGlobal> NewsGlobal { get; set; }
        public DbSet<NewsLocal> NewsLocal { get; set; }
        public DbSet<NewsStatusesGlobal> NewsStatusesGlobal { get; set; }
        public DbSet<NewsStatusesLocal> NewsStatusesLocal { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<TagsGlobal> TagsGlobal { get; set; }
        public DbSet<TagsLocal> TagsLocal { get; set; }

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

