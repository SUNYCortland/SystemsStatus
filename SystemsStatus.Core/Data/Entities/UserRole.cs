using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Data.Entities
{
    public class UserRole : Entity<int?>
    {
        public virtual string Name { get; set; }

        public virtual int Level { get; set; } // 1 = Admin, 2 = Service Owner, 3 = IRSC
    }
}
