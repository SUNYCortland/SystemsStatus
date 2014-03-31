using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Core.Data.Entities.Statuses
{
    public class OnlineServiceStatus : ServiceStatus
    {
        [Display(Name = "Online Since")]
        public virtual DateTime OnlineSince { get; set; }
    }
}
