using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.DTO.Api
{
    /// <summary>
    /// Defines interface for base entity type.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public interface IEntityApiDTO<TPrimaryKey>
    {
        /// <summary>
        /// Primary key of the entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
