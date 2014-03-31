using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Data.Entities
{
    /// <summary>
    /// Base class for all Entity types.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }
    }
}
