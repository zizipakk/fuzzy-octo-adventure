using System;
using System.Collections.Generic;

namespace Tax.WebAPI.Models
{
    public class DevicesBindingModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Type { get; set; }
        public string Lang { get; set; }
    }

}
