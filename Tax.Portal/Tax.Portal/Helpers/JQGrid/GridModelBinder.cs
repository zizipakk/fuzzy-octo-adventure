using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JQGrid.Helpers
{
    public class GridModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                
                var request = controllerContext.HttpContext.Request;
                return new GridSettings
                {
                    IsSearch = bool.Parse(request["_search"] ?? "false"),
                    PageIndex = int.Parse(request["page"] ?? "1"),
                    PageSize = int.Parse(request["rows"] ?? "10"),
                    SortColumn =  request["sidx"] ?? "",
                    SortOrder = request["sord"] ?? "asc",
                    Where = Filter.Create((request["filters"] ?? "").Length>0 ? 
                                            request["filters"] : 
                                            Convert.ToBoolean(request["_search"] ?? "false") ? 
                                            string.Format("{{\"groupOp\":\"AND\",\"rules\":[{{\"field\":\"{0}\",\"op\":\"{1}\",\"data\":\"{2}\"}}],\"groups\":[]}}"
                                                ,request["searchField"], request["searchOper"], request["searchString"]).Trim() : "")


//filters = 
//    {"groupOp":"AND",
//     "rules":[
//       {"field":"invdate","op":"ge","data":"2007-10-06"},
//       {"field":"invdate","op":"le","data":"2007-10-20"}, 
//       {"field":"name","op":"bw","data":"Client 3"}
//      ]
//    }
                    
//searchField:strResName
//searchString:Form
//searchOper:cn
                };
            }
            catch
            {
                return null;
            }
        }
    }
}