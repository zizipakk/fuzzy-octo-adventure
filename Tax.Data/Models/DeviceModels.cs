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
    /// Devices
    /// </summary>
    public class Device
    {
        public Device()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(1000)]
        public string Token { get; set; }
        public virtual DeviceType DeviceType { get; set; }
        public virtual Language Language { get; set; }

    }

    /// <summary>
    /// Type of devices
    /// </summary>
    public class DeviceType
    {
        public DeviceType()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
    }

}