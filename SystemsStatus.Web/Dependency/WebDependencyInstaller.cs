using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using NHibernate;
using SystemsStatus.Dependency.UoW;
using System.Web.Mvc;
using System.Configuration;
using FluentNHibernate.Cfg;
using SystemsStatus.Data.Repositories.Nh;
using System.Reflection;
using Castle.Core;
using Castle.Facilities.Logging;
using Castle.Core.Logging;
using FluentValidation;
using SystemsStatus.Web.Areas.Admin.Validators;
using System.Web.Http;

namespace SystemsStatus.Web.Dependency
{
    public class WebDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

            //Register all controllers
            container.Register(

                // All validators
                Classes.FromAssembly(Assembly.GetAssembly(typeof(LoginViewModelValidator))).BasedOn(typeof(IValidator<>)).WithService.Base().LifestyleTransient(),

                //All MVC controllers
                Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient(),

                Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped()
                );
        }
    }
}