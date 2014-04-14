using Tax.Data.Models;
using Tax.Portal.AcceptanceTests.Helpers;
using Tax.Portal.Controllers;
using Tax.Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TechTalk.SpecFlow;

namespace Tax.Portal.AcceptanceTests
{
    [Binding]
    public class FelhasznaloiProfilSteps
    {
        private RegisterViewModel rvm;
        private LoginViewModel lvm;
        private AccountController controller;
        private ActionResult actionResult;
        private ResetPasswordStartViewModel rpsvm;
        private ResetPasswordFinalizeViewModel rpfvm;
        private AccountProfileViewModel apvm;
        private ChangeEmailAddressStartViewModel ceavm;
        private ChangeEmailAddressFinalizeViewModel ceafvm;

        [Given(@"a regisztrációs oldal")]
        public void AmennyibenARegisztraciosOldal()
        {
            var fakeDbContext = new fakeContext();
            var mockUserStore = new Mock<UserStore<ApplicationUser>>(MockBehavior.Strict, fakeDbContext);
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(MockBehavior.Strict, mockUserStore.Object);
            mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns((ApplicationUser user, string password) => 
                    {
                        return Task.Run<IdentityResult>(() =>
                            {
                                return IdentityResult.Success;
                            });
                    });
            mockUserManager
                .Setup(x => x.CreateIdentityAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns((ApplicationUser user, string authenticationType) =>
                {
                    return Task.Run<ClaimsIdentity>(() =>
                    {
                        return new ClaimsIdentity();
                    });
                });
            controller = new AccountController(mockUserManager.Object);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());
            mockAuthenticationManager.Setup(x => x.SignIn());
            controller.AuthenticationManager = mockAuthenticationManager.Object;
        }

        [Given(@"a bejelentkezési oldal")]
        public void AmennyibenABejelentkezesiOldal()
        {
            var fakeDbContext = new fakeContext();
            var mockUserStore = new Mock<UserStore<ApplicationUser>>(MockBehavior.Strict, fakeDbContext);
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(MockBehavior.Strict, mockUserStore.Object);
            mockUserManager
                .Setup(x => x.FindAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string user, string password) =>
                {
                    return Task.Run<ApplicationUser>(() =>
                    {
                        return new ApplicationUser 
                        {
                            UserName = "Success",
                        };
                    });
                });
            mockUserManager
                .Setup(x => x.CreateIdentityAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns((ApplicationUser user, string authenticationType) =>
                {
                    return Task.Run<ClaimsIdentity>(() =>
                    {
                        return new ClaimsIdentity();
                    });
                });
            controller = new AccountController(mockUserManager.Object);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());
            mockAuthenticationManager.Setup(x => x.SignIn());
            controller.AuthenticationManager = mockAuthenticationManager.Object;

            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request
                .SetupGet(x => x.ApplicationPath)
                .Returns("/");
            request
                .SetupGet(x => x.Url)
                .Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request
                .SetupGet(x => x.ServerVariables)
                .Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response
                .Setup(x => x.ApplyAppPathModifier("/post1"))
                .Returns("http://localhost/post1");

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context
                .SetupGet(x => x.Request)
                .Returns(request.Object);
            context
                .SetupGet(x => x.Response)
                .Returns(response.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes); 
        }

        [When(@"megadom a '(.*)' és '(.*)' bejelentkezési adatot")]
        public void MajdMegadomAEsBejelentkezesiAdatot(string username, string pw)
        {
            lvm = new LoginViewModel
            {
                UserName = username,
                Password = pw,
                RememberMe = false
            };
            var task = controller.Login(model: lvm, returnUrl: "/Home/Index");
            actionResult = task.Result;
        }

