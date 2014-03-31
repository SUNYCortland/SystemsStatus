using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Core.Data.Entities.Statuses
{
    public class OnlineDegradedServiceStatus : ServiceStatus
    {
        public virtual string Cause { get; set; }

        [Display(Name = "Degraded Since")]
        public virtual DateTime DegradedSince { get; set; }

        [Display(Name = "Expected Normal")]
        public virtual DateTime? ExpectedOnline { get; set; }

        public virtual DateTime? ActualOnline { get; set; }
    }
}
