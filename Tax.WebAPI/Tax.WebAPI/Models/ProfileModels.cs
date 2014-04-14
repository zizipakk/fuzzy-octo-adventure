using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Models
{

    public class UserProfile
    {      
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PBXExtension { get; set; }
        public string PBXPassword { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }
    }
}