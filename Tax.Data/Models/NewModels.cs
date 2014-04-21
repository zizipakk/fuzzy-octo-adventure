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
    /// News globalzed model
    /// </summary>
    public class NewsGlobal
    {
        public NewsGlobal()
        {
            Id = Guid.NewGuid();
            TagsGlobal = new HashSet<TagsGlobal>();
            NewsLocal = new HashSet<NewsLocal>();
        }

        [Key]
        public Guid Id { get; set; }
//        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }
//        [Display(Name = "Headline picture")]
        public virtual AttachedFile Headline_picture { get; set; }
//        [Display(Name = "Thumbnail")]
        public virtual AttachedFile Thumbnail { get; set; }
//        [Display(Name = "Tags")]
        public virtual ICollection<TagsGlobal> TagsGlobal { get; set; }
//        [Display(Name = "Állapot")]
        public virtual NewsStatusesGlobal NewsStatus { get; set; }

        public virtual ICollection<NewsLocal> NewsLocal { get; set; }
    }

    /// <summary>
    /// News localized model
    /// </summary>
    public class NewsLocal
    {
        [Key, Column(Order = 0)]
        public Guid NewsGlobalId { get; set; }
        [ForeignKey("NewsGlobalId")]
        public virtual NewsGlobal NewsGlobal { get; set; }
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