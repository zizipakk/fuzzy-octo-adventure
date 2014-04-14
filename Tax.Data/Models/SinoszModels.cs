using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Tax.Data.Models
{

    /// <summary>
    /// Sinosz-tagok
    /// </summary>
    public class SinoszUser
    {
        public SinoszUser()
        {
            Id = Guid.NewGuid();
            Address = new HashSet<Address>();
            SinoszLog = new HashSet<SinoszLog>();
            AttachedFile = new HashSet<AttachedFile>();
            AccountingDocument = new HashSet<AccountingDocument>();
        }

        [Key]
        public Guid Id { get; set; }
        //[Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Tagi azonosító")]
        public string SinoszId { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Születési idő")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Név")]
        public string SinoszUserName { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Születési hely")]
        public string BirthPlace { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Anyja neve")]
        public string MothersName { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Lakcím")]
        public string HomeAddress { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Belépés ideje")]
        public DateTime EnterDate { get; set; }
        [Required(ErrorMessage = "A(z) [{0}] mezőt kötelező kitölteni")]
        [Display(Name = "Határozat száma")]
        public string DecreeNumber { get; set; }
        [Display(Name = "Születési név")]
        public string BirthName { get; set; }
        [Display(Name = "Megjegyzés")]
        public string Remark { get; set; }
        [Display(Name = "Képzettség")]
        public string Qualification { get; set; }
        [Display(Name = "Cochleáris implantátum")]
        public bool isImplant { get; set; }
        [Display(Name = "Hallókészülék")]
        public bool isHearingAid { get; set; }

        [Display(Name = "Vonalkód")]
        public string Barcode { get; set; }

        //Dupla nyilvántartás, de gyorstani kell a listáit
        public Guid? LastAccountingDocument { get; set; }
        public Guid? LastCard { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<SinoszLog> SinoszLog { get; set; }
        public virtual ICollection<AccountingDocument> AccountingDocument { get; set; }
        public virtual ICollection<AttachedFile> AttachedFile { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Postcode Postcode { get; set; }
        public virtual Genus Genus { get; set; }
        public virtual HearingStatus HearingStatus { get; set; }
        public virtual PensionType PensionType { get; set; }
        public virtual Nation Nation { get; set; }
        public virtual Position Position { get; set; }
        public virtual MaritalStatus MaritalStatus { get; set; }
        public virtual InjuryTime InjuryTime { get; set; }
        public virtual Education Education { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual SinoszUserStatus SinoszUserStatus { get; set; }
    }

    /// <summary>
    /// Tagszervezet
    /// </summary>
    public class Organization
    {
        public Organization()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }

        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
        public virtual Organization UpperOrganization { get; set; }
        public virtual Postcode Postcode { get; set;  }
    }

    /// <summary>
    /// Beosztás a tagszervezetnél
    /// </summary>
    public class Position
    {
        public Position()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string PositionName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Családi állapot
    /// </summary>
    public class MaritalStatus
    {
        public MaritalStatus()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string MaritalStatusName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Hallássérültség szintje
    /// </summary>
    public class HearingStatus
    {
        public HearingStatus()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string HearingStatusName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Sérülés idejeje
    /// </summary>
    public class InjuryTime
    {
        public InjuryTime()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string InjuryTimeText { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Iskolai végzettség
    /// </summary>
    public class Education
    {
        public Education()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string EducationName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Nem
    /// </summary>
    public class Genus
    {
        public Genus()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string GenusName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Állampolgárság
    /// </summary>
    public class Nation
    {
        public Nation()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string NationText { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Tagviszony
    /// </summary>
    public class Relationship
    {
        public Relationship()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
            AccountingType = new HashSet<AccountingType>();
        }

        [Key]
        public Guid Id { get; set; }
        public string RelationshipName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
        public virtual ICollection<AccountingType> AccountingType { get; set; }
    }

    /// <summary>
    /// Nyugdíjforma
    /// </summary>
    public class PensionType
    {
        public PensionType()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string PensionTypeName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Sinosztag állapota
    /// </summary>
    public class SinoszUserStatus
    {
        public SinoszUserStatus()
        {
            Id = Guid.NewGuid();
            SinoszUser = new HashSet<SinoszUser>();
        }

        [Key]
        public Guid Id { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<SinoszUser> SinoszUser { get; set; }
    }

    /// <summary>
    /// Állapotsorrend
    /// </summary>
    public class StatusToStatus
    {
        public StatusToStatus()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public virtual SinoszUserStatus FromStatus { get; set; }
        public virtual SinoszUserStatus ToStatus { get; set; }
    }

    /// <summary>
    /// Napló
    /// </summary>
    public class SinoszLog
    {
        public SinoszLog()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime ActionTime { get; set; }
        public string ActionName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual SinoszUser SinoszUser { get; set; }
    }

    /// <summary>
    /// Elérhetőség típusa
    /// </summary>
    public class AddressType
    {
        public AddressType()
        {
            Id = Guid.NewGuid();
            Address = new HashSet<Address>();
        }

        [Key]
        public Guid Id { get; set; }
        public string AddressTypeName { get; set; }

        public virtual ICollection<Address> Address { get; set; }
    }

    /// <summary>
    /// Elérhetőség
    /// </summary>
    public class Address
    {
        public Address()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string AddressText { get; set; }

        public virtual SinoszUser SinoszUser { get; set; }
        public virtual AddressType AddressType { get; set; }
    }

    /// <summary>
    /// Hírtípus
    /// </summary>
    public class NewsType
    {
        public NewsType()
        {
            Id = Guid.NewGuid();
            News = new HashSet<News>();
        }

        [Key]
        public Guid Id { get; set; }
        public string NewsTypeName { get; set; }

        public virtual ICollection<News> News { get; set; }
    }

    /// <summary>
    /// Hír
    /// </summary>
    public class News
    {
        public News()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string NewsText { get; set; }
        public DateTime PublishingDate { get; set; }
        public string NewsTitle { get; set; }

        public virtual NewsType NewsType { get; set; }
    }

    /// <summary>
    /// Könyvelési bizonylat típusa
    /// </summary>
    public class AccountingType
    {
        public AccountingType()
        {
            Id = Guid.NewGuid();
            AccountingDocument = new HashSet<AccountingDocument>();            
        }

        [Key]
        public Guid Id { get; set; }
        public string AccountingTypeName { get; set; }
        public bool isFixsum { get; set; }
        public float Presum { get; set; }
        public bool isMembershipCost { get; set; }
        public bool isCardCost { get; set; }
        public bool isEnabled { get; set; }

        public virtual ICollection<AccountingDocument> AccountingDocument { get; set; }
        public virtual Relationship Relationship { get; set; }
    }

    /// <summary>
    /// Könyvelési bizonylat állapota
    /// </summary>
    public class AccountingStatus
    {
        public AccountingStatus()
        {
            Id = Guid.NewGuid();
            AccountingDocument = new HashSet<AccountingDocument>();
        }

        [Key]
        public Guid Id { get; set; }
        public string AccountingStatusName { get; set; }

        public virtual ICollection<AccountingDocument> AccountingDocument { get; set; }
    }

    /// <summary>
    /// Könyvelési bizonylat
    /// </summary>
    public class AccountingDocument
    {
        public AccountingDocument()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumnetNumber { get; set; }
        public float Sum { get; set; }

        public virtual SinoszUser SinoszUser { get; set; }
        public virtual AccountingType AccountingType { get; set; }
        public virtual AccountingStatus AccountingStatus { get; set; }
    }

    /// <summary>
    /// Tagsági igazolvány állapota
    /// </summary>
    public class CardStatus
    {
        public CardStatus()
        {
            Id = Guid.NewGuid();
            Card = new HashSet<Card>();
        }

        [Key]
        public Guid Id { get; set; }
        public string CardStatusName { get; set; }

        public virtual ICollection<Card> Card { get; set; }
    }

    /// <summary>
    /// Igazolvány
    /// </summary>
    public class Card
    {
        public Card()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual SinoszUser SinoszUser { get; set; }
        public virtual CardStatus CardStatus { get; set; }
    }

    public class Comment
    {
        public Comment()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser Interperter { get; set; }
        public virtual ApplicationUser Client { get; set; }
    }

    [DataContract]
    public class Survey
    {
        public Survey()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        [DataMember]
        public string SurveyType { get; set; }
        [DataMember]
        public virtual ICollection<SurveyAnswer> Answers { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    [DataContract]
    public class SurveyAnswer
    {
        public SurveyAnswer()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string Answer { get; set; }

        public virtual Survey Survey { get; set; }
    }

    /// <summary>
    /// Előregisztrációs adatok
    /// </summary>
    public class PreregistrationData
    {
        public PreregistrationData()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public bool IsNeedForJob { get; set; }
        public string Job { get; set; }
        public bool IsNeedForHealth { get; set; }
        public bool IsNeedForLife { get; set; }
 
        public virtual SocialPosition SocialPosition { get; set; }
    }

    /// <summary>
    /// Társadakmi funkciók
    /// </summary>
    public class SocialPosition
    {
        public SocialPosition()
        {
            Id = Guid.NewGuid();
            PreregistrationData = new HashSet<PreregistrationData>();
        }

        [Key]
        public Guid Id { get; set; }
        public string SocialPositionName { get; set; }

        public virtual ICollection<PreregistrationData> PreregistrationData { get; set; }
    }

}