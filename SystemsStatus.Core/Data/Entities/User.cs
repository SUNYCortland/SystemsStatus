using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Core.Data.Entities
{
    /// <summary>
    /// Entity for representing a User of the system
    /// </summary>
    public class User : Entity<int?>
    {
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        public virtual string Username { get; set; }

        public virtual string HashedPassword { get; set; }

        public virtual byte[] PasswordSalt { get; set; }

        public virtual UserRole Role { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual bool Active { get; set; }

        public User()
        {
            this.Services = new HashSet<Service>();
        }
    }
}
