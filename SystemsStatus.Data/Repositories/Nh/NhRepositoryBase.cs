using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data.Entities;
using NHibernate;
using NHibernate.Linq;

namespace SystemsStatus.Data.Repositories.Nh
{
    /// <summary>
    /// Base class for all repositories those uses NHibernate.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public class NhRepositoryBase<TKey, T> : IRepository<TKey, T> where T : class, IEntity<TKey>
    {
        /// <summary>
        /// NHibernate session object to perform database operations.
        /// </summary>
        private readonly ISession _session;

        public NhRepositoryBase(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public T Insert(T entity)
        {
            try
            {
                _session.Save(entity);
            }
            catch (Exception ex)
            {

            }

            return entity;
        }

        /// <summary>
        /// Inserts new entities.
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Entities</returns>
        public IEnumerable<T> Insert(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                try
                {
                    _session.Save(entity);
                }
                catch (Exception ex)
                {

                }
            }

            return entities;
        }


        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public T Update(T entity)
        {
            try
            {
                _session.Update(entity);
            }
            catch (Exception ex)
            {

            }

            return entity;
        }

        /// <summary>
        /// Updates or saves an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public T SaveOrUpdate(T entity)
        {
            try
            {
                _session.SaveOrUpdate(entity);
            }
            catch (Exception ex)
            {

            }

            return entity;
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public bool Delete(TKey id)
        {
            try
            {
                _session.Delete(_session.Load<T>(id));
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public bool Delete(T entity)
        {

            try
            {
                _session.Delete(entity);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes a collection of entities.
        /// </summary>
        /// <param name="entity">Collection of entities to delete</param>
        public bool Delete(System.Collections.Generic.IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                try
                {
                    _session.Save(entity);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Used to get a IQueryable that is used to retrive object from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        public IQueryable<T> All()
        {
            return _session.Query<T>();
        }

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="expression">LINQ expression used to evaluate and find an entity</param>
        /// <returns>Entity</returns>
        public T FindBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return FilterBy(expression).SingleOrDefault();
        }

        /// <summary>
        /// Used to get an IQueryable that is used to retrieve entities from evaluated LINQ expression.
        /// </summary>
        /// <param name="expression">LINQ expression used to evaluate and find entities</param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        public T FindBy(TKey id)
        {
            return _session.Get<T>(id);
        }

        /// <summary>
        /// Merges entity with existing entity in session
        /// </summary>
        /// <param name="entity">Entity to merge</param>
        /// <returns>Entity after merge</returns>
        public T Merge(T entity)
        {
            return _session.Merge(entity);
        }
    }
}
