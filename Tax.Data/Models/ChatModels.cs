using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax.Data.Models
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }

        [NotMapped]
        public string SenderExtension { get; set; }
        [NotMapped]
        public string ReceiverExtension { get; set; }

        public string MessageText { get; set; }

        public DateTime Timestamp { get; set; }
        public DateTime? SentTimestamp { get; set; }
        public DateTime? DeliveredTimetamp { get; set; }
    }
}
