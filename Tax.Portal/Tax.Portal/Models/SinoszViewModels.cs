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
    /// Lenyílók a ViewModelekhez
    /// </summary>
    [NotMapped]
    public class MyListItem
    {
        public Guid? Value { get; set; }
        public string Text { get; set; }
    }

    /// <summary>
    /// Sinosz-tagok ViewModel
    /// </summary>
    public class SinoszUserViewModel : BootstrapViewModel
    {
        public SinoszUser SinoszUserModel { get; set; }        

        public SinoszUserViewModel(SinoszUser SinoszUserModel)
        {
            this.SinoszUserModel = SinoszUserModel;

            OrganizationList = new HashSet<MyListItem>();
            PostcodeList = new HashSet<MyListItem>();
            GenusList = new HashSet<MyListItem>();
            HearingStatusList = new HashSet<MyListItem>();
            PensionTypeList = new HashSet<MyListItem>();
            NationList = new HashSet<MyListItem>();
            PositionList = new HashSet<MyListItem>();
            MaritalStatusList = new HashSet<MyListItem>();
            InjuryTimeList = new HashSet<MyListItem>();
            EducationList = new HashSet<MyListItem>();
            //RelationshipList = new HashSet<MyListItem>();
            SinoszUserStatusList = new HashSet<MyListItem>();
        }

        public SinoszUserViewModel()
        {}

        [Display(Name = "Nyilvántartó szervezet")]
        public ICollection<MyListItem> OrganizationList { get; set; }
        [Display(Name = "Irányítószám, város")]
        public ICollection<MyListItem> PostcodeList { get; set; }
        [Display(Name = "Neme")]
        public ICollection<MyListItem> GenusList { get; set; }
        [Display(Name = "Hallásállapot")]
        public ICollection<MyListItem> HearingStatusList { get; set; }
        [Display(Name = "Nyugdíj")]
        public ICollection<MyListItem> PensionTypeList { get; set; }
        [Display(Name = "Állampolgárság")]
        public ICollection<MyListItem> NationList { get; set; }
        [Display(Name = "Tisztség, funkció")]
        public ICollection<MyListItem> PositionList { get; set; }
        [Display(Name = "Családi állapot")]
        public ICollection<MyListItem> MaritalStatusList { get; set; }
        [Display(Name = "Halláskárosodás időpontja")]
        public ICollection<MyListItem> InjuryTimeList { get; set; }
        [Display(Name = "Végzettsége")]
        public ICollection<MyListItem> EducationList { get; set; }
        //[Display(Name = "Tagviszony")]
        //public ICollection<MyListItem> RelationshipList { get; set; }
        [Display(Name = "Tagsági állapot")]
        public ICollection<MyListItem> SinoszUserStatusList { get; set; }
        [Display(Name = "Állománytípus")]
        public ICollection<MyListItem> FileTypeList { get; set; }

        //a fénykép törléséhez az azonosító
        [Display(Name = "Fénykép")]
        public Guid? fileId { get; set; }

        //a validásláshoz
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public DateTime EnterDate { get; set; }
    }

    /// <summary>
    /// Printing ViewModel
    /// </summary>
    public class PrintViewModel
    {

        public PrintViewModel()
        {
            lstHeaders = new List<string>();
            listRows = new List<string[]>();
        }

        public List<string> lstHeaders { get; set; }
        public ICollection<string[]> listRows { get; set; }
    }
}