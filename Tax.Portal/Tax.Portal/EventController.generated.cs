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
    public partial class EventController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public EventController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected EventController(Dummy d) { }

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
        public virtual System.Web.Mvc.JsonResult ListEvents()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListEvents);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult FlashEvent()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FlashEvent);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Edit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteEvent()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteEvent);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateEventStatus()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateEventStatus);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public EventController Actions { get { return MVC.Event; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Event";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Event";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string ListEvents = "ListEvents";
            public readonly string FlashEvent = "FlashEvent";
            public readonly string Create = "Create";
            public readonly string Edit = "Edit";
            public readonly string DeleteEvent = "DeleteEvent";
            public readonly string UpdateEventStatus = "UpdateEventStatus";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string ListEvents = "ListEvents";
            public const string FlashEvent = "FlashEvent";
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string DeleteEvent = "DeleteEvent";
            public const string UpdateEventStatus = "UpdateEventStatus";
        }


        static readonly ActionParamsClass_ListEvents s_params_ListEvents = new ActionParamsClass_ListEvents();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListEvents ListEventsParams { get { return s_params_ListEvents; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListEvents
        {
            public readonly string grid = "grid";
        }
        static readonly ActionParamsClass_FlashEvent s_params_FlashEvent = new ActionParamsClass_FlashEvent();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_FlashEvent FlashEventParams { get { return s_params_FlashEvent; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_FlashEvent
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
        static readonly ActionParamsClass_DeleteEvent s_params_DeleteEvent = new ActionParamsClass_DeleteEvent();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteEvent DeleteEventParams { get { return s_params_DeleteEvent; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteEvent
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_UpdateEventStatus s_params_UpdateEventStatus = new ActionParamsClass_UpdateEventStatus();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateEventStatus UpdateEventStatusParams { get { return s_params_UpdateEventStatus; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateEventStatus
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
            public readonly string _DetailPartial = "~/Views/Event/_DetailPartial.cshtml";
            public readonly string Edit = "~/Views/Event/Edit.cshtml";
            public readonly string Index = "~/Views/Event/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_EventController : Tax.Portal.Controllers.EventController
    {
        public T4MVC_EventController() : base(Dummy.Instance) { }

        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        partial void ListEventsOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid);

        public override System.Web.Mvc.JsonResult ListEvents(JQGrid.Helpers.GridSettings grid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListEvents);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ListEventsOverride(callInfo, grid);
            return callInfo;
        }

        partial void FlashEventOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        public override System.Web.Mvc.ActionResult FlashEvent(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FlashEvent);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            FlashEventOverride(callInfo, id);
            return callInfo;
        }

        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Create()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            CreateOverride(callInfo);
            return callInfo;
        }

        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tax.Portal.Models.EventViewModel model);

        public override System.Web.Mvc.ActionResult Create(Tax.Portal.Models.EventViewModel model)
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

        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tax.Portal.Models.EventViewModel model);

        public override System.Web.Mvc.ActionResult Edit(Tax.Portal.Models.EventViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            EditOverride(callInfo, model);
            return callInfo;
        }

        partial void DeleteEventOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        public override System.Web.Mvc.ActionResult DeleteEvent(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteEvent);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DeleteEventOverride(callInfo, id);
            return callInfo;
        }

        partial void UpdateEventStatusOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid? id, string to);

        public override System.Web.Mvc.ActionResult UpdateEventStatus(System.Guid? id, string to)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateEventStatus);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "to", to);
            UpdateEventStatusOverride(callInfo, id, to);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
