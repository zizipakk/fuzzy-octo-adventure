using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace JQGrid.Helpers
{
    public class JsonResult
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public JsonRow[] rows { get; set; }
    }

    public class JsonRow
    {
        public string id { get; set; }
        public string[] cell;
    }

    public class JsonTranslate
    {
        public string sTextEn { get; set; }
        public string sTextDe { get; set; }
    }

}