using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Data.Entities
{
    public class Widget : Entity<int?>
    {
        public virtual int Height { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual User Owner { get; set; }
        public virtual string Name { get; set; }
        public virtual Department Department { get; set; }

        public Widget()
        {
            this.Services = new HashSet<Service>();
            this.Height = 200;
            this.Name = "My Services Widget";
            this.Guid = Guid.NewGuid();
        }
    }
}