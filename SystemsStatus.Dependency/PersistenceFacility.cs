using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using NHibernate;
using FluentNHibernate.Cfg;
using SystemsStatus.Data.Repositories.Nh;
using System.Reflection;
using SystemsStatus.Data.Repositories.Nh.Mappings.MsSql;

namespace SystemsStatus.Dependency
{
    public class PersistenceFacility : AbstractFacility
    {

        protected override void Init()
        {
            Kernel.Register(
                //Nhibernate session factory
                Component.For<ISessionFactory>().UsingFactoryMethod(CreateNhSessionFactory).LifeStyle.Singleton,

                //Nhibernate session
                Component.For<ISession>().UsingFactoryMethod(kernel => kernel.Resolve<ISessionFactory>().OpenSession()).LifeStyle.PerWebRequest
            );
        }

        /// <summary>
        /// Creates NHibernate Session Factory.
        /// </summary>
        /// <returns>NHibernate Session Factory</returns>
        private static ISessionFactory CreateNhSessionFactory()
        {
            return Fluently.Configure()
                .Database(NhMsSql2012Configuration.Dialect.ConnectionString(x => x.FromConnectionStringWithKey("DBConnect")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(ServiceMapping))))
                .BuildSessionFactory();
        }
    }
}
