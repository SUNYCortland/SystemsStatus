using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Data.Entities
{
    public class Department : Entity<int?>
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public Department()
        {
            this.Services = new HashSet<Service>();
        }
    }
}
