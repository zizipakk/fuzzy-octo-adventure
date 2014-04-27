using System;
using System.Collections.Generic;

namespace Tax.WebAPI.Models
{
    public class CategoriesBindingModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }

}
