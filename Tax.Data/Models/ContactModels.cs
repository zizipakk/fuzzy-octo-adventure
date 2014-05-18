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
        public DateTime? PublishingDate { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Linkedin { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public virtual File Photo { get; set; }
        public virtual ICollection<TagsGlobal> TagsGlobal { get; set; }
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
        public string Department { get; set; }
        public string Position { get; set; }
        public string Profile { get; set; }
    }
}