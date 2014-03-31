using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Core.Data.Entities.Statuses
{
    public class OfflineUnplannedServiceStatus : ServiceStatus
    {
        [Display(Name = "Down Since")]
        public virtual DateTime OfflineSince { get; set; }

        [Display(Name = "Expected Online")]
        public virtual DateTime? ExpectedOnline { get; set; }

        [Display(Name = "Actual Online")]
        public virtual DateTime? ActualOnline { get; set; }

        [Display(Name = "Cause")]
        public virtual string OfflineCause { get; set; }
    }
}
