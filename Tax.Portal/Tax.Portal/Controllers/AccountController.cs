using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
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
                        if (user.isLocked)
                        {
                            return RedirectToAction(MVC.Account.Login());
                        }

                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);   
                    }
                    else
                    {
                        ModelState.AddModelError("", "Access denied. Please contact your system administrator.");
                    }
                }
                // If we got this far, something failed, redisplay form
                model.Refresh(ModelState);
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));
                log.Info("end");
                return View(model);
            }
        }

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
        public virtual async Task<ActionResult> Register(ApplicationUser model, string Password)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Register(Post)"))
            {
                log.Info("begin");
                log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(model)));

                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email,
                    isLocked = false
                };

                //user.Roles readonly, ezért indirekt írása az ApplicationUserRole táblának
                IdentityRole role = db.Roles.SingleOrDefault(x => x.Name == "User"); //Egy ilyen kell, különben balhé
                if (null != role)
                {
                    user.Roles.Add(new IdentityUserRole() { Role = role });
                }

                var result = await UserManager.CreateAsync(user, Password);//ez menti a modeleket
                log.Info("User létrehozva");
                if (!result.Succeeded)
                {
                    log.Info("end with error");
                    return Json(new { success = false, error = true, response = result.Errors });
                }
                
                log.Info("end with ok");
                return Json(new { success = true, error = false, response = ""});
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

                if (hasPassword)
                {
                    if (ModelState.IsValid)
                    {
                        IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {

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
                model.Refresh(ModelState);
                log.Info("end");
                return View(model);
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