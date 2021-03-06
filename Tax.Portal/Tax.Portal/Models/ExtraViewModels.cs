﻿using Microsoft.AspNet.Identity.EntityFramework;
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
    public class ExtraViewModel : BootstrapViewModel
    {
        public ExtraViewModel()
        {}

        public string Mode { get; set; }
        [NotMapped]
        public List<MyListItem> CategoryToList { get; set; }

        public Guid Id { get; set; }
        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "[{0}] is required")]
        public Guid CategoryId { get; set; }
        [Display(Name = "Status")]
        public string NewsStatusName { get; set; }
        [Display(Name = "Order")]
        [Required(ErrorMessage = "[{0}] is required")]
        public int? Order { get; set; }

        [Display(Name = "Title1")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title1 { get; set; }
        [Display(Name = "Title2")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title2 { get; set; }
        [Display(Name = "Subtitle")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Subtitle { get; set; }

        [Display(Name = "Body_text")]
        [Required(ErrorMessage = "[{0}] is required")]
        [AllowHtml]
        public string Body_text { get; set; }
    }
}