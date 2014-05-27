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
            MessagesLocalDeviceType = new HashSet<MessagesLocalDeviceType>();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime? PublishingDate { get; set; }
        
        public virtual NewsStatusesGlobal NewsStatus { get; set; }
        public virtual ICollection<MessagesLocal> MessagesLocal { get; set; }
        public virtual ICollection<MessagesLocalDeviceType> MessagesLocalDeviceType { get; set; }
    }

    /// <summary>
    /// Messages localized
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

        public string Message { get; set; }
    }

    /// <summary>
    /// Messages - DeviceType
    /// </summary>
    public class MessagesLocalDeviceType
    {
        public MessagesLocalDeviceType()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string ServiceResponse { get; set; }
        public bool isOK { get; set; }

        public virtual MessagesGlobal MessagesGlobal { get; set; }
        public virtual DeviceType DeviceType { get; set; }
    }

}