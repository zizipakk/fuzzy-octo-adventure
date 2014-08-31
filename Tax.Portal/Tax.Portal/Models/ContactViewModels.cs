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
    [NotMapped]
    public class MyOrderList
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class ContactViewModel : BootstrapViewModel
    {
        public ContactViewModel()
        {}

        public string Mode { get; set; }
        [NotMapped]
        public List<MyListItem> TagFromList { get; set; }
        [Display(Name = "Tags")]
        [Required(ErrorMessage = "[{0}] is required")]
        public Guid[] TagsIn { get; set; }
        [NotMapped]
        public List<MyListItem> TagToList { get; set; }

        public Guid Id { get; set; }
        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }
        [Display(Name = "Photo")]
        [Required(ErrorMessage = "[{0}] is required")]
        public Guid? PhotoId { get; set; }
        [Display(Name = "Status")]
        public string NewsStatusName { get; set; }
        [Display(Name = "First name")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string First_name { get; set; }
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Last_name { get; set; }
        [Display(Name = "Linkedin")]
        //[Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Linkedin { get; set; }
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Phone { get; set; }
        [Display(Name = "Mobile")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Mobile { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Email { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Department { get; set; }
        [Display(Name = "Position")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Position { get; set; }
        [Display(Name = "Profile")]
        [Required(ErrorMessage = "[{0}] is required")]
        //[AllowHtml]
        public string Profile { get; set; }

        [Display(Name = "Order")]
        [Required(ErrorMessage = "[{0}] is required")]
        public int Order { get; set; }
        [NotMapped]
        public List<MyOrderList> OrderList { get; set; }

    }
}