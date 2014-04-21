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
    /// Categories globalzed
    /// </summary>
    public class CategoriesGlobal
    {
        public CategoriesGlobal()
        {
            Id = Guid.NewGuid();
            CategoriesLocal = new HashSet<CategoriesLocal>();
        }

        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Order")]
        [Required(ErrorMessage = "[{0}] field is required")]
        public int Order { get; set; }
        public virtual ICollection<CategoriesLocal> CategoriesLocal { get; set; }
    }

    /// <summary>
    /// Categories localized
    /// </summary>
    public class CategoriesLocal
    {
        [Key, Column(Order = 0)]
        public Guid CategoriesGlobalId { get; set; }
        [ForeignKey("CategoriesGlobalId")]
        public virtual CategoriesGlobal CategoriesGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "[{0}] field is required")]
        public string Name { get; set; }
    }
}