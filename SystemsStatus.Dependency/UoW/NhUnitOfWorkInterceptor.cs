using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SystemsStatus.Data.Repositories.Nh;
using System.Reflection;
using Castle.Core.Logging;

namespace SystemsStatus.Dependency.UoW
{
    public class NhUnitOfWorkInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly ISession _session;

        /// <summary>
        /// Creates a new NhUnitOfWorkInterceptor object.
        /// </summary>
        /// <param name="sessionFactory">Nhibernate session.</param>
        public NhUnitOfWorkInterceptor(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Intercepts a method.
        /// </summary>
        /// <param name="invocation">Method invocation arguments</param>
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            //If there is a running transaction, just run the method
            if (NhUnitOfWork.Current != null || !RequiresDbConnection(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }

            try
            {
                NhUnitOfWork.Current = new NhUnitOfWork(_session);
                NhUnitOfWork.Current.BeginTransaction();

                try
                {
                    invocation.Proceed();
                    NhUnitOfWork.Current.Commit();
                }
                catch
                {
                    try
                    {
                        NhUnitOfWork.Current.Rollback();
                    }
                    catch
                    {

                    }

                    throw;
                }
            }
            finally
            {
                NhUnitOfWork.Current = null;
            }
        }

        private static bool RequiresDbConnection(MethodInfo methodInfo)
        {
            if (UnitOfWorkHelper.HasUnitOfWorkAttribute(methodInfo))
            {
                return true;
            }

            if (UnitOfWorkHelper.IsRepositoryMethod(methodInfo))
            {
                return true;
            }

            return false;
        }
    }
}
