using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tax.Data.Models
{
     [Table("Logs", Schema = "Log4net")]
     public class Log
     {
         public Log()
         {
             Id = Guid.NewGuid();
         }
         [Key]
         public Guid Id { get; set; }
         [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
         public int Order { get; set; }

         public DateTime Date { get; set; }
         public string Thread { get; set; }
         public string Level { get; set; }
         public string Logger { get; set; }

         public string Message { get; set; }
         public string Exception { get; set; }

     }
}