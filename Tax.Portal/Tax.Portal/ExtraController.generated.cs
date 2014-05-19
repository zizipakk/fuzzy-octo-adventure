// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Tax.Portal.Controllers
{
    public partial class ExtraController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ExtraController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ExtraController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult ListExtras()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListExtras);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult FlashExtra()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FlashExtra);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Edit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteExtra()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteExtra);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateExtraStatus()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateExtraStatus);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ExtraController Actions { get { return MVC.Extra; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Extra";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Extra";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string ListExtras = "ListExtras";
            public readonly string FlashExtra = "FlashExtra";
            public readonly string Create = "Create";
            public readonly string Edit = "Edit";
            public readonly string DeleteExtra = "DeleteExtra";
            public readonly string UpdateExtraStatus = "UpdateExtraStatus";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string ListExtras = "ListExtras";
            public const string FlashExtra = "FlashExtra";
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string DeleteExtra = "DeleteExtra";
            public const string UpdateExtraStatus = "UpdateExtraStatus";
        }


        static readonly ActionParamsClass_ListExtras s_params_ListExtras = new ActionParamsClass_ListExtras();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListExtras ListExtrasParams { get { return s_params_ListExtras; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListExtras
        {
            public readonly string grid = "grid";
        }
        static readonly ActionParamsClass_FlashExtra s_params_FlashExtra = new ActionParamsClass_FlashExtra();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FlashExtra FlashExtraParams { get { return s_params_FlashExtra; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FlashExtra
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string id = "id";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_DeleteExtra s_params_DeleteExtra = new ActionParamsClass_DeleteExtra();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteExtra DeleteExtraParams { get { return s_params_DeleteExtra; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteExtra
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_UpdateExtraStatus s_params_UpdateExtraStatus = new ActionParamsClass_UpdateExtraStatus();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateExtraStatus UpdateExtraStatusParams { get { return s_params_UpdateExtraStatus; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateExtraStatus
        {
            public readonly string id = "id";
            public readonly string to = "to";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _DetailPartial = "_DetailPartial";
                public readonly string Edit = "Edit";
                public readonly string Index = "Index";
            }
            public readonly string _DetailPartial = "~/Views/Extra/_DetailPartial.cshtml";
            public readonly string Edit = "~/Views/Extra/Edit.cshtml";
            public readonly string Index = "~/Views/Extra/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ExtraController : Tax.Portal.Controllers.ExtraController
    {
        public T4MVC_ExtraController() : base(Dummy.Instance) { }

        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        partial void ListExtrasOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid);

        public override System.Web.Mvc.JsonResult ListExtras(JQGrid.Helpers.GridSettings grid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListExtras);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ListExtrasOverride(callInfo, grid);
            return callInfo;
        }

        partial void FlashExtraOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        public override System.Web.Mvc.ActionResult FlashExtra(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FlashExtra);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            FlashExtraOverride(callInfo, id);
            return callInfo;
        }

        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Create()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            CreateOverride(callInfo);
            return callInfo;
        }

        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tax.Portal.Models.ExtraViewModel model);

        public override System.Web.Mvc.ActionResult Create(Tax.Portal.Models.ExtraViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            CreateOverride(callInfo, model);
            return callInfo;
        }

        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        public override System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            EditOverride(callInfo, id);
            return callInfo;
        }

        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tax.Portal.Models.ExtraViewModel model);

        public override System.Web.Mvc.ActionResult Edit(Tax.Portal.Models.ExtraViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            EditOverride(callInfo, model);
            return callInfo;
        }

        partial void DeleteExtraOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        public override System.Web.Mvc.ActionResult DeleteExtra(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteExtra);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DeleteExtraOverride(callInfo, id);
            return callInfo;
        }

        partial void UpdateExtraStatusOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid? id, string to);

        public override System.Web.Mvc.ActionResult UpdateExtraStatus(System.Guid? id, string to)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateExtraStatus);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "to", to);
            UpdateExtraStatusOverride(callInfo, id, to);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
