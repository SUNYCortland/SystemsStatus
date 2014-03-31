using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel;
using System.Web.Http.Dependencies;

namespace SystemsStatus.Web.Dependency
{
    internal class WindsorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel container;

        public WindsorDependencyResolver(IKernel container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(this.container);
        }

        public object GetService(Type serviceType)
        {
            return this.container.HasComponent(serviceType) ? this.container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose() { }
    }
}