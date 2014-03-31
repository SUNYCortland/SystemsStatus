using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace SystemsStatus.Web.Dependency
{
    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly IKernel container;
        private readonly IDisposable scope;

        public WindsorDependencyScope(IKernel container)
        {
            this.container = container;
            this.scope = container.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            return this.container.HasComponent(serviceType) ? this.container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            this.scope.Dispose();
        }
    }
}