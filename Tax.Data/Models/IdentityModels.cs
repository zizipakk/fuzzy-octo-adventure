using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Tax.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {        
        [Display(Name = "Name")]
        [Required(ErrorMessage = "[{0}] field is required")]
        public string Name { get; set; }
        [Display(Name = "Email cím")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "It is locked?")]
        public bool isLocked { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
            SubMenu = new HashSet<SubMenu>();
        }

        public virtual ICollection<SubMenu> SubMenu { get; set; }
    }

}