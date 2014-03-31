using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Core.Data.Entities.Statuses
{
    public class ServiceStatus : Entity<int?>
    {
        [Display(Name = "Status Name")]
        public virtual string Name { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual string Type { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public ServiceStatus()
        {
            this.Services = new HashSet<Service>();
        }
    }
}
