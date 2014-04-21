using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.IO;

namespace Tax.Data.Models
{

    /// <summary>
    /// A filetable-t kiegészíti a típussal
    /// </summary>
    public partial class AttachedFile
    {
        public AttachedFile()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
    }


    /// <summary>
    /// A filetable
    /// </summary>
    [Table("Files")] //Rámappelem 
    public partial class File
    {
        [Key]
        public Guid stream_id { get; set; }
        public byte[] file_stream { get; set; }
        public string name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string file_type { get; set; }
    }

}