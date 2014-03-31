using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using NHibernate;
using SystemsStatus.Dependency.UoW;
using System.Reflection;
using Castle.Facilities.Logging;
using SystemsStatus.Data.Repositories.Nh;
using SystemsStatus.Core.Services.Impl;
using Castle.Core;
using FluentValidation;
using SystemsStatus.Core.Validators;
using SystemsStatus.Core.Parsers.Xml;
using SystemsStatus.Core.Parsers;

namespace SystemsStatus.Dependency
{
    public class SystemsStatusDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            container.AddFacility(new PersistenceFacility());
            
            // Register all controllers
            container.Register(
                // Unitofwork interceptor
                Component.For<NhUnitOfWorkInterceptor>().LifeStyle.PerWebRequest,

                // XML parsers
                Classes.FromAssembly(Assembly.GetAssembly(typeof(DepartmentXmlParser))).BasedOn(typeof(IXmlParser<>)).WithService.Base().LifestyleTransient(),

                // All validators
                Classes.FromAssembly(Assembly.GetAssembly(typeof(ServiceValidator))).BasedOn(typeof(IValidator<>)).WithService.Base().LifestyleTransient(),

                // All repositories
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NhServiceRepository))).InSameNamespaceAs<NhServiceRepository>().WithService.DefaultInterfaces().LifestyleTransient(),

                // All services
                Classes.FromAssembly(Assembly.GetAssembly(typeof(ServiceService))).InSameNamespaceAs<ServiceService>().WithService.DefaultInterfaces().LifestyleTransient()

                );
        }

        void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            //Intercept all methods of all repositories.
            if (UnitOfWorkHelper.IsRepositoryClass(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NhUnitOfWorkInterceptor)));
            }

            //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
            foreach (var method in handler.ComponentModel.Implementation.GetMethods())
            {
                if (UnitOfWorkHelper.HasUnitOfWorkAttribute(method))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NhUnitOfWorkInterceptor)));
                    return;
                }
            }
        }
    }
}
