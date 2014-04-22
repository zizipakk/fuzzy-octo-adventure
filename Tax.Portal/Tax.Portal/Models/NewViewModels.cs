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
   
    public class NewViewModel : BootstrapViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "PublishingDate")]
        public DateTime? PublishingDate { get; set; }
        [Display(Name = "Headline picture")]
        [Required(ErrorMessage = "[{0}] is required")]
        public Guid Headline_pictureId { get; set; }
        [Display(Name = "Thumbnail")]
        [Required(ErrorMessage = "[{0}] is required")]
        public Guid ThumbnailId { get; set; }
        [Display(Name = "Tags")]
        public IEnumerable<TagBindingModel> TagsGlobal { get; set; }
        [Display(Name = "Állapot")]
        public string NewsStatus { get; set; }
        [Display(Name = "Title1")]
        [Required(ErrorMessage = "[{0}] is required")]
        [StringLength(ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title1 { get; set; }
        [Display(Name = "Title2")]
        [StringLength(ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title2 { get; set; }
        [Display(Name = "Subtitle")]
        [StringLength(ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Subtitle { get; set; }
        [Display(Name = "Body text")]
        [Required(ErrorMessage = "[{0}] is required")]
        public string Body_text { get; set; }

    }
}