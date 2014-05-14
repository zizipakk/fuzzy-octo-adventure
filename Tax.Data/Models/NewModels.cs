using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
//using System.Data.Entity.Infrastructure;

namespace Tax.Data.Models
{

    /// <summary>
    /// News globalzed model
    /// </summary>
    //[Table("NewsGlobals")] //Rámappelem 
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
        public DateTime? PublishingDate { get; set; }
        public virtual File Headline_picture { get; set; }
        public virtual File Thumbnail { get; set; }
        public virtual ICollection<TagsGlobal> TagsGlobal { get; set; }
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

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Subtitle { get; set; }
        public string Body_text { get; set; }
    }

    //a kapcsoló tábla még nem megy data annotetion-nel
    /// <summary>
    /// A kapcsoló táblát létrehozta, de kell a karbantartásához
    /// </summary>
    [Table("TagsGlobalNewsGlobals")] //Rámappelem 
    public partial class TagsGlobalNewsGlobal
    {
        [Key, Column(Order = 0)]
        public Guid TagsGlobal_Id { get; set; }
        [ForeignKey("TagsGlobal_Id")]
        public virtual TagsGlobal TagsGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid NewsGlobal_Id { get; set; }
        [ForeignKey("NewsGlobal_Id")]
        public virtual NewsGlobal NewsGlobal { get; set; }
    }

}