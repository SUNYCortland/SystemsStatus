using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SystemsStatus.Core.Data.Entities
{
    public class ScheduledMaintenance : Entity<int?>
    {
        [Display(Name = "Maintenance Name")]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Service Service { get; set; }

        public virtual DateTime Offline { get; set; }

        [Display(Name = "Expected Online")]
        public virtual DateTime? ExpectedOnline { get; set; }

        public virtual DateTime? Online { get; set; }

        public virtual User ScheduledBy { get; set; }
    }
}
