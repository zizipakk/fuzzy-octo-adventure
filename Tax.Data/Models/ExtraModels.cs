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
    /// Extras globalzed
    /// </summary>
    public class ExtrasGlobal
    {
        public ExtrasGlobal()
        {
            Id = Guid.NewGuid();
            CategoriesGlobal = new HashSet<CategoriesGlobal>();
            ExtrasLocal = new HashSet<ExtrasLocal>();
        }

        [Key]
        public Guid Id { get; set; }
//        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }
//        [Display(Name = "Order")]
        public int Order { get; set; }
//        [Display(Name = "TaCategoriesGlobalgs")]
        public virtual ICollection<CategoriesGlobal> CategoriesGlobal { get; set; }
//        [Display(Name = "Állapot")]
        public virtual NewsStatusesGlobal NewsStatus { get; set; }

        public virtual ICollection<ExtrasLocal> ExtrasLocal { get; set; }
    }

    /// <summary>
    /// News localized model
    /// </summary>
    public class ExtrasLocal
    {
        [Key, Column(Order = 0)]
        public Guid ExtrasGlobalId { get; set; }
        [ForeignKey("ExtrasGlobalId")]
        public virtual ExtrasGlobal ExtrasGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        //[Display(Name = "Title1")]
        //[Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public string Title1 { get; set; }
        //[Display(Name = "Title2")]        
        public string Title2 { get; set; }
        //[Display(Name = "Subtitle")]
        public string Subtitle { get; set; }

        //[Display(Name = "Body text")]
        public string Body_text { get; set; }
    }
}