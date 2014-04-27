using System;
using System.Collections.Generic;

namespace Tax.WebAPI.Models
{
    public class ContactsBindingModel
    {
        public string Id { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Profil { get; set; }
        public string ImageURL { get; set; }
        public IEnumerable<PhonenumbersBindingModel> PhoneNumbers { get; set; }
        public string Email { get; set; }
        public IEnumerable<TagsBindingModel> Tags { get; set; }
        public string linkedinURL { get; set; }
    }

    public class PhonenumbersBindingModel
    {
        public string Label { get; set; }
        public string Number { get; set; }
    }

}
