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
    /// Messages globalzed
    /// </summary>
    public class MessagesGlobal
    {
        public MessagesGlobal()
        {
            Id = Guid.NewGuid();
            MessagesLocal = new HashSet<MessagesLocal>();
        }

        [Key]
        public Guid Id { get; set; }
//        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }

//        [Display(Name = "Status")]
        public virtual NewsStatusesGlobal NewsStatus { get; set; }

        public virtual ICollection<MessagesLocal> MessagesLocal { get; set; }
    }

    /// <summary>
    /// MessAGES localized
    /// </summary>
    public class MessagesLocal
    {
        [Key, Column(Order = 0)]
        public Guid MessagesGlobalId { get; set; }
        [ForeignKey("MessagesGlobalId")]
        public virtual MessagesGlobal MessagesGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        //        [Display(Name = "Messages")]
        //[Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public string Message { get; set; }
    }
}