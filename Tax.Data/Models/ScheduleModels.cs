using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax.Data.Models
{
    public enum ScheduledActitvity
    {
        InterpreterWork,
        InterpreterOnDuty,
        InterpreterUnavailable,
        InterpreterExternalDuty
    }
    
    public class ScheduleItem
    {
        public ScheduleItem()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public String Description { get; set; }
        public ScheduledActitvity Activity { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

    public class ClosedSchedules
    {
        public ClosedSchedules()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime SchedulePeriodStart { get; set; }
        public DateTime SchedulePeriodEnd { get; set; }

        public DateTime ClosureTimestamp { get; set; }
    }
}
