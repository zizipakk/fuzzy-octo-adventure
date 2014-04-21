namespace Tax.Data.Migrations
{
    using Tax.Data.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false }; 
            ApplicationUser user;
            IdentityResult result;

            if (null == UserManager.FindByName("admin"))
            {
                var role = context.Roles.Single(x => x.Name == "SysAdmin");

                user = new ApplicationUser()
                {
                    UserName = "admin",
                    Name = "Admin Admin",
                    Email = "admin@tax.hu",
                };
                result = UserManager.Create(user, "Admin123");

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült az admin felhasználó rögzítése");
                }

                var iur = new IdentityUserRole()
                {
                    User = user,
                    Role = role
                };

                context.Entry(iur).State = EntityState.Added;
            }

            context.SaveChanges();            
        }
    
    }
}
