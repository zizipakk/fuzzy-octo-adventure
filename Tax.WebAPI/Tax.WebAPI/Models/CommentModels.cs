using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Models
{
    public class CommentSaveModel
    {
        public string Comment { get; set; }
        public string ClientExtension { get; set; }
    }

    public class CommentQueryResult
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Comment { get; set; }

        public string InterpeterId { get; set; }
        public string ClientId { get; set; }
        public string InterpeterFirstName { get; set; }
        public string InterpeterLastName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
    }
}