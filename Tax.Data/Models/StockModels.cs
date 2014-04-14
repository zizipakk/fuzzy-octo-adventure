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
    /// Nyilvántarott eszközök
    /// </summary>
    public class Devices
    {
        public Devices()
        {
            Id = Guid.NewGuid();
            DeviceLog = new HashSet<DeviceLog>();
        }

        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Gyári sorozatszám")]
        public string DeviceId { get; set; }
        [Display(Name = "Nyilvántartási szám")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public string AccountingId { get; set; }
        [Display(Name = "Megnevezés")]
        public string DeviceName { get; set; }


        [Display(Name = "Típus")]
        public virtual DeviceTypes DeviceTypes { get; set; }
        [Display(Name = "Állapot")]
        public virtual DeviceStatus DeviceStatus { get; set; }
        public virtual ICollection<DeviceLog> DeviceLog { get; set; }
    }

    /// <summary>
    /// Eszköztípusok
    /// </summary>
    public class DeviceTypes
    {
        public DeviceTypes()
        {
            Id = Guid.NewGuid();
            Devices = new HashSet<Devices>();
        }

        [Key]
        public Guid Id { get; set; }
        public string DeviceTypeName { get; set; }

        public virtual ICollection<Devices> Devices { get; set; }
    }

    /// <summary>
    /// Eszközstátuszok
    /// </summary>
    public class DeviceStatus
    {
        public DeviceStatus()
        {
            Id = Guid.NewGuid();
            Devices = new HashSet<Devices>();
        }

        [Key]
        public Guid Id { get; set; }
        public string DeviceStatusName { get; set; }

        public virtual ICollection<Devices> Devices { get; set; }
    }

    /// <summary>
    /// Eszközmozgás naplója
    /// </summary>
    public class DeviceLog
    {
        public DeviceLog()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Készletváltozás ideje")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public DateTime DeviceLogDate { get; set; }
        [Display(Name = "Megjegyzés")]
        public string Remark { get; set; }

        public virtual Devices Devices { get; set; }
        public virtual KontaktUser KontaktUser { get; set; }
        public virtual Area Area { get; set; }
    }

    /// <summary>
    /// Időfoglalás időpontja
    /// </summary>
    public class ReservationTime
    {
        public ReservationTime()
        {
            Id = Guid.NewGuid();
            Reservation = new HashSet<Reservation>();
        }

        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Dátum")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public DateTime ReservationDate { get; set; }
        [Display(Name = "Maximális foglalás")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public int ReservationMax { get; set; }
        [Display(Name = "Foglalások száma")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public int ReservationCurrent { get; set; }
        [Display(Name = "Időpont")]
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        public TimeSpan ReservationBegin { get; set; }
        [Display(Name = "Engedélyezve?")]
        public bool isEnabled { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
        
    }

    /// <summary>
    /// Időfoglalás
    /// </summary>
    public class Reservation
    {
        public Reservation()
        {
            Id = Guid.NewGuid();                        
        }

        [Key]
        public Guid Id { get; set; }               
        public bool isPresent { get; set; }

        public virtual ReservationTime ReservationTime { get; set; }
        public virtual KontaktUser KontaktUser { get; set; }

    }

}