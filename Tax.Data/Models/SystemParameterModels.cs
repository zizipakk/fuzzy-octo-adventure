using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tax.Data.Models
{
    [DataContract]
    public class SystemParameter
    {
        public SystemParameter()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Description { get; set; }

        public bool Public { get; set; }

    }

    /// <summary>
    /// Sattus of news globalized
    /// </summary>
    public class NewsStatusesGlobal
    {
        public NewsStatusesGlobal()
        {
            Id = Guid.NewGuid();
            NewsStatusesLocal = new HashSet<NewsStatusesLocal>();
            NewsGlobal = new HashSet<NewsGlobal>();
        }

        [Key]
        public Guid Id { get; set; }
        public string NameGlobal { get; set; }
        public virtual ICollection<NewsStatusesLocal> NewsStatusesLocal { get; set; }

        public virtual ICollection<NewsGlobal> NewsGlobal { get; set; }
    }

    /// <summary>
    /// Status of news localized
    /// </summary>
    public class NewsStatusesLocal
    {
        [Key, Column(Order = 0)]
        public Guid NewsStatusGlobalId { get; set; }
        [ForeignKey("NewsStatusGlobalId")]
        public virtual NewsStatusesGlobal NewsStatusGlobal { get; set; }
        [Key, Column(Order = 1)]
        public Guid LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "[{0}] field is required")]
        public string Name { get; set; }
    }


    public class Language
    {
        public Language()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "[{0}] field is required")]
        public string Name { get; set; }
        [StringLength(2)]        
        public string ShortName { get; set; }
    }

}
