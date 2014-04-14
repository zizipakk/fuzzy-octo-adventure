using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Tax.Portal.Mailers;
using System.Collections.Generic;
using Tax.Portal.Helpers;
using System.Data.Entity;
using System;
using Newtonsoft.Json;
using Tax.Data.Models;
using Tax.Portal.Models;
using JQGrid.Helpers;
using System.Diagnostics;

namespace Tax.Portal.Controllers  
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = new ApplicationDbContext();

        private IAuthenticationManager _AuthenticationManager;
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                using (log4net.ThreadContext.Stacks["NDC"].Push("AuthenticationManager.get"))
                {
                    log.Info("begin");
                    if (null == _AuthenticationManager)
                    {
                        _AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                    }
                    log.Info("end");
                    return _AuthenticationManager;
                }
            }
            set
            {
                using (log4net.ThreadContext.Stacks["NDC"].Push("AuthenticationManager.get"))
                {
                    log.Info("begin");
                    _AuthenticationManager = value;
                    log.Info("end");
                }
            }
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public AccountController()
            : this(new ApplicationDbContext())
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("AccountController()"))
            {
                log.Info("begin");
                log.Info("end");
            }
        }

        public AccountController(ApplicationDbContext db)
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("AccountController(db)"))
            {
                log.Info("begin");
                this.db = db;
                log.Info("end");
            }
        }
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("AccountController(userManager)"))
            {
                log.Info("begin");
                UserManager = userManager; 
                UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };                
                log.Info("end");
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Login(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("returnUrl: {0}", returnUrl));
                ViewBag.ReturnUrl = returnUrl;
                var model = new LoginViewModel();
                model.Refresh(ModelState);
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end");
                return View(model);
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Login(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}, returnUrl: {1}", JsonConvert.SerializeObject(model), returnUrl));
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user != null)
                    {
                        if (!user.isEmailValidated)
                            return RedirectToAction(MVC.Home.MissingEmailValidation());

                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);   
                    }
                    else
                    {
                        ModelState.AddModelError("", "A bejelentkezési név vagy jelszó helytelen!");
                    }
                }
                // If we got this far, something failed, redisplay form
                model.Refresh(ModelState);
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end");
                return View(model);
            }
        }

        [AllowAnonymous]
        public virtual ActionResult Preregister()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Register(Get)"))
            {
                log.Info("begin");
                PreRegisterViewModel model = new PreRegisterViewModel();
                model.IsSinosz = true;
                model.SocialPositionList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                                            .Union(db.SocialPosition.Select(x => new MyListItem { Value = x.Id, Text = x.SocialPositionName }))
                                            .OrderBy(x => x.Text)
                                            .ToList();
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Preregister(PreRegisterViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Preregister(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));

                //ellenőrzés születési dátum és tagi azonosító szerint
                var sinoszuser = db.SinoszUser.FirstOrDefault(x => x.SinoszId == model.PreSinoszId && x.BirthDate == model.PreBirthDate);
                if (null == sinoszuser)
                {
                    ModelState.AddModelError("SinoszId", "Az általad megadott tagsági szám a beírt születési dátummal együtt nem szerepel a tagnyilvántartásban!");
                }
                else
                {
                    var usr = db.Users.Any(x => x.SinoszUser.Id == sinoszuser.Id && x.isLocked == false);
                    if (usr)//regisztráció tartozik hozzá és még él
                        ModelState.AddModelError("SinoszId", "Ezzel a SINOSZ tagsági számmal már létrejött a regisztráció.");
                    if (sinoszuser.SinoszUserStatus.StatusName != "Aktív")
                        ModelState.AddModelError("SinoszId", "Ha SINOSZ tag vagy, győződj meg róla, hogy befizetted-e a 2014. évi tagsági díjat. Ha igen, és ennek ellenére probléma merül fel, vagy szeretnél SINOSZ tag lenni, keresd fel szervezetünket: www.sinosz.hu");
                }

                log.Info("step 1 sinosz user ellenőrizve");
                if (ModelState.IsValid)
                {
                    //előregisztrációs adatok
                    PreregistrationData preregistrationdata = db.PreregistrationData.Create();
                    SocialPosition socialposition = db.SocialPosition.Find(model.SocialPositionId);
                    preregistrationdata.SocialPosition = socialposition;
                    preregistrationdata.IsNeedForHealth = model.IsNeedForHealth;
                    preregistrationdata.IsNeedForJob = model.IsNeedForJob;
                    preregistrationdata.IsNeedForLife = model.IsNeedForLife;
                    preregistrationdata.Job = model.Job;
                    db.Entry(preregistrationdata).State = EntityState.Added;

                    //profil
                    KontaktUser kontaktuser = db.KontaktUser.Create();
                    kontaktuser.FirstName = model.FirstName;
                    kontaktuser.LastName = model.LastName;
                    kontaktuser.isSinoszMember = model.IsSinosz;
                    kontaktuser.SinoszId = model.PreSinoszId;
                    kontaktuser.isCommunicationRequested = model.IsRequestCommunication;
                    kontaktuser.isDeviceReqested = model.IsRequestDevice;
                    kontaktuser.BirthDate = model.PreBirthDate;
                    kontaktuser.PreregistrationData = preregistrationdata;

                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
//TODO. átmenetileg simán mentem a jelszót is a pbx miatt
                        Password = model.Password,
                        KontaktUser = kontaktuser,
//TODO nem köthetem össze, amíg nem validál
                        //SinoszUser = sinoszuser,
                        isSynced = false,
                    };

                    IdentityRole role = db.Roles.SingleOrDefault(x => x.Name == "Ügyfél"); 
                    if (null != role)
                    {
                        user.Roles.Add(new ApplicationUserRole() { Role = role }); //indirekció: IdentityRole => ApplicationUserRole => ApplicationUserRole
                    }

                    var result = await UserManager.CreateAsync(user, model.Password);//ez menti a userre bekötött modelleket
                    log.Info("step 2 user létrehozva");
                    if (result.Succeeded)
                    {

                        //A előregisztrációról email-t küldünk, amivel 
                        //az email címet validáljuk

                        //címzés
                        var addresses = new List<Addressee>();
                        addresses.Add(new Addressee
                        {
                            Email = model.Email,
                            FullName = model.UserName
                        });

                        //token létrehozása
                        var token = new Token(db, TokenTargets.EmailValidation, user.Id);//menti is a db-!
                        log.Info("step 3 token létrehozva");

                        //adatok a levélre
                        var ved = new ValidateEmailData
                        {
                            email = model.Email,
                            username = model.UserName,
                            token = token.Code,
                            ValidUntil = token.ValidUntil,
                            fullname = string.Format("{0} {1}", model.FirstName ?? "", model.LastName ?? "")
                        };

                        //maga a levél
                        var message = new Message<ValidateEmailData>
                        {
                            //Subject = "Előregisztráció: e-mail cím érvényesítése",
                            Subject = "Regisztráció: e-mail cím érvényesítése",
                            Data = ved
                        };

                        //Küldjük a levelet
                        log.Info("step 4 levélküldés kezdete");
                        MessageHelper<ValidateEmailData>.SendMessageToQueue(message, addresses, Url);
                        log.Info("step 5 levélküldés vége");

                        //Megyünk a tájékoztató oldalra
                        log.Info("end with ok");
                        return RedirectToAction(MVC.Home.WaitForEmailValidationAfterPreRegistration());
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
//TODO béna megoldás átmenetileg, mert nem tudom különben átírni a hibaüzenetet, ami angol
                else
                {
                    if (ModelState["PreBirthDate"].Errors.Count() > 0
                        && !ModelState["PreBirthDate"].Errors.Select(x => x.ErrorMessage).Contains("A(z) [Születési idő] mezőt kötelező kitölteni"))
                    {
                        ModelState["PreBirthDate"].Errors.Clear();
                        ModelState["PreBirthDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Születési dátum] mezőben!"));
                    }
                }
//

                // If we got this far, something failed, redisplay form
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end with validation error");
                model.Refresh(ModelState);

                model.SocialPositionList = (new List<MyListItem>() { new MyListItem { Value = Guid.Empty, Text = string.Empty } })
                            .Union(db.SocialPosition.Select(x => new MyListItem { Value = x.Id, Text = x.SocialPositionName }))
                            .OrderBy(x => x.Text)
                            .ToList();

                return View(model);
            }
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Register(Get)"))
            {
                log.Info("begin");
                var model = new RegisterViewModel();
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Register(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));

                if (model.IsSinosz)//ellenőrzés születési dátum és tagi azonosító szerint
                {
                    //ellenőrzés születési dátum és tagi azonosító szerint
                    var sinoszuser = db.SinoszUser.FirstOrDefault(x => x.SinoszId == model.SinoszId && x.BirthDate == model.BirthDate);
                    if (null == sinoszuser)
                    {
                        ModelState.AddModelError("SinoszId", "Az általad megadott tagsági szám a beírt születési dátummal együtt nem szerepel a tagnyilvántartásban!");
                    }
                    else
                    {
                        var usr = db.Users.Any(x => x.SinoszUser.Id == sinoszuser.Id && x.isLocked == false);
                        if (usr)//regisztráció tartozik hozzá és még él
                            ModelState.AddModelError("SinoszId", "Ezzel a SINOSZ tagsági számmal már létrejött a regisztráció.");
                        if (sinoszuser.SinoszUserStatus.StatusName != "Aktív")
                            ModelState.AddModelError("SinoszId", "Ha SINOSZ tag vagy, győződj meg róla, hogy befizetted-e a 2014. évi tagsági díjat. Ha igen, és ennek ellenére probléma merül fel, vagy szeretnél SINOSZ tag lenni, keresd fel szervezetünket: www.sinosz.hu");
                    }
                }                

                log.Info("step 1 sinosz user ellenőrizve");
                if (ModelState.IsValid)
                {
                    //mindenképpen kell a telefonkönyv miatt
                    KontaktUser kontaktuser = db.KontaktUser.Create();
                    kontaktuser.FirstName = model.FirstName;
                    kontaktuser.LastName = model.LastName;

                    if (model.IsSinosz)
                    {
                        kontaktuser.isSinoszMember = model.IsSinosz;
                        kontaktuser.SinoszId = model.SinoszId;
                        kontaktuser.isCommunicationRequested = model.IsRequestCommunication;
                        kontaktuser.isDeviceReqested = model.IsRequestDevice;
                        kontaktuser.BirthDate = model.BirthDate;
                    }

                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
//TODO. átmenetileg simán mentem a jelszót is a pbx miatt
                        Password = model.Password,
                        KontaktUser = kontaktuser,
//TODO nem köthetem össze, amíg nem validál
                        //SinoszUser = sinoszuser,
                        isSynced = false,
                    };

                    //user.Roles readonly, ezért indirekt írása az ApplicationUserRole táblának
                    IdentityRole role = db.Roles.SingleOrDefault(x => x.Name == "Ügyfél"); //Egy ilyen kell, különben balhé
                    if (null != role)
                    {
                        user.Roles.Add(new ApplicationUserRole() { Role = role }); //indirekció: IdentityRole => ApplicationUserRole => ApplicationUserRole
                    }

                    var result = await UserManager.CreateAsync(user, model.Password);//ez menti a modeleket
                    log.Info("step 2 user létrehozva");
                    if (result.Succeeded)
                    {
                        //Todo csak a megerősítő levél után kell beléptetni
                        //await SignInAsync(user, isPersistent: false);

                        //A regisztrációról email-t küldünk, amivel 
                        //az email címet validáljuk

                        //címzés
                        var addresses = new List<Addressee>();
                        addresses.Add(new Addressee
                        {
                            Email = model.Email,
                            FullName = model.UserName
                        });

                        //token létrehozása
                        var token = new Token(db, TokenTargets.EmailValidation, user.Id);//menti is a db-!
                        log.Info("step 3 token létrehozva");

                        //adatok a levélre
                        var ved = new ValidateEmailData
                        {
                            email = model.Email,
                            username = model.UserName,
                            token = token.Code,
                            ValidUntil = token.ValidUntil,
                            fullname = string.Format("{0} {1}", model.FirstName ?? "", model.LastName ?? "")
                        };

                        //maga a levél
                        var message = new Message<ValidateEmailData>
                        {
                            Subject = "Új regisztráció: e-mail cím érvényesítése",
                            Data = ved
                        };

                        //Küldjük a levelet
                        log.Info("step 4 levélküldés kezdete");
                        MessageHelper<ValidateEmailData>.SendMessageToQueue(message, addresses, Url);
                        log.Info("step 5 levélküldés vége");

                        log.Info("end with ok");
                        return RedirectToAction(MVC.Home.WaitForEmailValidation());
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                else
                {
//TODO béna megoldás átmenetileg, mert nem tudom különben átírni a hibaüzenetet, ami angol
                    if (ModelState["BirthDate"].Errors.Count() > 0)
                    {
                        ModelState["BirthDate"].Errors.Clear();
                        ModelState["BirthDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Születési dátum] mezőben!"));
                    }
                }
                

                // If we got this far, something failed, redisplay form
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end with validation error");
                model.Refresh(ModelState);
                return View(model);
            }
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Disassociate"))
            {
                log.Info("begin");
                ManageMessageId? message = null;
                IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
                if (result.Succeeded)
                {
                    message = ManageMessageId.RemoveLoginSuccess;
                }
                else
                {
                    message = ManageMessageId.Error;
                }
                log.Info("end");
                return RedirectToAction(MVC.Account.Manage(message: message));
            }
        }

        //
        // GET: /Account/Manage
        public virtual ActionResult Manage(ManageMessageId? message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Manage(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("message: {0}", message));
                ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess ? "A megváltozott jelszó elmentve."
                    : message == ManageMessageId.SetPasswordSuccess ? "A jelszó beállítva."
                    : message == ManageMessageId.RemoveLoginSuccess ? "A külső jelszó törölve."
                    : message == ManageMessageId.Error ? "Hiba történt."
                    : "";
                ViewBag.HasLocalPassword = HasPassword();
                ViewBag.ReturnUrl = Url.Action(MVC.Account.Manage());
                var model = new ManageUserViewModel();
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Manage(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                bool hasPassword = HasPassword();
                ViewBag.HasLocalPassword = hasPassword;
                ViewBag.ReturnUrl = Url.Action("Manage");

//TODO. átmenetileg simán mentem a jelszót is a pbx miatt
                var user = db.Users.Find(User.Identity.GetUserId());
                if (null != user)
                {
                    user.Password = model.NewPassword;
                    if (user.isSynced) { user.isSynced = false; }
                    
                    //db.Entry(user).State = EntityState.Modified;
                    //var pbxext = db.PBXExtensionData.SingleOrDefault(x => x.ApplicationUser == user && !x.isDroped);
                    //if (null != pbxext) //amennyiben van extension-ja
                    //{
                    //    //pbxext.Password = model.NewPassword;
                    //    db.Entry(pbxext).State = EntityState.Modified;
                    //}
                }

                if (hasPassword)
                {
                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            //email küldése
                            var subject = string.Empty;
                            var addresses = new List<Addressee>();

                            addresses.Add(new Addressee
                            {
                                Email = user.Email,
                                FullName = user.UserName
                            });

                            subject = string.Format("{0}: {1}", "Jelszóváltoztatás sikeres", user.Email);
                            var ied = new ResetPasswordCompletedData
                            {
                                now = DateTime.Now,
                                username = user.UserName,
                                email = user.Email,
                                FullName = string.Format("{0} {1}", user.KontaktUser.FirstName ?? "", user.KontaktUser.LastName ?? "")
                            };
                            var mail = new Message<ResetPasswordCompletedData>
                            {
                                Subject = subject,
                                Data = ied
                            };
                            MessageHelper<ResetPasswordCompletedData>.SendMessageToQueue(mail, addresses, Url);

                            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }
                else
                {
                    // User does not have a password so remove any validation errors caused by a missing OldPassword field
                    ModelState state = ModelState["OldPassword"];
                    if (state != null)
                    {
                        state.Errors.Clear();
                    }

                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                        if (result.Succeeded)
                        {
                            //email küldése
                            var subject = string.Empty;
                            var addresses = new List<Addressee>();

                            addresses.Add(new Addressee
                            {
                                Email = user.Email,
                                FullName = user.UserName
                            });

                            subject = string.Format("{0}: {1}", "Jelszóváltoztatás sikeres", user.Email);
                            var ied = new ResetPasswordCompletedData
                            {
                                now = DateTime.Now,
                                username = user.UserName,
                                email = user.Email
                            };
                            var mail = new Message<ResetPasswordCompletedData>
                            {
                                Subject = subject,
                                Data = ied
                            };
                            MessageHelper<ResetPasswordCompletedData>.SendMessageToQueue(mail, addresses, Url);

                            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }

                // If we got this far, something failed, redisplay form
                model.Refresh(ModelState);
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end (with validation error)");
                return View(model);
            }
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ExternalLogin(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("provider: {0}, returnUrl: {1}", provider, returnUrl));
                // Request a redirect to the external login provider
                log.Info("end");
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ExternalLoginCallback"))
            {
                log.Info("begin");
                log.Info(string.Format("returnUrl: {0}", returnUrl));
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (loginInfo == null)
                {
                    log.Info("end (with prompt to login)");
                    return RedirectToAction(MVC.Account.Login());
                }

                // Sign in the user with this external login provider if the user already has a login
                var user = await UserManager.FindAsync(loginInfo.Login);
                if (user != null)
                {
                    //Beléptetem
                    //if (!user.isEmailValidated)
                    //    return RedirectToAction(MVC.Home.MissingEmailValidation());
                    //else
                        await SignInAsync(user, isPersistent: false);                            

                    log.Info("end (with logged in ok))");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    log.Info("end (with prompt to create account)");
                    return View(MVC.Account.Views.ExternalLoginConfirmation, 
                        new ExternalLoginConfirmationViewModel 
                        { 
                            UserName = loginInfo.DefaultUserName 
                        });
                }
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LinkLogin(string provider)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("LinkLogin(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("provider: {0}", provider));
                // Request a redirect to the external login provider to link a login for the current user
                log.Info("end");
                return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
            }
        }

        //
        // GET: /Account/LinkLoginCallback
        public virtual async Task<ActionResult> LinkLoginCallback()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("LinkLoginCallback"))
            {
                log.Info("begin");
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
                if (loginInfo == null)
                {
                    log.Error("end with loginInfo==null");
                    return RedirectToAction(MVC.Account.Manage(message: ManageMessageId.Error));
                }
                var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
                if (result.Succeeded)
                {
                    log.Info("end");
                    return RedirectToAction(MVC.Account.Manage());
                }
                log.Error("end with not succeded login");
                return RedirectToAction(MVC.Account.Manage(message: ManageMessageId.Error));
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ExternalLoginConfirmation(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}, returnUrl: {1}", JsonConvert.SerializeObject(model), returnUrl));

                if (User.Identity.IsAuthenticated)
                {
                    //return RedirectToAction("Manage");
                    return RedirectToAction(MVC.Account.Manage());
                }

                if (ModelState.IsValid)
                {
                    // Get the information about the user from the external login provider
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        //return View("ExternalLoginFailure");
                        return View(MVC.Account.Views.ExternalLoginFailure);
                    }
                    var user = new ApplicationUser() { UserName = model.UserName };
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            //Beléptetem
                            //if (!user.isEmailValidated)
                            //    return RedirectToAction(MVC.Home.MissingEmailValidation());
                            //else
                                await SignInAsync(user, isPersistent: false);                            

                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }

                ViewBag.ReturnUrl = returnUrl;
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("LogOff(Post)"))
            {
                log.Info("begin");
                AuthenticationManager.SignOut();
                log.Info("end");
                return RedirectToAction(MVC.Home.Index());
            }
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public virtual ActionResult ExternalLoginFailure()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ExternalLoginFailure"))
            {
                log.Info("begin");
                log.Info("end");
                return View();
            }
        }

        [ChildActionOnly]
        public virtual ActionResult RemoveAccountList()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("RemoveAccountList"))
            {
                log.Info("begin");
                var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
                ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
                log.Info("end");
                return (ActionResult)PartialView(MVC.Account.Views._RemoveAccountPartial, linkedAccounts);
            }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction(MVC.Home.Index());
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordStart()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ResetPasswordStart(Get)"))
            {
                log.Info("begin");
                var model = new ResetPasswordStartViewModel();
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual async Task<ActionResult> ResetPasswordStart(ResetPasswordStartViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ResetPasswordStart(Post)"))
            {
                log.Info("begin");
                if (ModelState.IsValid)
                {
                    //A jelszóvisszaállításról email-t küldünk, amivel 
                    //az email címet validáljuk

                    //legfeljebb egy ilyen e-mail cím van
                    var user = await db.Users.SingleOrDefaultAsync(x => x.Email == model.Email);

                    if (null != user)
                    {
                        //címzés
                        var addresses = new List<Addressee>();
                        addresses.Add(new Addressee
                        {
                            Email = user.Email,
                            FullName = user.UserName
                        });

                        //token létrehozása
                        var token = new Token(db, TokenTargets.PasswordReset, user.Id);

                        //adatok a levélre
                        var ved = new ResetPasswordData
                        {
                            email = user.Email,
                            username = user.UserName,
                            token = token.Code,
                            ValidUntil = token.ValidUntil,
                            FullName = string.Format("{0} {1}", user.KontaktUser.FirstName ?? "", user.KontaktUser.LastName ?? "")
                        };

                        //maga a levél
                        var email = new Message<ResetPasswordData>
                        {
                            Subject = "Jelszó visszaállítása",
                            Data = ved
                        };

                        //Küldjük a levelet
                        MessageHelper<ResetPasswordData>.SendMessageToQueue(email, addresses, Url);
                    }
                }
                var message = "Köszönjük az információt! Ha az e-mail cím szerepel az adatbázisunkban, elindítjuk az új jelszó megadásához szükséges folyamatot. Ebben az esetben a további lépésekről e-mail-t küldünk, amiben minden információ szerepel.";
                log.Info("end");
                return RedirectToAction(MVC.Account.ResetPasswordFinalize(token: string.Empty, message: message));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordFinalize(string token, string message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ResetPasswordFinalize(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("token: {0}, message: {1}", token, message));
                var model = new ResetPasswordFinalizeViewModel() { Token = token, Message=message };
                model.Refresh(ModelState);
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual async Task<ActionResult> ResetPasswordFinalize(ResetPasswordFinalizeViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ResetPasswordFinalize(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                if (ModelState.IsValid)
                {
                    string message = null;

                    var token = db.Tokens.SingleOrDefault(x => x.Code == model.Token);
                    if (null == token)
                    { // nincs ilyen token, megyünk a hiba oldalra
                        return RedirectToAction(MVC.Account.ValidateFailure(message: message));
                    }

                    var user = await db.Users.SingleOrDefaultAsync(x => x.Id == token.TargetId);
                    if (null == user)
                    { //nincs ilyen felhasználó, megyünk a hibaoldalra
                        return RedirectToAction(MVC.Account.ValidateFailure(message: message));
                    }

                    var result = await UserManager.RemovePasswordAsync(token.TargetId);
                    if (!result.Succeeded)
                    { //nem sikerült a jelszóváltoztatás, megyünk a hibaoldalra
                        message = result.Errors.FirstOrDefault();
                        return RedirectToAction(MVC.Account.ValidateFailure(message: message));
                    }

                    //TODO. átmenetileg simán mentem a jelszót is a pbx miatt
                    user.Password = model.NewPassword;//alantam mentés

                    result = await UserManager.AddPasswordAsync(token.TargetId, model.NewPassword);
                    if (!result.Succeeded)
                    { //nem sikerült a jelszóváltoztatás, megyünk a hibaoldalra
                        message = result.Errors.FirstOrDefault();
                        return RedirectToAction(MVC.Account.ValidateFailure(message: message));
                    }

                    token.ValidateDate = DateTime.Now;
                    if (user.isSynced) { user.isSynced = false; }
                    await db.SaveChangesAsync();

                    message = "A jelszó beállítása sikerült.";
                    if (!user.isEmailValidated)
                        return RedirectToAction(MVC.Home.MissingEmailValidation());
                    else
                        await SignInAsync(user, isPersistent: false);
                    message = string.Format("{0} {1}", message, "Egyben be is léptél az oldalra.");

                    //email küldése
                    var subject = string.Empty;
                    var addresses = new List<Addressee>();

                    addresses.Add(new Addressee
                    {
                        Email = user.Email,
                        FullName = user.UserName
                    });

                    subject = string.Format("{0}: {1}", "Jelszóváltoztatás sikeres", user.Email);
                    var ied = new ResetPasswordCompletedData
                    {
                        now = DateTime.Now,
                        username = user.UserName,
                        email = user.Email,
                        FullName = string.Format("{0} {1}", user.KontaktUser.FirstName ?? "", user.KontaktUser.LastName ?? "")

                    };
                    var mail = new Message<ResetPasswordCompletedData>
                    {
                        Subject = subject,
                        Data = ied
                    };
                    MessageHelper<ResetPasswordCompletedData>.SendMessageToQueue(mail, addresses, Url);

                    return RedirectToAction(MVC.Account.ValidateSuccess(message: message));

                }
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordCompleted(string message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ResetPasswordCompleted(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("message: {0}", message));
                log.Info("end");
                return View(model: message);
            }
        }

        [Authorize]
        public virtual System.Web.Mvc.JsonResult ListTrafficMonths(GridSettings grid)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ListTrafficMonths"))
            {
                log.Info("begin");
                JQGrid.Helpers.JsonResult result;

                DateTime bgn = DateTime.Now.AddMonths(-2); //cél a jelenlegi és a megelőző 2 hónap forgalma

                var rs0 = db.AccountPeriod
                            .Where(x => x.PeriodEnd >= bgn)
                            .AsEnumerable();

                var rs = rs0.AsQueryable().GridPage(grid, out result);

                result.rows = (from r in rs
                                select new JsonRow
                                {
                                    id = r.Id.ToString(),
                                    cell = new string[] 
                                    {                             
                                    r.Id.ToString()
                                    ,r.PeriodBegin.ToString()
                                    ,r.PeriodEnd.ToString()
                                    }
                                }).ToArray();
            
                log.Info("end");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public virtual System.Web.Mvc.JsonResult ListTrafficCalls(GridSettings grid, string userId, Guid monthId)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ListTrafficCalls"))
            {
                log.Info("begin");
                log.Info(string.Format("monthId: {0}, userId: {1}", monthId, userId));

                JQGrid.Helpers.JsonResult result;

                var pbx = db.PBXExtensionData.Where(x => x.ApplicationUser.Id == userId);//lehet, hogy több van
                var ap = db.AccountPeriod.Find(monthId);

//ez lehet, hogy változni fog teljesen
                var rs0 = db.PBXSession
                            .Where(x => 
                                        x.StartTime >= ap.PeriodBegin && x.StartTime <= ap.PeriodEnd //adott időszak
                                        && x.State != 0 //ez a sessioncreated esemény, ami nem kell
                                   )
                            .Join(pbx, x => x.CallerId, y => y.PhoneNumber.InnerPhoneNumber, (x, y) => x)
                            .SelectMany(x => db.PBXExtensionData.Where(y => y.PhoneNumber.InnerPhoneNumber == x.Destination || y.PhoneNumber.ExternalPhoneNumber == x.Destination),
                                                (x, y) => new { x, y })
                            .SelectMany(x => db.Price.Where(y => y.ValidityBegin <= x.x.StartTime && y.ValidityEnd >= x.x.StartTime),
                                                (x, y) => new { x, 
                                                    Id = x.x.Id,
                                                    StartTime = x.x.StartTime,
                                                    Sum = y.Sum,
                                                    CallerId = x.x.CallerId,
                                                    Destination = x.x.Destination,
                                                    FullName = x.y.ApplicationUser.KontaktUser.FirstName + " " + x.y.ApplicationUser.KontaktUser.LastName,
                                                    TalkDuration = x.x.TalkDuration,
                                                    Netto = Math.Round(y.Sum / 60 * (x.x.TalkDuration.Hours * 3600 + x.x.TalkDuration.Minutes * 60 + x.x.TalkDuration.Seconds + x.x.TalkDuration.Milliseconds / 1000.0), 2),
                                                    VAT = Math.Round(y.VAT * 100, 0),
                                                    Brutto = Math.Round(Math.Round(y.Sum / 60 * (x.x.TalkDuration.Hours * 3600 + x.x.TalkDuration.Minutes * 60 + x.x.TalkDuration.Seconds + x.x.TalkDuration.Milliseconds / 1000.0), 2) * (1 + y.VAT), 2)
                                                })
                            .AsEnumerable();

                foreach (var item in rs0.ToList()) { Debug.WriteLine(item); }

                var rs = rs0.AsQueryable().GridPage(grid, out result);

                result.rows = (from r in rs
                               select new JsonRow
                               {
                                   id = r.Id.ToString(),
                                   cell = new string[] 
                                    {                             
                                    r.Id.ToString()
                                    ,r.StartTime.ToString()
                                    ,r.Sum.ToString()
                                    ,r.CallerId
                                    ,r.Destination
                                    ,r.FullName                                  
                                    ,r.TalkDuration.ToString()
                                    ,r.Netto.ToString()
                                    ,r.VAT.ToString() + " %"
                                    ,r.Brutto.ToString()
                                    }
                               }).ToArray();

                log.Info("end");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize]
        public virtual ActionResult AccountProfileEdit(string Id, string message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("AccountProfileEdit(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("Id: {0}, message: {1}", Id, message));
                var model = new AccountProfileViewModel();
                var user = db.Users.Find(Id);
                model.UserId = Id;
                model.LoginName = user.UserName;
                model.LoginEmail = user.Email;
                var pbx = db.PBXExtensionData.FirstOrDefault(x => x.ApplicationUser.Id == user.Id && !x.isDroped);
                model.ExternalNumber = null == pbx ? "" :
                                        null == pbx.PhoneNumber ? "" : pbx.PhoneNumber.ExternalPhoneNumber;
                model.InnerNumber = null == pbx ? "" :
                                        null == pbx.PhoneNumber ? "" : pbx.PhoneNumber.InnerPhoneNumber;
                if (null != user.KontaktUser)
                {
                    model.FirstName = user.KontaktUser.FirstName;
                    model.LastName = user.KontaktUser.LastName;
                    model.IsSinosz = user.KontaktUser.isSinoszMember;
                    model.SinoszId = user.KontaktUser.SinoszId;
                    model.BirthDate = user.KontaktUser.BirthDate;
                    model.IsRequestCommunication = user.KontaktUser.isCommunicationRequested;
                    model.IsRequestDevice = user.KontaktUser.isDeviceReqested;

                    if (null != user.KontaktUser.DeviceLog)
                    {
                        var rs = user.KontaktUser
                                        .DeviceLog
                                        .Select(x => new
                                        {
                                            DeviceLogDate = x.DeviceLogDate,
                                            DeviceId = x.Devices.DeviceId,
                                            DeviceName = x.Devices.DeviceName
                                        })
                                        .ToArray();
                        model.Devices = new List<string[]>();
                        model.Devices.AddRange(rs.Select(r => new[] {
                            r.DeviceLogDate.ToString()
                            ,r.DeviceId
                            ,r.DeviceName
                        }));
                    }
                    else
                    {
                        model.Devices = null;
                    }
                }
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public virtual async Task<ActionResult> AccountProfileEdit(AccountProfileViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("AccountProfileEdit(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                var user = db.Users.Find(model.UserId); 
                if (ModelState.IsValid)
                {
                    var kontaktuser = db.KontaktUser.Find(user.KontaktUser.Id);
                    //már letiltásra kerültek
                    //if (kontaktuser.FirstName != model.FirstName) { kontaktuser.FirstName = model.FirstName; }
                    //if (kontaktuser.LastName != model.LastName) { kontaktuser.LastName = model.LastName; }
                    //már ez sem kell
                    //if (kontaktuser.isCommunicationRequested != model.IsRequestCommunication) { kontaktuser.isCommunicationRequested = model.IsRequestCommunication; }
                    //if (kontaktuser.isDeviceReqested != model.IsRequestDevice) { kontaktuser.isDeviceReqested = model.IsRequestDevice; } 
                    db.Entry(kontaktuser).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //Megyünk vissza az editre
                    log.Info("end with ok");
                    var message = string.Empty;
                    return RedirectToAction(MVC.Account.AccountProfileEdit(Id: model.UserId, message: message));
                }

                if (null != user.KontaktUser && null != user.KontaktUser.DeviceLog)
                {
                    var rs = user.KontaktUser
                                    .DeviceLog
                                    .Select(x => new
                                    {
                                        DeviceLogDate = x.DeviceLogDate,
                                        DeviceId = x.Devices.DeviceId,
                                        DeviceName = x.Devices.DeviceName
                                    })
                                    .ToArray();
                    model.Devices = new List<string[]>();
                    model.Devices.AddRange(rs.Select(r => new[] {
                            r.DeviceLogDate.ToString()
                            ,r.DeviceId
                            ,r.DeviceName
                        }));
                }
                else
                {
                    model.Devices = null;
                }
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));                
                log.Info("end with validation error");
                model.Refresh(ModelState);
                return View(model);
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangeEmailAddressStart(ChangeEmailAddressStartViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ChangeEmailAddressStart(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                var message = "Köszönjük az információt! Elindítjuk az új E-mail cím megadásához szükséges folyamatot. A további lépésekről e-mail-t küldtünk, amiben minden információ szerepel.";
                log.Info("end");
                return RedirectToAction(MVC.Account.ChangeEmailAddressFinalize(token: string.Empty, message: message));
            }
        }

        [HttpGet]
        public virtual ActionResult ChangeEmailAddressFinalize(string token, string message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ChangeEmailAddressFinalize(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("token: {0}, message: {1}", token, message));
                var model = new ChangeEmailAddressFinalizeViewModel() { Token = token };
                model.Refresh(ModelState);
                log.Info(string.Format("model: {0}",JsonConvert.SerializeObject(model)));
                log.Info("end");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangeEmailAddressFinalize(ChangeEmailAddressFinalizeViewModel model)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ChangeEmailAddressFinalize(Post)"))
            {
                log.Info("begin");
                if (ModelState.IsValid)
                {
                    string message = null;
                    log.Info("end");
                    return RedirectToAction(MVC.Account.ChangeEmailAddressCompleted(message: message));
                }
                model.Refresh(ModelState);
                log.Info(string.Format("end with validation errors: {0}", JsonConvert.SerializeObject(model)));
                return View(model);
            }
        }

        [HttpGet]
        public virtual ActionResult ChangeEmailAddressCompleted(string message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ChangeEmailAddressCompleted(Get)"))
            {
                log.Info("begin");
                log.Info(string.Format("message: {0}", message));
                log.Info("end");
                return View();
            }
        }

        #region Input validation

        /// <summary>
        /// Ellenőrizzük a bevitel érvényességét
        /// Verify Input a Register oldalon
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        public virtual JsonpResult viRegister(RegisterViewModel model, string field)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("viRegister"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}, field: {1}", JsonConvert.SerializeObject(model), field));
                string ename = ModelState.IsValidField(field).ToString();
//TODO béna megoldás átmenetileg, mert nem tudom különben átírni a hibaüzenetet, ami angol
                if (null != ModelState["BirthDate"])
                {
                    if (ModelState["BirthDate"].Errors.Count() > 0)
                    {
                        ModelState["BirthDate"].Errors.Clear();
                        ModelState["BirthDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Születési dátum] mezőben!"));
                    }
                }
//              
                var result = new JsonpResult(new[] 
                { 
                    new 
                    { 
                        valid = ename, 
                        message = string.Join("/n", ModelState[field].Errors.Select(x=>x.ErrorMessage) ) 
                    } 
                });
                log.Info(string.Format("result: {0}", JsonConvert.SerializeObject(result)));
                log.Info("end");
                return result;
            }
        }

        /// <summary>
        /// Ellenőrizzük a bevitel érvényességét
        /// Verify Input a Preregister oldalon
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        public virtual JsonpResult viPreregister(PreRegisterViewModel model, string field)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("viPreregister"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}, field: {1}", JsonConvert.SerializeObject(model), field));
                string ename = ModelState.IsValidField(field).ToString();
//TODO béna megoldás átmenetileg, mert nem tudom különben átírni a hibaüzenetet, ami angol
                if (ModelState["PreBirthDate"].Errors.Count() > 0
                    && !ModelState["PreBirthDate"].Errors.Select(x => x.ErrorMessage).Contains("A(z) [Születési idő] mezőt kötelező kitölteni"))
                {
                    ModelState["PreBirthDate"].Errors.Clear();
                    ModelState["PreBirthDate"].Errors.Add(new ModelError("Érvénytelen dátum a [Születési dátum] mezőben!"));
                }
//
                var result = new JsonpResult(new[] 
                { 
                    new 
                    { 
                        valid = ename, 
                        message = string.Join("/n", ModelState[field].Errors.Select(x=>x.ErrorMessage) ) 
                    } 
                });
                log.Info(string.Format("result: {0}", JsonConvert.SerializeObject(result)));
                log.Info("end");
                return result;
            }
        }

        /// <summary>
        /// Ellenőrizzük a bevitel érvényességét
        /// Verify Input a ResetPasswordStart oldalon
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        public virtual JsonpResult viResetPasswordStart(ResetPasswordStartViewModel model, string field)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("viResetPasswordStart"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}, field: {1}", JsonConvert.SerializeObject(model), field));
                string ename = ModelState.IsValidField(field).ToString();
                var result = new JsonpResult(new[] 
                { 
                    new 
                    { 
                        valid = ename, 
                        message = string.Join("/n", ModelState[field].Errors.Select(x=>x.ErrorMessage) ) 
                    } 
                });
                log.Info(string.Format("result: {0}", JsonConvert.SerializeObject(result)));
                log.Info("end");
                return result;
            }
        }

        /// <summary>
        /// Ellenőrizzük a bevitel érvényességét
        /// Verify Input a Register oldalon
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        public virtual JsonpResult viResetPasswordFinalize(ResetPasswordFinalizeViewModel model, string field)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("viResetPasswordFinalize"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}, field: {1}", JsonConvert.SerializeObject(model), field));
                string ename = ModelState.IsValidField(field).ToString();
                var result = new JsonpResult(new[] 
                { 
                    new 
                    { 
                        valid = ename, 
                        message = string.Join("/n", ModelState[field].Errors.Select(x=>x.ErrorMessage) ) 
                    } 
                });
                log.Info(string.Format("result: {0}", JsonConvert.SerializeObject(result)));
                log.Info("end");
                return result;
            }
        }

        #endregion

        [AllowAnonymous]
        public virtual async Task<ActionResult> MailAgain(string loginname)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("MailAgain(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("loginname: {0}", loginname));

                var user = db.Users.FirstOrDefault(x => x.UserName == loginname && x.isLocked == false);

                if (null != user)
                {
                    //A előregisztrációról email-t küldünk, amivel 
                    //az email címet validáljuk

                    //címzés
                    var addresses = new List<Addressee>();
                    addresses.Add(new Addressee
                    {
                        Email = user.Email,
                        FullName = user.UserName
                    });

                    //token létrehozása
                    var token = new Token(db, TokenTargets.EmailValidation, user.Id);//menti is a db-!
                    log.Info("token létrehozva");

                    //adatok a levélre
                    var ved = new ValidateEmailData
                    {
                        email = user.Email,
                        username = user.UserName,
                        token = token.Code,
                        ValidUntil = token.ValidUntil,
                        fullname = string.Format("{0} {1}", user.KontaktUser.FirstName ?? "", user.KontaktUser.LastName ?? "")
                    };

                    //maga a levél
                    var message = new Message<ValidateEmailData>
                    {
                        //Subject = "Előregisztráció: e-mail cím érvényesítése",
                        Subject = "Regisztráció: e-mail cím érvényesítése",
                        Data = ved
                    };

                    //Küldjük a levelet
                    log.Info("levélküldés kezdete");
                    MessageHelper<ValidateEmailData>.SendMessageToQueue(message, addresses, Url);
                    log.Info("levélküldés vége");

                    //Megyünk a tájékoztató oldalra
                    log.Info("end");
                    return RedirectToAction(MVC.Home.WaitForEmailValidationAfterPreRegistration());
                }
                return null;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Validate(string id)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Validate"))
            {
                log.Info("begin");
                log.Info(string.Format("id: {0}", id));
                string message;
                var token = db.Tokens.SingleOrDefault(x => x.Code == id);
                if (null != token)
                {
                    if (0 >= token.ValidUntil.CompareTo(DateTime.Now))
                    { //érvénytelen token, hibaoldal
                        message = "Az érvényesítőkód érvényessége lejárt!";
                        log.Error(string.Format("end with failure: {0}", message));
                        return RedirectToAction(MVC.Account.ValidateFailure(message));
                    }

                    if (null != token.ValidateDate)
                    { //érvénytelen token, hibaoldal
                        message = "Az érvényesítőkódot már felhasználták, kétszer nem használható!";
                        log.Error(string.Format("end with failure: {0}", message));
                        return RedirectToAction(MVC.Account.ValidateFailure(message));
                    }

                    //switch (token.TokenTarget)
                    //{
                    //    case TokenTargets.EmailValidation:
                    //        //e-mail érvényesítés
                    var user = db.Users.Find(token.TargetId);
                    if (null == user)
                    { // nincs meg a felhasználó, hibaoldal
                        message = "Az érvényesítőkódhoz nem található felhasználó!";
                        log.Error(string.Format("end with failure: {0}", message));
                        return RedirectToAction(MVC.Account.ValidateFailure(message));
                    }
                    token.ValidateDate = DateTime.Now;
                    user.isEmailValidated = true;
                    SinoszUser sinoszuser = db.SinoszUser.FirstOrDefault(x => x.SinoszId == user.KontaktUser.SinoszId && x.BirthDate == user.KontaktUser.BirthDate);
                    user.SinoszUser = sinoszuser;
                    await db.SaveChangesAsync();

                    //beléptetem
                    await SignInAsync(user, isPersistent: false);

                    //címzés
                    var addresses = new List<Addressee>();
                    addresses.Add(new Addressee
                    {
                        Email = user.Email,
                        FullName = user.UserName
                    });

                    //adatok a levélre
                    var wd = new WelcomeData
                    {
                        Email = user.Email,
                        UserId = user.Id,
                        UserName = user.UserName,
                        FullName = string.Format("{0} {1}", user.KontaktUser.FirstName ?? "", user.KontaktUser.LastName ?? "")
                    };

                    //maga a levél
                    var email = new Message<WelcomeData>
                    {
                        Subject = "Üdvözöljük a regisztrált felhasználók között!",
                        Data = wd
                    };

                    //Küldjük a levelet
                    MessageHelper<WelcomeData>.SendMessageToQueue(email, addresses, Url);

                    message = string.Format("Az e-mail cím érvényesítése sikeres: {0}!", user.Email);
                    log.Info(string.Format("end with success: {0}", message));
                    
                    //ApplicationUser auser = user;
                    //return RedirectToAction(MVC.Account.ValidateSuccess(message, user));
                    return RedirectToAction(MVC.Account.ValidateSuccess(message));
                    //    case TokenTargets.PasswordReset:
                    //        //todo: ezt még meg kell majd írni
                    //        message = "Jelszóérvényesítés sikertelen!";
                    //        log.Error(string.Format("end with failure: {0}", JsonConvert.SerializeObject(message)));
                    //        return RedirectToAction(MVC.Account.ValidateFailure(message));
                    //    default:
                    //        //ismeretlen cél, hibaoldal
                    //        message = "Nincs ilyen érvényesítőkód!";
                    //        log.Error(string.Format("end with failure: {0}", message));
                    //        return RedirectToAction(MVC.Account.ValidateFailure(message));
                    //}

                }
                else
                { // nincs ilyen token, hibaoldal
                    message = "Nincs ilyen érvényesítőkód!";
                    log.Error(string.Format("end with failure: {0}", message));
                    return RedirectToAction(MVC.Account.ValidateFailure(message));
                }
            }
        }

        [AllowAnonymous]
        public virtual ViewResult ValidateSuccess(string message)        
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ValidateSuccess"))
            {
                log.Info("begin");
                log.Info(string.Format("message: {0}", message));
                //await SignInAsync(user, isPersistent: false);
                log.Info("end");
                return View(model: message);
            }
        }

        [AllowAnonymous]
        public virtual ViewResult ValidateFailure(string message)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("ValidateFailure"))
            {
                log.Info("begin");
                log.Info(string.Format("message: {0}", message));
                log.Info("end");
                return View(model: message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Dispose"))
            {
                log.Info("begin");
                log.Info(string.Format("disposing: {0}",disposing));
                if (disposing)
                {
                    if (null != UserManager)
                    {
                        UserManager.Dispose();
                        UserManager = null;
                    }

                    if (null != db)
                    {
                        db.Dispose();
                        db = null;
                    }
                }
                log.Info("end");
                base.Dispose(disposing);
            }
        }
    }
}