using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Tax.Data.Models;
using System.Web.Mvc;


namespace Tax.Portal.Models
{
    /// <summary>
    /// Sinosz-tagok ViewModel
    /// </summary>
    public class ReservationViewModel : BootstrapViewModel
    {
        [Display(Name = "Szabad időpontok")]
        public List<ReservationTime> FreeTimes { get; set; }
        [Display(Name = "Eddigi foglalások")]
        public List<string[]> Reservations { get; set; }        
        public Guid KontaktUserId { get; set; }
        public Guid SelectedReservationTimeId { get; set; }
        public bool isAdd { get; set; }
    }

}