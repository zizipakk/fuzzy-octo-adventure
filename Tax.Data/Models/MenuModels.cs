using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tax.Data.Models
{

    /// <summary>
    /// Főmenü pontjai
    /// </summary>
    public partial class Menu
    {
        public Menu()
        {
            Id = Guid.NewGuid();
            SubMenu = new HashSet<SubMenu>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool isActive { get; set; }
        public int Position { get; set; }
        public virtual ICollection<SubMenu> SubMenu { get; set; }
    }

    /// <summary>
    /// Almenük pontjai
    /// </summary>
    public partial class SubMenu
    {
        public SubMenu()
        {
            Id = Guid.NewGuid();
            KontaktRole = new HashSet<KontaktRole>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool isActive { get; set; }
        public int Position { get; set; }

        public virtual ICollection<KontaktRole> KontaktRole { get; set; }
        public virtual Menu Menu { get; set; }
    }

}