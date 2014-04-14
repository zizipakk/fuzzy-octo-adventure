using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.Portal.Models
{
    public class InterpreterViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AreaName { get; set; }

        public string DisplayName { get { return FirstName + " " + LastName; } }

        static readonly string[] ComplexLetters = { "Cs", "Dzs", "Dz", "Gy", "Ly", "Ny", "Sz", "Ty", "Zs" };
        public string Initial 
        {
            get
            {
                string firstInitial = FirstName[0].ToString();
                string lastInitial = LastName[0].ToString();
                
                foreach(var cl in ComplexLetters)
                {
                    if (FirstName.StartsWith(cl)) {
                        firstInitial = cl;
                    }
                    if (LastName.StartsWith(cl))
                    {
                        lastInitial = cl;
                    }
                }

                return firstInitial + ". " + lastInitial + ".";
            }
        }
    }

    public class ScheduleItemViewModel
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Start { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime End { get; set; }
        public int Activity { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get { return FirstName + " " + LastName; } }
    }

    public class GenerateScheduleRequest
    {
        public int SelectedNumber { get; set; }
        public bool[] SelectedDays { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}