        [Then(@"a rendszer tájékoztat a bejelentkezés '(.*)'-éről")]
        public void AkkorARendszerTajekoztatABejelentkezes_Erol(string rslt)
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectResult));
            var result = (RedirectResult)actionResult;
            Assert.AreEqual(result.Url, "/Home/Index");
        }
        
        [Given(@"a bejelentkezett felhasználó")]
        public void AmennyibenABejelentkezettFelhasznalo()
        {
            var fakeDbContext = new fakeContext();
            var mockUserStore = new Mock<UserStore<ApplicationUser>>(MockBehavior.Strict, fakeDbContext);
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(MockBehavior.Strict, mockUserStore.Object);
            controller = new AccountController(mockUserManager.Object);
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());
            controller.AuthenticationManager = mockAuthenticationManager.Object;
        }
        
        [When(@"megadom a '(.*)' '(.*)' '(.*)' és '(.*)' regisztrációs adatot")]
        public void MajdMegadomAEsRegisztraciosAdatot(string username, string email, string pw1, string pw2)
        {
            rvm = new RegisterViewModel 
            { 
                UserName = username, 
                Email = email,
                Password = pw1, 
                ConfirmPassword = pw2 
            };
            var task = controller.Register(model: rvm);
            actionResult = task.Result;
        }

        [When(@"kijelentkezem")]
        public void MajdKijelentkezem()
        {
            actionResult = controller.LogOff();
        }

        [Then(@"a rendszer tájékoztat a regisztráció '(.*)'-éről")]
        public void AkkorARendszerTajekoztatARegisztracio_Erol(string rslt)
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 3);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

        [Then(@"a rendszer tájékoztat a kijelentkezésről")]
        public void AkkorARendszerTajekoztatAKijelentkezesrol()
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 3); 
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

        [Given(@"a jelszóvisszaállítási oldal")]
        public void AmennyibenAJelszovisszaallitasiOldal()
        {
            var authHelper = new AuthHelper();
            controller = new AccountController(authHelper.UserManager.Object);
            controller.AuthenticationManager = authHelper.AuthenticationManager.Object;
            controller.ControllerContext = new ControllerContext(authHelper.Context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(authHelper.Context.Object, new RouteData()), authHelper.Routes);
        }

        [When(@"megadom a '(.*)'-et")]
        public void MajdMegadomA_Et(string email)
        {
            rpsvm = new ResetPasswordStartViewModel() { Email = email };
        }

        [When(@"kezdeményezem a jelszó visszaállítást")]
        public void MajdKezdemenyezemAJelszoVisszaallitast()
        {
            var task = controller.ResetPasswordStart(rpsvm);
            actionResult = task.Result;
        }

        [Then(@"a rendszer tájékoztat a jelszóvisszaállítás teendőiről")]
        public void AkkorARendszerTajekoztatAJelszovisszaallitasTeendoirol()
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area, a token és a message is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 5);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Account");
            Assert.AreEqual(result.RouteValues["action"], "ResetPasswordFinalize");
        }

        [Given(@"a jelszóvisszaállítási '(.*)'")]
        public void AmennyibenAJelszovisszaallitasi(string token)
        {
            var authHelper = new AuthHelper();
            controller = new AccountController(authHelper.UserManager.Object);
            controller.AuthenticationManager = authHelper.AuthenticationManager.Object;
            controller.ControllerContext = new ControllerContext(authHelper.Context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(authHelper.Context.Object, new RouteData()), authHelper.Routes);
            rpfvm = new ResetPasswordFinalizeViewModel() { Token = token };
        }

        [When(@"megadom a '(.*)' és '(.*)' jelszó adatot")]
        public void MajdMegadomAEsJelszoAdatot(string pw1, string pw2)
        {
            rpfvm.NewPassword = pw1;
            rpfvm.ConfirmPassword = pw2;
        }

        [When(@"megerősítem a jelszó visszaállítást")]
        public void MajdMegerositemAJelszoVisszaallitast()
        {
            var task = controller.ResetPasswordFinalize(rpfvm);
            actionResult = task.Result;
        }

        [Then(@"a rendszer tájékoztat a jelszóvisszaállítás '(.*)'-ről")]
        public void AkkorARendszerTajekoztatAJelszovisszaallitas_Rol(string eredmeny)
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area és a message is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 4);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Account");
            Assert.AreEqual(result.RouteValues["action"], "ResetPasswordCompleted");
        }

        [Given(@"a bejelentkezett felhasználó az emailcím változtatása oldalon")]
        public void AmennyibenABejelentkezettFelhasznaloAzEmailcimValtoztatasaOldalon()
        {
            var authHelper = new AuthHelper();
            controller = new AccountController(authHelper.UserManager.Object);
            controller.AuthenticationManager = authHelper.AuthenticationManager.Object;
            controller.ControllerContext = new ControllerContext(authHelper.Context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(authHelper.Context.Object, new RouteData()), authHelper.Routes);
        }

        [When(@"megadom az '(.*)' email címet")]
        public void MajdMegadomAzEmailCimet(string email)
        {
            ceavm = new ChangeEmailAddressStartViewModel()
                {
                    UserId = "",
                    Email = email
                };
        }

        [When(@"kezdeményzem az email cím megváltoztatását")]
        public void MajdKezdemenyzemAzEmailCimMegvaltoztatasat()
        {
            var task = controller.ChangeEmailAddressStart(ceavm);
            actionResult = task.Result;
        }

        [Then(@"a rendszer tájékoztat az email cím változtatás teendőiről")]
        public void AkkorARendszerTajekoztatAzEmailCimValtoztatasTeendoirol()
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area, token és a message is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 5);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Account");
            Assert.AreEqual(result.RouteValues["action"], "ChangeEmailAddressFinalize");
        }

        [Given(@"az email cím változtatási '(.*)'")]
        public void AmennyibenAzEmailCimValtoztatasi(string token)
        {
            var authHelper = new AuthHelper();
            controller = new AccountController(authHelper.UserManager.Object);
            controller.AuthenticationManager = authHelper.AuthenticationManager.Object;
            controller.ControllerContext = new ControllerContext(authHelper.Context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(authHelper.Context.Object, new RouteData()), authHelper.Routes);
            ceafvm = new ChangeEmailAddressFinalizeViewModel() { Token = token };
        }

        [When(@"megerősítem az email cím változtatást")]
        public void MajdMegerositemAzEmailCimValtoztatast()
        {
            var task = controller.ChangeEmailAddressFinalize(ceafvm);
            actionResult = task.Result;
        }

        [Then(@"a rendszer tájékoztat az email cím változtatás '(.*)'-ről")]
        public void AkkorARendszerTajekoztatAzEmailCimValtoztatas_Rol(string eredmeny)
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area és a message is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 4);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Account");
            Assert.AreEqual(result.RouteValues["action"], "ChangeEmailAddressCompleted");
        }

        [Given(@"a bejelentkezett felhasználó a profiladatok változtatása oldalon")]
        public void AmennyibenABejelentkezettFelhasznaloAProfiladatokValtoztatasaOldalon()
        {
            var authHelper = new AuthHelper();
            controller = new AccountController(authHelper.UserManager.Object);
            controller.AuthenticationManager = authHelper.AuthenticationManager.Object;
            controller.ControllerContext = new ControllerContext(authHelper.Context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(authHelper.Context.Object, new RouteData()), authHelper.Routes);
        }

        [When(@"megadom a '(.*)', '(.*)', '(.*)', '(.*)' és '(.*)' adatot")]
        public void MajdMegadomAEsAdatot(string SinoszTag, string SinoszTagsagiAzonosito, string SzuletesiDatum, string KommunikaciosIgeny, string EszkozIgeny)
        {
            apvm = new AccountProfileViewModel()
            {
                IsSinosz = string.Equals(SinoszTag, "igen", StringComparison.OrdinalIgnoreCase),
                SinoszId = SinoszTagsagiAzonosito,
                BirthDate = DateTime.Parse(SzuletesiDatum),
                IsRequestCommunication = string.Equals(KommunikaciosIgeny, "igen", StringComparison.OrdinalIgnoreCase),
                IsRequestDevice = string.Equals(EszkozIgeny, "igen", StringComparison.OrdinalIgnoreCase),
            };
            var task = controller.AccountProfileEdit(apvm);
            actionResult = task.Result;                      
        }

        [Then(@"a rendszer tájékoztat az adatváltoztatás '(.*)'-éről")]
        public void AkkorARendszerTajekoztatAzAdatvaltoztatas_Erol(string eredmeny)
        {
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            var result = (RedirectToRouteResult)actionResult;
            //jön még az area, Id és a message is, de nem használjuk
            Assert.AreEqual(result.RouteValues.Count, 5);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.AreEqual(result.RouteValues["controller"], "Account");
            Assert.AreEqual(result.RouteValues["action"], "AccountProfileEdit");
        }
    }
}
