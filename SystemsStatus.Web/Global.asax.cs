using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using SystemsStatus.Web.Dependency;
using SystemsStatus.Dependency;
using System.Web.Optimization;
using SystemsStatus.Common.Validation;
using FluentValidation.Mvc;
using System.Web.Security;
using System.Web.Script.Serialization;
using SystemsStatus.Web.Areas.Admin.Authentication;
using SystemsStatus.DTO.Api;

namespace SystemsStatus.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private WindsorContainer _windsorContainer;

        protected void Application_Start()
        {
            InitializeWindsor();
            InitializeValidation();

            AreaRegistration.RegisterAllAreas();
            AutoMapperApiConfig.Configure();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UserPrincipalPoco serializeModel = serializer.Deserialize<UserPrincipalPoco>(authTicket.UserData);

                UserPrincipal newUser = new UserPrincipal(authTicket.Name);
                newUser.Id = serializeModel.Id;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.Role = serializeModel.Role;
                newUser.Username = serializeModel.Username;

                HttpContext.Current.User = newUser;
            }
        }

        private void InitializeValidation()
        {
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new WindsorFluentValidatorFactory(_windsorContainer.Kernel);
            });
        }

        private void InitializeWindsor()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(new SystemsStatusDependencyInstaller());
            _windsorContainer.Install(new WebDependencyInstaller());
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_windsorContainer.Kernel);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_windsorContainer.Kernel));
        }
    }
}