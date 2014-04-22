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
    /// Contacts globalzed
    /// </summary>
    public class ContactsGlobal
    {
        public ContactsGlobal()
        {
            Id = Guid.NewGuid();
            TagsGlobal = new HashSet<TagsGlobal>();
            ContactsLocal = new HashSet<ContactsLocal>();
        }

        [Key]
        public Guid Id { get; set; }
//        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }
//        [Display(Name = "First name")]
        public string First_name { get; set; }
//        [Display(Name = "Last name")]
        public string Last_name { get; set; }
//        [Display(Name = "Linkedin")]
        public string Linkedin { get; set; }
//        [Display(Name = "Phone")]
        public string Phone { get; set; }
//        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
//        [Display(Name = "E-mail")]
        public string Email { get; set; }

//        [Display(Name = "Photo")]
        public virtual File Photo { get; set; }
//        [Display(Name = "Tags")]
        public virtual ICollection<TagsGlobal> TagsGlobal { get; set; }
//        [Display(Name = "Állapot")]
        public virtual NewsStatusesGlobal NewsStatus { get; set; }

        public virtual ICollection<ContactsLocal> ContactsLocal { get; set; }
    }

    /// <summary>
    /// Contacts localized
    /// </summary>
    public class ContactsLocal
    {
        [Key, Column(Order = 0)]
        public Guid ContactsGlobalId { get; set; }
        [ForeignKey("ContactsGlobalId")]
        public virtual ContactsGlobal ContactsGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        //[Display(Name = "Department")]
        //[Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public string Department { get; set; }
        //[Display(Name = "Position")]        
        public string Position { get; set; }
        //[Display(Name = "Profile")]
        public string Profile { get; set; }
    }
}