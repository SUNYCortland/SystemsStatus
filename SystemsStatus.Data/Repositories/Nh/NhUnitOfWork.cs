using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SystemsStatus.Core.Data;

namespace SystemsStatus.Data.Repositories.Nh
{
    /// <summary>
    /// Implements Unit of work for NHibernate.
    /// </summary>
    public class NhUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets current instance of the NhUnitOfWork.
        /// It gets the right instance that is related to current thread.
        /// </summary>
        public static NhUnitOfWork Current
        {
            get { return _current; }
            set { _current = value; }
        }
        [ThreadStatic]
        private static NhUnitOfWork _current;

        /// <summary>
        /// Reference to the Nhibernate session object to perform queries.
        /// </summary>
        public readonly ISession _session;

        /// <summary>
        /// Reference to the currently running transcation.
        /// </summary>
        private ITransaction _transaction;

        /// <summary>
        /// Creates a new instance of NhUnitOfWork.
        /// </summary>
        /// <param name="session">NHibernate Session</param>
        public NhUnitOfWork(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Opens database connection and begins transaction.
        /// </summary>
        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        /// <summary>
        /// Commits transaction
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// Rollbacks transaction
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
