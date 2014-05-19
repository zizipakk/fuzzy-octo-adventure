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
    /// Events globalzed
    /// </summary>
    public class EventsGlobal
    {
        public EventsGlobal()
        {
            Id = Guid.NewGuid();
            EventsLocal = new HashSet<EventsLocal>();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? PublishingDate { get; set; }
        public virtual NewsStatusesGlobal NewsStatus { get; set; }

        public virtual ICollection<EventsLocal> EventsLocal { get; set; }
    }

    /// <summary>
    /// Events localized model
    /// </summary>
    public class EventsLocal
    {
        [Key, Column(Order = 0)]
        public Guid EventsGlobalId { get; set; }
        [ForeignKey("EventsGlobalId")]
        public virtual EventsGlobal EventsGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Body_text { get; set; }
    }
}