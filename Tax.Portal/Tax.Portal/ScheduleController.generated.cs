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
    public partial class ScheduleController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ScheduleController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ScheduleController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult SaveSchedule()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveSchedule);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteSchedule()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteSchedule);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CloseSchedule()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CloseSchedule);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult OpenSchedule()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.OpenSchedule);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GenerateSchedule()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GenerateSchedule);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ScheduleController Actions { get { return MVC.Schedule; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Schedule";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Schedule";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Edit = "Edit";
            public readonly string Display = "Display";
            public readonly string ScheduleItems = "ScheduleItems";
            public readonly string ScheduleItemsPrivate = "ScheduleItemsPrivate";
            public readonly string SaveSchedule = "SaveSchedule";
            public readonly string DeleteSchedule = "DeleteSchedule";
            public readonly string IntereterProfile = "IntereterProfile";
            public readonly string Interpreters = "Interpreters";
            public readonly string CloseSchedule = "CloseSchedule";
            public readonly string OpenSchedule = "OpenSchedule";
            public readonly string GenerateSchedule = "GenerateSchedule";
            public readonly string ClosedSchedulePeriods = "ClosedSchedulePeriods";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Edit = "Edit";
            public const string Display = "Display";
            public const string ScheduleItems = "ScheduleItems";
            public const string ScheduleItemsPrivate = "ScheduleItemsPrivate";
            public const string SaveSchedule = "SaveSchedule";
            public const string DeleteSchedule = "DeleteSchedule";
            public const string IntereterProfile = "IntereterProfile";
            public const string Interpreters = "Interpreters";
            public const string CloseSchedule = "CloseSchedule";
            public const string OpenSchedule = "OpenSchedule";
            public const string GenerateSchedule = "GenerateSchedule";
            public const string ClosedSchedulePeriods = "ClosedSchedulePeriods";
        }


        static readonly ActionParamsClass_SaveSchedule s_params_SaveSchedule = new ActionParamsClass_SaveSchedule();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveSchedule SaveScheduleParams { get { return s_params_SaveSchedule; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveSchedule
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_DeleteSchedule s_params_DeleteSchedule = new ActionParamsClass_DeleteSchedule();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteSchedule DeleteScheduleParams { get { return s_params_DeleteSchedule; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteSchedule
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_CloseSchedule s_params_CloseSchedule = new ActionParamsClass_CloseSchedule();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CloseSchedule CloseScheduleParams { get { return s_params_CloseSchedule; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CloseSchedule
        {
            public readonly string start = "start";
            public readonly string end = "end";
        }
        static readonly ActionParamsClass_OpenSchedule s_params_OpenSchedule = new ActionParamsClass_OpenSchedule();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_OpenSchedule OpenScheduleParams { get { return s_params_OpenSchedule; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_OpenSchedule
        {
            public readonly string start = "start";
            public readonly string end = "end";
        }
        static readonly ActionParamsClass_GenerateSchedule s_params_GenerateSchedule = new ActionParamsClass_GenerateSchedule();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GenerateSchedule GenerateScheduleParams { get { return s_params_GenerateSchedule; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GenerateSchedule
        {
            public readonly string generationParams = "generationParams";
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
                public readonly string Display = "Display";
                public readonly string Edit = "Edit";
            }
            public readonly string Display = "~/Views/Schedule/Display.cshtml";
            public readonly string Edit = "~/Views/Schedule/Edit.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ScheduleController : Tax.Portal.Controllers.ScheduleController
    {
        public T4MVC_ScheduleController() : base(Dummy.Instance) { }

        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Edit()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            EditOverride(callInfo);
            return callInfo;
        }

        partial void DisplayOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Display()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Display);
            DisplayOverride(callInfo);
            return callInfo;
        }

        partial void ScheduleItemsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult ScheduleItems()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ScheduleItems);
            ScheduleItemsOverride(callInfo);
            return callInfo;
        }

        partial void ScheduleItemsPrivateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult ScheduleItemsPrivate()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ScheduleItemsPrivate);
            ScheduleItemsPrivateOverride(callInfo);
            return callInfo;
        }

        partial void SaveScheduleOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tax.Portal.Models.ScheduleItemViewModel model);

        public override System.Web.Mvc.ActionResult SaveSchedule(Tax.Portal.Models.ScheduleItemViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveSchedule);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            SaveScheduleOverride(callInfo, model);
            return callInfo;
        }

        partial void DeleteScheduleOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        public override System.Web.Mvc.ActionResult DeleteSchedule(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteSchedule);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DeleteScheduleOverride(callInfo, id);
            return callInfo;
        }

        partial void IntereterProfileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult IntereterProfile()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.IntereterProfile);
            IntereterProfileOverride(callInfo);
            return callInfo;
        }

        partial void InterpretersOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Interpreters()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Interpreters);
            InterpretersOverride(callInfo);
            return callInfo;
        }

        partial void CloseScheduleOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.DateTime start, System.DateTime end);

        public override System.Web.Mvc.ActionResult CloseSchedule(System.DateTime start, System.DateTime end)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CloseSchedule);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "start", start);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "end", end);
            CloseScheduleOverride(callInfo, start, end);
            return callInfo;
        }

        partial void OpenScheduleOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.DateTime start, System.DateTime end);

        public override System.Web.Mvc.ActionResult OpenSchedule(System.DateTime start, System.DateTime end)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.OpenSchedule);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "start", start);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "end", end);
            OpenScheduleOverride(callInfo, start, end);
            return callInfo;
        }

        partial void GenerateScheduleOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Tax.Portal.Models.GenerateScheduleRequest generationParams);

        public override System.Web.Mvc.ActionResult GenerateSchedule(Tax.Portal.Models.GenerateScheduleRequest generationParams)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GenerateSchedule);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "generationParams", generationParams);
            GenerateScheduleOverride(callInfo, generationParams);
            return callInfo;
        }

        partial void ClosedSchedulePeriodsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult ClosedSchedulePeriods()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ClosedSchedulePeriods);
            ClosedSchedulePeriodsOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
