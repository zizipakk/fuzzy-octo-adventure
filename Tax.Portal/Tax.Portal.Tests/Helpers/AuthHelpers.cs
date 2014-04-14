using Tax.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Tax.Portal.AcceptanceTests.Helpers
{
    public class fakeContext : ApplicationDbContext
    {
        public fakeContext(){}
        static fakeContext(){}
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder){}
        protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, System.Collections.Generic.IDictionary<object, object> items)
        { return null; }
    }

    public class AuthHelper
    {
        public Mock<UserManager<ApplicationUser>> UserManager { get; set; }
        public Mock<IAuthenticationManager> AuthenticationManager {get; set;}
        public Mock<HttpRequestBase> Request {get; set;}
        public Mock<HttpResponseBase> Response {get; set;}
        public Mock<HttpContextBase> Context {get; set;}
        public RouteCollection Routes {get; set;}

        public AuthHelper()
	    {
            setupUserManager();
            setupAuthenticationManager();
            setupRoutes();
            setupRequest();
            setupResponse();
            setupContext();
	    }

        public void setupUserManager()
        {
            var fakeDbContext = new fakeContext();
            var mockUserStore = new Mock<UserStore<ApplicationUser>>(MockBehavior.Strict, fakeDbContext);
            UserManager = new Mock<UserManager<ApplicationUser>>(MockBehavior.Strict, mockUserStore.Object);
            UserManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns((ApplicationUser user, string password) => 
                    {
                        return Task.Run<IdentityResult>(() =>
                            {
                                return IdentityResult.Success;
                            });
                    });
            UserManager
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
            UserManager
                .Setup(x => x.CreateIdentityAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns((ApplicationUser user, string authenticationType) =>
                {
                    return Task.Run<ClaimsIdentity>(() =>
                    {
                        return new ClaimsIdentity();
                    });
                });
        }

        public void setupAuthenticationManager()
        {
            AuthenticationManager = new Mock<IAuthenticationManager>();
            AuthenticationManager.Setup(x => x.SignOut());
            AuthenticationManager.Setup(x => x.SignIn());
        }

        public void setupRoutes()
        {
            Routes = new RouteCollection();
            RouteConfig.RegisterRoutes(Routes);
        }

        public void setupRequest()
        {
            Request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            Request
                .SetupGet(x => x.ApplicationPath)
                .Returns("/");
            Request
                .SetupGet(x => x.Url)
                .Returns(new Uri("http://localhost/a", UriKind.Absolute));
            Request
                .SetupGet(x => x.ServerVariables)
                .Returns(new System.Collections.Specialized.NameValueCollection());
        }

        public void setupResponse()
        {
            Response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            Response
                .Setup(x => x.ApplyAppPathModifier("/post1"))
                .Returns("http://localhost/post1");
        }

        public void setupContext()
        {
            Context = new Mock<HttpContextBase>(MockBehavior.Strict);
            Context
                .SetupGet(x => x.Request)
                .Returns(Request.Object);
            Context
                .SetupGet(x => x.Response)
                .Returns(Response.Object);
        }
    }
}
