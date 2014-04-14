using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Models
{
    public class InterpreterCallerInfo
    {
        public bool Fromtax { get; set; }
        public string DialedNumber { get; set; }
        public string CallerId { get; set; }
    }
}