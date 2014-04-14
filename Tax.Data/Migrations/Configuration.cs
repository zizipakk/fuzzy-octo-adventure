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
            KontaktUser kontaktuser;

            var role = context.Roles.Single(x => x.Name == "PBXUser");

            /*if (null == UserManager.FindByName("teszt1"))
            {
                kontaktuser = context.KontaktUser.Create();

                user = new ApplicationUser()
                {
                    UserName = "teszt1",
                    Email = "kontaktteszt1@egroup.hu",
                    isEmailValidated = true,
                    Password = "123456",
                    isSynced = false,
                    KontaktUser = kontaktuser
                };
                result = UserManager.Create(user, user.Password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült a teszt1 felhasználó rögzítése");
                }

                var aur = new ApplicationUserRole()
                {
                    User = user,
                    Role = role
                };

                context.Entry(aur).State = EntityState.Added;

                var phone = (from pn in context.PhoneNumber
                             from px in context.PBXExtensionData.Where(x => x.PhoneNumber.Id == pn.Id && x.isDroped == false).DefaultIfEmpty()
                             where (null == px.Id //nincs ilyen aktív szám
                                   && aur.Role.Name != "PBXUser" //nem pbxuser
                                   && "" == pn.ExternalPhoneNumber) //nem kell külsõ szám, ha "belsõ" szereplõk vagyunk
                                   ||
                                   (null == px.Id //nincs ilyen aktív szám
                                   && aur.Role.Name == "PBXUser" //pbxuser
                                   && "" != pn.ExternalPhoneNumber) //nincs ilyen külsõ szám
                             select pn)
                            .OrderBy(x => x.InnerPhoneNumber).First();

                aur.KontaktUserRole = new KontaktUserRole()
                {
                    PBXExtensionData = new PBXExtensionData()
                    {
                        ApplicationUser = user,
                        PhoneNumber = phone,
                        //ExtensionID = phone.InnerPhoneNumber + "@" + user.UserName,
                        //Password = user.Password,
                        isDroped = false,
                        isSynced = false,
                        StartTime = DateTime.Now,
                        EndTime = DateTime.MaxValue
                    }
                };

            }

            if (null == UserManager.FindByName("teszt2"))
            {
                kontaktuser = context.KontaktUser.Create();

                user = new ApplicationUser()
                {
                    UserName = "teszt2",
                    Email = "kontaktteszt1@egroup.hu",
                    isEmailValidated = true,
                    Password = "123456",
                    isSynced = false,
                    KontaktUser = kontaktuser
                };
                result = UserManager.Create(user, user.Password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült a teszt2 felhasználó rögzítése");
                }

                var aur = new ApplicationUserRole()
                {
                    User = user,
                    Role = role
                };

                context.Entry(aur).State = EntityState.Added;

                var phone = (from pn in context.PhoneNumber
                             from px in context.PBXExtensionData.Where(x => x.PhoneNumber.Id == pn.Id && x.isDroped == false).DefaultIfEmpty()
                             where (null == px.Id //nincs ilyen aktív szám
                                   && aur.Role.Name != "PBXUser" //nem pbxuser
                                   && "" == pn.ExternalPhoneNumber) //nem kell külsõ szám, ha "belsõ" szereplõk vagyunk
                                   ||
                                   (null == px.Id //nincs ilyen aktív szám
                                   && aur.Role.Name == "PBXUser" //pbxuser
                                   && "" != pn.ExternalPhoneNumber) //nincs ilyen külsõ szám
                             select pn)
                            .OrderBy(x => x.InnerPhoneNumber).First();

                aur.KontaktUserRole = new KontaktUserRole()
                {
                    PBXExtensionData = new PBXExtensionData()
                    {
                        ApplicationUser = user,
                        PhoneNumber = phone,
                        //ExtensionID = phone.InnerPhoneNumber + "@" + user.UserName,
                        //Password = user.Password,
                        isDroped = false,
                        isSynced = false,
                        StartTime = DateTime.Now,
                        EndTime = DateTime.MaxValue
                    }
                };
            }*/

            if (null == UserManager.FindByName("admin"))
            {
                kontaktuser = context.KontaktUser.Create();

                role = context.Roles.Single(x => x.Name == "SysAdmin");

                user = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "sinosz.kontakt@egroup.hu",
                    isEmailValidated = true,
                    Password = "Admin123",
                    isSynced = true,
                    KontaktUser = kontaktuser
                };
                result = UserManager.Create(user, user.Password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült az admin felhasználó rögzítése");
                }

                var aur = new ApplicationUserRole()
                {
                    User = user,
                    Role = role
                };

                context.Entry(aur).State = EntityState.Added;
            }

            //tagnyilvántartó felhasználói
            var sinoszadmins = new List<ApplicationUser>();
            //if (null == UserManager.FindByName("")) sinoszadmins.Add(new ApplicationUser() { UserName = "", Email = "", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("papj")) sinoszadmins.Add(new ApplicationUser() { UserName = "papj", Email = "sandor.judit@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("adamax")) sinoszadmins.Add(new ApplicationUser() { UserName = "adamax", Email = "kosa.adam@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("ormodi")) sinoszadmins.Add(new ApplicationUser() { UserName = "ormodi", Email = "ormodi.robert@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });

            role = context.Roles.Single(x => x.Name == "SinoszAdmin");
            foreach (var item in sinoszadmins)
            {
                result = UserManager.Create(item, item.Password);

                if (!result.Succeeded)
                    throw new ApplicationException("Nem sikerült az admin felhasználó rögzítése");

                var aur = new ApplicationUserRole()
                {
                    User = item,
                    Role = role
                };

                context.Entry(aur).State = EntityState.Added;
                context.SaveChanges();
            }

            var sinoszusers = new List<ApplicationUser>();
            if (null == UserManager.FindByName("miskolc")) sinoszusers.Add(new ApplicationUser() { UserName = "miskolc", Email = "miskolc@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("hamvasr")) sinoszusers.Add(new ApplicationUser() { UserName = "hamvasr", Email = "hamvas.robert@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("habanzs")) sinoszusers.Add(new ApplicationUser() { UserName = "habanzs", Email = "haban.zsuzsa@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("kisidatomi")) sinoszusers.Add(new ApplicationUser() { UserName = "kisidatomi", Email = "kisida.tamas@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("bekesmegye")) sinoszusers.Add(new ApplicationUser() { UserName = "bekesmegye", Email = "bekescsaba@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("bacskk")) sinoszusers.Add(new ApplicationUser() { UserName = "bacskk", Email = "kecskemet@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("cserizs")) sinoszusers.Add(new ApplicationUser() { UserName = "cserizs", Email = "pecs@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("patricia")) sinoszusers.Add(new ApplicationUser() { UserName = "patricia", Email = "szeged@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("ozerita")) sinoszusers.Add(new ApplicationUser() { UserName = "ozerita", Email = "szekesfehervar@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("magyarb")) sinoszusers.Add(new ApplicationUser() { UserName = "magyarb", Email = "gyor@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("mosonik")) sinoszusers.Add(new ApplicationUser() { UserName = "mosonik", Email = "mosoni.krisztina@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("egerszervezet")) sinoszusers.Add(new ApplicationUser() { UserName = "egerszervezet", Email = "eger@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("bigagy")) sinoszusers.Add(new ApplicationUser() { UserName = "bigagy", Email = "szolnok@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("tvarosekp")) sinoszusers.Add(new ApplicationUser() { UserName = "tvarosekp", Email = "tatabanya@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("martonmark")) sinoszusers.Add(new ApplicationUser() { UserName = "martonmark", Email = "nograd@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("somogy")) sinoszusers.Add(new ApplicationUser() { UserName = "somogy", Email = "kaposvar@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("dcanta")) sinoszusers.Add(new ApplicationUser() { UserName = "dcanta", Email = "nyiregyhaza@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("tosina")) sinoszusers.Add(new ApplicationUser() { UserName = "tosina", Email = "tmtitkar@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("schmidtm")) sinoszusers.Add(new ApplicationUser() { UserName = "schmidtm", Email = "veszprem@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("szombathely")) sinoszusers.Add(new ApplicationUser() { UserName = "szombathely", Email = "szombathely@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("pummerkata")) sinoszusers.Add(new ApplicationUser() { UserName = "pummerkata", Email = "zalaegerszeg@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("baloghzsuzsi")) sinoszusers.Add(new ApplicationUser() { UserName = "baloghzsuzsi", Email = "bpsiketek@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("bpnagyothallok")) sinoszusers.Add(new ApplicationUser() { UserName = "bpnagyothallok", Email = "bp.nagyothallok@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("vac")) sinoszusers.Add(new ApplicationUser() { UserName = "vac", Email = "vac@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("budaagi")) sinoszusers.Add(new ApplicationUser() { UserName = "budaagi", Email = "buda.agi@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("nagykanizsa")) sinoszusers.Add(new ApplicationUser() { UserName = "nagykanizsa", Email = "nagykanizsa@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("mako")) sinoszusers.Add(new ApplicationUser() { UserName = "mako", Email = "mako@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("barcika")) sinoszusers.Add(new ApplicationUser() { UserName = "barcika", Email = "kazincbarcika@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("szentes")) sinoszusers.Add(new ApplicationUser() { UserName = "szentes", Email = "szentes@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("zemplen")) sinoszusers.Add(new ApplicationUser() { UserName = "zemplen", Email = "zemplen@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("ozd")) sinoszusers.Add(new ApplicationUser() { UserName = "ozd", Email = "ozd@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("sopron")) sinoszusers.Add(new ApplicationUser() { UserName = "sopron", Email = "sopron@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("oroshaza")) sinoszusers.Add(new ApplicationUser() { UserName = "oroshaza", Email = "oroshaza@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("gyongyos")) sinoszusers.Add(new ApplicationUser() { UserName = "gyongyos", Email = "gyongyos@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("ajka")) sinoszusers.Add(new ApplicationUser() { UserName = "ajka", Email = "heckkarolyne@freemail.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("nemetheva")) sinoszusers.Add(new ApplicationUser() { UserName = "nemetheva", Email = "nemeth.eva@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("hmvhely")) sinoszusers.Add(new ApplicationUser() { UserName = "hmvhely", Email = "hodmezovasarhely@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("solt")) sinoszusers.Add(new ApplicationUser() { UserName = "solt", Email = "solt@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("bajaszervezet")) sinoszusers.Add(new ApplicationUser() { UserName = "bajaszervezet", Email = "baja@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });
            if (null == UserManager.FindByName("havaldazs")) sinoszusers.Add(new ApplicationUser() { UserName = "havaldazs", Email = "havalda.zsofi@sinosz.hu", isEmailValidated = true, Password = "Sinosz123", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() });

            role = context.Roles.Single(x => x.Name == "SinoszUser");
            foreach (var item in sinoszusers)
            {
                result = UserManager.Create(item, item.Password);

                if (!result.Succeeded)
                    throw new ApplicationException("Nem sikerült az admin felhasználó rögzítése");

                var aur = new ApplicationUserRole()
                {
                    User = item,
                    Role = role
                };

                context.Entry(aur).State = EntityState.Added;
                context.SaveChanges();
            }
        
            var interpreters = new List<ApplicationUser>();
            var centers = new List<ApplicationUser>();
            var profiles = new List<KontaktUser>();

            if (null == UserManager.FindByName("keglovics.zsuzsa")) { interpreters.Add(new ApplicationUser() { UserName = "keglovics.zsuzsa", Email = "keglovics.zsuzsa@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "keglovics.zsuzsa", Id = "F9EFEC81-1052-4CE2-9DDB-6691D07360F3" }); profiles.Add(new KontaktUser() { FirstName = "Keglovics", LastName = "Zsuzsa", SinoszId = "keglovics.zsuzsa" }); }
            if (null == UserManager.FindByName("guttyan.timea")) { interpreters.Add(new ApplicationUser() { UserName = "guttyan.timea", Email = "guttyan.timea@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "guttyan.timea", Id = "F9EFEC81-1052-4CE2-9DDB-6691D07360F3" }); profiles.Add(new KontaktUser() { FirstName = "Sipos", LastName = "Koppányné", SinoszId = "guttyan.timea" }); }
            if (null == UserManager.FindByName("marton.orsolya")) { interpreters.Add(new ApplicationUser() { UserName = "marton.orsolya", Email = "marton.orsolya@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "marton.orsolya", Id = "F9EFEC81-1052-4CE2-9DDB-6691D07360F3" }); profiles.Add(new KontaktUser() { FirstName = "Valter-Marton", LastName = "Orsolya", SinoszId = "marton.orsolya" }); }
            if (null == UserManager.FindByName("bertus.edit")) { interpreters.Add(new ApplicationUser() { UserName = "bertus.edit", Email = "bertus.edit@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "bertus.edit", Id = "3022F3F5-35B1-4231-8BD2-81A8F5113DA7" }); profiles.Add(new KontaktUser() { FirstName = "Bertus", LastName = "Edit", SinoszId = "bertus.edit" }); }
            if (null == UserManager.FindByName("karl.janosne")) { interpreters.Add(new ApplicationUser() { UserName = "karl.janosne", Email = "karl.janosne@sinosz.hu ", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "karl.janosne", Id = "3022F3F5-35B1-4231-8BD2-81A8F5113DA7" }); profiles.Add(new KontaktUser() { FirstName = "Karl", LastName = "Jánosné", SinoszId = "karl.janosne" }); }
            if (null == UserManager.FindByName("finna.monika")) { interpreters.Add(new ApplicationUser() { UserName = "finna.monika", Email = "finna.monika@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "finna.monika", Id = "B0C9751E-0707-41A3-B978-5FB72401D0A7" }); profiles.Add(new KontaktUser() { FirstName = "Finna", LastName = "Mónika", SinoszId = "finna.monika" }); }
            if (null == UserManager.FindByName("fitor.margit")) { interpreters.Add(new ApplicationUser() { UserName = "fitor.margit", Email = "fitor.margit@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "fitor.margit", Id = "B0C9751E-0707-41A3-B978-5FB72401D0A7" }); profiles.Add(new KontaktUser() { FirstName = "Kernné Fitor", LastName = "Margit", SinoszId = "fitor.margit" }); }
            if (null == UserManager.FindByName("nagy.anita")) { interpreters.Add(new ApplicationUser() { UserName = "nagy.anita", Email = "nagy.anita@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "nagy.anita", Id = "B0C9751E-0707-41A3-B978-5FB72401D0A7" }); profiles.Add(new KontaktUser() { FirstName = "Nagy", LastName = "Anita", SinoszId = "nagy.anita" }); }
            if (null == UserManager.FindByName("matrai.gabriella")) { interpreters.Add(new ApplicationUser() { UserName = "matrai.gabriella", Email = "matrai.gabriella@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "matrai.gabriella", Id = "E94E97BE-EAAA-460F-8CDB-760F8DAA2A4A" }); profiles.Add(new KontaktUser() { FirstName = "Mátrai", LastName = "Gabriella", SinoszId = "matrai.gabriella" }); }
            if (null == UserManager.FindByName("vorosk")) { interpreters.Add(new ApplicationUser() { UserName = "vorosk", Email = "vorosk@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "vorosk", Id = "E94E97BE-EAAA-460F-8CDB-760F8DAA2A4A" }); profiles.Add(new KontaktUser() { FirstName = "Vörös", LastName = "Krisztina", SinoszId = "vorosk" }); }
            if (null == UserManager.FindByName("voros.zsolt")) { interpreters.Add(new ApplicationUser() { UserName = "voros.zsolt", Email = "voros.zsolt@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "voros.zsolt", Id = "E94E97BE-EAAA-460F-8CDB-760F8DAA2A4A" }); profiles.Add(new KontaktUser() { FirstName = "Vörös", LastName = "Zsolt", SinoszId = "voros.zsolt" }); }
            if (null == UserManager.FindByName("kiss.gabriella")) { interpreters.Add(new ApplicationUser() { UserName = "kiss.gabriella", Email = "kiss.gabriella@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "kiss.gabriella", Id = "7AABCFDB-22CE-4A2F-9A43-650B4D2FCAAA" }); profiles.Add(new KontaktUser() { FirstName = "Kiss", LastName = "Gabriella", SinoszId = "kiss.gabriella" }); }
            if (null == UserManager.FindByName("hortovanyi.zsuzsanna")) { interpreters.Add(new ApplicationUser() { UserName = "hortovanyi.zsuzsanna", Email = "hortovanyi.zsuzsanna@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "hortovanyi.zsuzsanna", Id = "7AABCFDB-22CE-4A2F-9A43-650B4D2FCAAA" }); profiles.Add(new KontaktUser() { FirstName = "Némethné Hortoványi", LastName = "Zsuzsanna", SinoszId = "hortovanyi.zsuzsanna" }); }
            if (null == UserManager.FindByName("szemanszki.renata")) { interpreters.Add(new ApplicationUser() { UserName = "szemanszki.renata", Email = "szemanszki.renata@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "szemanszki.renata", Id = "7AABCFDB-22CE-4A2F-9A43-650B4D2FCAAA" }); profiles.Add(new KontaktUser() { FirstName = "Szemánszki", LastName = "Renáta ", SinoszId = "szemanszki.renata" }); }
            if (null == UserManager.FindByName("hamerszki.erika")) { interpreters.Add(new ApplicationUser() { UserName = "hamerszki.erika", Email = "hamerszki.erika@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "hamerszki.erika", Id = "0ABF15E6-5E70-4F2D-BB44-FBF3773C72F3" }); profiles.Add(new KontaktUser() { FirstName = "Hamerszki", LastName = "Erika", SinoszId = "hamerszki.erika" }); }
            if (null == UserManager.FindByName("kosik.maria")) { interpreters.Add(new ApplicationUser() { UserName = "kosik.maria", Email = "kosik.maria@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "kosik.maria", Id = "0ABF15E6-5E70-4F2D-BB44-FBF3773C72F3" }); profiles.Add(new KontaktUser() { FirstName = "Kosik", LastName = "Mária", SinoszId = "kosik.maria" }); }
            if (null == UserManager.FindByName("zsoldos.erzsebet")) { interpreters.Add(new ApplicationUser() { UserName = "zsoldos.erzsebet", Email = "zsoldos.erzsebet@sinosz.hu ", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "zsoldos.erzsebet", Id = "8C386AAE-7030-4265-B145-971A7DA44172" }); profiles.Add(new KontaktUser() { FirstName = "Gréczi-Zsoldos", LastName = "Miklósné", SinoszId = "zsoldos.erzsebet" }); }
            if (null == UserManager.FindByName("kiss.nikoletta")) { interpreters.Add(new ApplicationUser() { UserName = "kiss.nikoletta", Email = "kiss.nikoletta@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "kiss.nikoletta", Id = "8C386AAE-7030-4265-B145-971A7DA44172" }); profiles.Add(new KontaktUser() { FirstName = "Polyákné Kiss", LastName = "Nikoletta", SinoszId = "kiss.nikoletta" }); }
            if (null == UserManager.FindByName("gyorfi.gabi")) { interpreters.Add(new ApplicationUser() { UserName = "gyorfi.gabi", Email = "gyorfi.gabi@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "gyorfi.gabi", Id = "057EAEFC-220C-4225-8910-FE726F548A08" }); profiles.Add(new KontaktUser() { FirstName = "Gyõrfi", LastName = "Gabriella", SinoszId = "gyorfi.gabi" }); }
            if (null == UserManager.FindByName("laczina.arpadne")) { interpreters.Add(new ApplicationUser() { UserName = "laczina.arpadne", Email = "laczina.arpadne@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "laczina.arpadne", Id = "057EAEFC-220C-4225-8910-FE726F548A08" }); profiles.Add(new KontaktUser() { FirstName = "Laczina", LastName = "Árpádné", SinoszId = "laczina.arpadne" }); }
            if (null == UserManager.FindByName("tamas.angela")) { interpreters.Add(new ApplicationUser() { UserName = "tamas.angela", Email = "tamas.angela@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "tamas.angela", Id = "057EAEFC-220C-4225-8910-FE726F548A08" }); profiles.Add(new KontaktUser() { FirstName = "Tamás", LastName = "Angéla", SinoszId = "tamas.angela" }); }
            if (null == UserManager.FindByName("szabo.ildiko")) { interpreters.Add(new ApplicationUser() { UserName = "szabo.ildiko", Email = "szabo.ildiko@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "szabo.ildiko", Id = "A3D3FDE2-7D1F-4B5E-BD6F-C9B83C5B1479" }); profiles.Add(new KontaktUser() { FirstName = "Demeter-Szabó", LastName = "Ildikó", SinoszId = "szabo.ildiko" }); }
            if (null == UserManager.FindByName("hete.lilla")) { interpreters.Add(new ApplicationUser() { UserName = "hete.lilla", Email = "hete.lilla@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "hete.lilla", Id = "A3D3FDE2-7D1F-4B5E-BD6F-C9B83C5B1479" }); profiles.Add(new KontaktUser() { FirstName = "Hete", LastName = "Lilla", SinoszId = "hete.lilla" }); }
            if (null == UserManager.FindByName("riskone.tunde")) { interpreters.Add(new ApplicationUser() { UserName = "riskone.tunde", Email = "riskone.tunde@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "riskone.tunde", Id = "A3D3FDE2-7D1F-4B5E-BD6F-C9B83C5B1479" }); profiles.Add(new KontaktUser() { FirstName = "Riskó", LastName = "Istvánné", SinoszId = "riskone.tunde" }); }
            if (null == UserManager.FindByName("magyar.melinda")) { interpreters.Add(new ApplicationUser() { UserName = "magyar.melinda", Email = "magyar.melinda@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "magyar.melinda", Id = "3B9F193E-087A-4C81-B0C9-BAA55ECA8F6B" }); profiles.Add(new KontaktUser() { FirstName = "Magyar", LastName = "Melinda ", SinoszId = "magyar.melinda" }); }
            if (null == UserManager.FindByName("foldesi.eva")) { interpreters.Add(new ApplicationUser() { UserName = "foldesi.eva", Email = "foldesi.eva@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "foldesi.eva", Id = "3B9F193E-087A-4C81-B0C9-BAA55ECA8F6B" }); profiles.Add(new KontaktUser() { FirstName = "Földesi", LastName = "Éva", SinoszId = "foldesi.eva" }); }
            if (null == UserManager.FindByName("horvath.brigitta")) { interpreters.Add(new ApplicationUser() { UserName = "horvath.brigitta", Email = "horvath.brigitta@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "horvath.brigitta", Id = "3B9F193E-087A-4C81-B0C9-BAA55ECA8F6B" }); profiles.Add(new KontaktUser() { FirstName = "Horváth", LastName = "Brigitta", SinoszId = "horvath.brigitta" }); }
            if (null == UserManager.FindByName("jonas.zsofia")) { interpreters.Add(new ApplicationUser() { UserName = "jonas.zsofia", Email = "jonas.zsofia@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "jonas.zsofia", Id = "F40F974F-6EBF-4BCA-A6DB-D8B1CE881740" }); profiles.Add(new KontaktUser() { FirstName = "Jónás", LastName = "Zsófia", SinoszId = "jonas.zsofia" }); }
            if (null == UserManager.FindByName("takacs.robertne")) { interpreters.Add(new ApplicationUser() { UserName = "takacs.robertne", Email = "takacs.robertne@sinosz.hu", isEmailValidated = true, Password = "Tolmacs!2014", isSynced = true, KontaktUser = kontaktuser = context.KontaktUser.Create() }); centers.Add(new ApplicationUser() { UserName = "takacs.robertne", Id = "F40F974F-6EBF-4BCA-A6DB-D8B1CE881740" }); profiles.Add(new KontaktUser() { FirstName = "Takács", LastName = "Róbertné", SinoszId = "takacs.robertne" }); }

            role = context.Roles.Single(x => x.Name == "Jeltolmácsok");
            foreach (var item in interpreters)
            {
                var pro = profiles.FirstOrDefault(x => x.SinoszId == item.UserName);
                item.KontaktUser.FirstName = pro.FirstName;
                item.KontaktUser.LastName = pro.LastName;

                result = UserManager.Create(item, item.Password);

                if (!result.Succeeded)
                    throw new ApplicationException("Nem sikerült az jeltolmácsok rögzítése");

                var aur = new ApplicationUserRole()
                {
                    User = item,
                    Role = role
                };

                var cent = centers.FirstOrDefault(x => x.UserName == item.UserName);
                Guid centG = Guid.Parse(cent.Id);
                var inter = context.InterpreterCenter.Find(centG);

                var kur = context.KontaktUserRole.Create();
                kur.InterpreterCenter = inter;

                var pbx = context.PBXExtensionData.Create();
                pbx.ApplicationUser = context.Users.Single(x => x.UserName == item.UserName);
                var num = (from pn in context.PhoneNumber
                            from px in context.PBXExtensionData.Where(x => x.PhoneNumber.Id == pn.Id && x.isDroped == false).DefaultIfEmpty()
                            where (null == px.Id //nincs ilyen aktív szám
                                  && (null == pn.ExternalPhoneNumber) //nem kell külsõ szám + átállunk NULL-ra
                                  && pn.InnerPhoneNumber.Length <= 4)//a 7000-es számmezõbõl
                            select pn)
                            .OrderBy(x => x.InnerPhoneNumber).FirstOrDefault();
                pbx.PhoneNumber = num; 
                pbx.isDroped = false;
                pbx.isSynced = false;
                pbx.StartTime = DateTime.Now;
                pbx.EndTime = DateTime.MaxValue;

                kur.PBXExtensionData = pbx;
                
                aur.KontaktUserRole = kur;

                context.Entry(aur).State = EntityState.Added; //remélem menti kur-t is
                context.SaveChanges();
            }           
            
        }
    
    }
}
