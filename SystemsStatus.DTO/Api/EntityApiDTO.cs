using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SystemsStatus.DTO.Api
{
    /// <summary>
    /// Base class for all Entity types.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [DataContract(Namespace = "")]
    public class EntityApiDTO<TPrimaryKey> : IEntityApiDTO<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [DataMember]
        public TPrimaryKey Id { get; set; }
    }
}
