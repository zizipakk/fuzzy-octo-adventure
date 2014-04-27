using System;
using System.Collections.Generic;

namespace Tax.WebAPI.Models
{
    public class ExtrasBindingModel
    {
        public string Id { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Subtitle { get; set; }
        public string Body { get; set; }
        public int Order { get; set; }
        public string Date { get; set; }
        public CategoriesBindingModel Category { get; set; }
    }

}
