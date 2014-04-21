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
    /// Tags globalzed
    /// </summary>
    public class TagsGlobal
    {
        public TagsGlobal()
        {
            Id = Guid.NewGuid();
            TagsGLocal = new HashSet<TagsLocal>();
            NewsGlobal = new HashSet<NewsGlobal>();
        }

        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<TagsLocal> TagsGLocal { get; set; }

        public virtual ICollection<NewsGlobal> NewsGlobal { get; set; }
    }

    /// <summary>
    /// tAGS localized
    /// </summary>
    public class TagsLocal
    {
        [Key, Column(Order = 0)]
        public Guid TagsGlobalId { get; set; }
        [ForeignKey("TagsGlobalId")]
        public virtual TagsGlobal TagsGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "[{0}] field is required")]
        public string Name { get; set; }
    }
}