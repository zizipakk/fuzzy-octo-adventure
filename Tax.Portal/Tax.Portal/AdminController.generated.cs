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
    public partial class AdminController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AdminController(Dummy d) { }

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
        public virtual System.Web.Mvc.JsonResult ListInterpreterCenters()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListInterpreterCenters);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateInterpreterCenters()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateInterpreterCenters);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult Listpostcodes()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Listpostcodes);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdatePostCodes()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdatePostCodes);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Rule()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Rule);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult ListUsers()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListUsers);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateUsers()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateUsers);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateUsersIsElected()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateUsersIsElected);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult ListUserRoles()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListUserRoles);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateRolesIsInclude()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateRolesIsInclude);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateUserRoles()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateUserRoles);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult ListSystemParameters()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListSystemParameters);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateSystemParameters()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSystemParameters);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UpdateSetupIspublic()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSetupIspublic);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminController Actions { get { return MVC.Admin; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Admin";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Log = "Log";
            public readonly string Trunk = "Trunk";
            public readonly string ListInterpreterCenters = "ListInterpreterCenters";
            public readonly string UpdateInterpreterCenters = "UpdateInterpreterCenters";
            public readonly string Listpostcodes = "Listpostcodes";
            public readonly string UpdatePostCodes = "UpdatePostCodes";
            public readonly string Rule = "Rule";
            public readonly string ListUsers = "ListUsers";
            public readonly string UpdateUsers = "UpdateUsers";
            public readonly string UpdateUsersIsElected = "UpdateUsersIsElected";
            public readonly string ListUserRoles = "ListUserRoles";
            public readonly string UpdateRolesIsInclude = "UpdateRolesIsInclude";
            public readonly string UpdateUserRoles = "UpdateUserRoles";
            public readonly string Setup = "Setup";
            public readonly string ListSystemParameters = "ListSystemParameters";
            public readonly string UpdateSystemParameters = "UpdateSystemParameters";
            public readonly string UpdateSetupIspublic = "UpdateSetupIspublic";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Log = "Log";
            public const string Trunk = "Trunk";
            public const string ListInterpreterCenters = "ListInterpreterCenters";
            public const string UpdateInterpreterCenters = "UpdateInterpreterCenters";
            public const string Listpostcodes = "Listpostcodes";
            public const string UpdatePostCodes = "UpdatePostCodes";
            public const string Rule = "Rule";
            public const string ListUsers = "ListUsers";
            public const string UpdateUsers = "UpdateUsers";
            public const string UpdateUsersIsElected = "UpdateUsersIsElected";
            public const string ListUserRoles = "ListUserRoles";
            public const string UpdateRolesIsInclude = "UpdateRolesIsInclude";
            public const string UpdateUserRoles = "UpdateUserRoles";
            public const string Setup = "Setup";
            public const string ListSystemParameters = "ListSystemParameters";
            public const string UpdateSystemParameters = "UpdateSystemParameters";
            public const string UpdateSetupIspublic = "UpdateSetupIspublic";
        }


        static readonly ActionParamsClass_ListInterpreterCenters s_params_ListInterpreterCenters = new ActionParamsClass_ListInterpreterCenters();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListInterpreterCenters ListInterpreterCentersParams { get { return s_params_ListInterpreterCenters; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListInterpreterCenters
        {
            public readonly string grid = "grid";
        }
        static readonly ActionParamsClass_UpdateInterpreterCenters s_params_UpdateInterpreterCenters = new ActionParamsClass_UpdateInterpreterCenters();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateInterpreterCenters UpdateInterpreterCentersParams { get { return s_params_UpdateInterpreterCenters; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateInterpreterCenters
        {
            public readonly string id = "id";
            public readonly string oper = "oper";
            public readonly string r = "r";
            public readonly string Code = "Code";
        }
        static readonly ActionParamsClass_Listpostcodes s_params_Listpostcodes = new ActionParamsClass_Listpostcodes();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Listpostcodes ListpostcodesParams { get { return s_params_Listpostcodes; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Listpostcodes
        {
            public readonly string grid = "grid";
        }
        static readonly ActionParamsClass_UpdatePostCodes s_params_UpdatePostCodes = new ActionParamsClass_UpdatePostCodes();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdatePostCodes UpdatePostCodesParams { get { return s_params_UpdatePostCodes; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdatePostCodes
        {
            public readonly string id = "id";
            public readonly string oper = "oper";
            public readonly string r = "r";
        }
        static readonly ActionParamsClass_Rule s_params_Rule = new ActionParamsClass_Rule();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Rule RuleParams { get { return s_params_Rule; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Rule
        {
            public readonly string KontaktUserId = "KontaktUserId";
        }
        static readonly ActionParamsClass_ListUsers s_params_ListUsers = new ActionParamsClass_ListUsers();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListUsers ListUsersParams { get { return s_params_ListUsers; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListUsers
        {
            public readonly string grid = "grid";
            public readonly string KontaktUserId = "KontaktUserId";
        }
        static readonly ActionParamsClass_UpdateUsers s_params_UpdateUsers = new ActionParamsClass_UpdateUsers();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateUsers UpdateUsersParams { get { return s_params_UpdateUsers; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateUsers
        {
            public readonly string id = "id";
            public readonly string oper = "oper";
            public readonly string u = "u";
            public readonly string k = "k";
        }
        static readonly ActionParamsClass_UpdateUsersIsElected s_params_UpdateUsersIsElected = new ActionParamsClass_UpdateUsersIsElected();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateUsersIsElected UpdateUsersIsElectedParams { get { return s_params_UpdateUsersIsElected; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateUsersIsElected
        {
            public readonly string UserId = "UserId";
            public readonly string isElected = "isElected";
        }
        static readonly ActionParamsClass_ListUserRoles s_params_ListUserRoles = new ActionParamsClass_ListUserRoles();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListUserRoles ListUserRolesParams { get { return s_params_ListUserRoles; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListUserRoles
        {
            public readonly string grid = "grid";
            public readonly string uid = "uid";
        }
        static readonly ActionParamsClass_UpdateRolesIsInclude s_params_UpdateRolesIsInclude = new ActionParamsClass_UpdateRolesIsInclude();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateRolesIsInclude UpdateRolesIsIncludeParams { get { return s_params_UpdateRolesIsInclude; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateRolesIsInclude
        {
            public readonly string UserId = "UserId";
            public readonly string RoleId = "RoleId";
            public readonly string isInclude = "isInclude";
        }
        static readonly ActionParamsClass_UpdateUserRoles s_params_UpdateUserRoles = new ActionParamsClass_UpdateUserRoles();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateUserRoles UpdateUserRolesParams { get { return s_params_UpdateUserRoles; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateUserRoles
        {
            public readonly string id = "id";
            public readonly string oper = "oper";
            public readonly string UserId = "UserId";
            public readonly string RoleId = "RoleId";
            public readonly string isIncludes = "isIncludes";
            public readonly string InterpreterCenter = "InterpreterCenter";
            public readonly string OrganizationName = "OrganizationName";
        }
        static readonly ActionParamsClass_ListSystemParameters s_params_ListSystemParameters = new ActionParamsClass_ListSystemParameters();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListSystemParameters ListSystemParametersParams { get { return s_params_ListSystemParameters; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListSystemParameters
        {
            public readonly string grid = "grid";
        }
        static readonly ActionParamsClass_UpdateSystemParameters s_params_UpdateSystemParameters = new ActionParamsClass_UpdateSystemParameters();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateSystemParameters UpdateSystemParametersParams { get { return s_params_UpdateSystemParameters; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateSystemParameters
        {
            public readonly string id = "id";
            public readonly string oper = "oper";
            public readonly string r = "r";
        }
        static readonly ActionParamsClass_UpdateSetupIspublic s_params_UpdateSetupIspublic = new ActionParamsClass_UpdateSetupIspublic();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateSetupIspublic UpdateSetupIspublicParams { get { return s_params_UpdateSetupIspublic; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateSetupIspublic
        {
            public readonly string Id = "Id";
            public readonly string isPublic = "isPublic";
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
                public readonly string Log = "Log";
                public readonly string Rule = "Rule";
                public readonly string Setup = "Setup";
                public readonly string Trunk = "Trunk";
            }
            public readonly string Log = "~/Views/Admin/Log.cshtml";
            public readonly string Rule = "~/Views/Admin/Rule.cshtml";
            public readonly string Setup = "~/Views/Admin/Setup.cshtml";
            public readonly string Trunk = "~/Views/Admin/Trunk.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AdminController : Tax.Portal.Controllers.AdminController
    {
        public T4MVC_AdminController() : base(Dummy.Instance) { }

        partial void LogOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Log()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Log);
            LogOverride(callInfo);
            return callInfo;
        }

        partial void TrunkOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Trunk()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Trunk);
            TrunkOverride(callInfo);
            return callInfo;
        }

        partial void ListInterpreterCentersOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid);

        public override System.Web.Mvc.JsonResult ListInterpreterCenters(JQGrid.Helpers.GridSettings grid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListInterpreterCenters);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ListInterpreterCentersOverride(callInfo, grid);
            return callInfo;
        }

        partial void UpdateInterpreterCentersOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, string oper, Tax.Data.Models.InterpreterCenter r, string Code);

        public override System.Web.Mvc.ActionResult UpdateInterpreterCenters(string id, string oper, Tax.Data.Models.InterpreterCenter r, string Code)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateInterpreterCenters);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oper", oper);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "r", r);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Code", Code);
            UpdateInterpreterCentersOverride(callInfo, id, oper, r, Code);
            return callInfo;
        }

        partial void ListpostcodesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid);

        public override System.Web.Mvc.JsonResult Listpostcodes(JQGrid.Helpers.GridSettings grid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Listpostcodes);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ListpostcodesOverride(callInfo, grid);
            return callInfo;
        }

        partial void UpdatePostCodesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, string oper, Tax.Data.Models.Postcode r);

        public override System.Web.Mvc.ActionResult UpdatePostCodes(string id, string oper, Tax.Data.Models.Postcode r)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdatePostCodes);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oper", oper);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "r", r);
            UpdatePostCodesOverride(callInfo, id, oper, r);
            return callInfo;
        }

        partial void RuleOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid? KontaktUserId);

        public override System.Web.Mvc.ActionResult Rule(System.Guid? KontaktUserId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Rule);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "KontaktUserId", KontaktUserId);
            RuleOverride(callInfo, KontaktUserId);
            return callInfo;
        }

        partial void ListUsersOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid, System.Guid? KontaktUserId);

        public override System.Web.Mvc.JsonResult ListUsers(JQGrid.Helpers.GridSettings grid, System.Guid? KontaktUserId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListUsers);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "KontaktUserId", KontaktUserId);
            ListUsersOverride(callInfo, grid, KontaktUserId);
            return callInfo;
        }

        partial void UpdateUsersOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, string oper, Tax.Data.Models.ApplicationUser u, Tax.Data.Models.KontaktUser k);

        public override System.Web.Mvc.ActionResult UpdateUsers(string id, string oper, Tax.Data.Models.ApplicationUser u, Tax.Data.Models.KontaktUser k)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateUsers);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oper", oper);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "u", u);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "k", k);
            UpdateUsersOverride(callInfo, id, oper, u, k);
            return callInfo;
        }

        partial void UpdateUsersIsElectedOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string UserId, bool isElected);

        public override System.Web.Mvc.ActionResult UpdateUsersIsElected(string UserId, bool isElected)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateUsersIsElected);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UserId", UserId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "isElected", isElected);
            UpdateUsersIsElectedOverride(callInfo, UserId, isElected);
            return callInfo;
        }

        partial void ListUserRolesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid, string uid);

        public override System.Web.Mvc.JsonResult ListUserRoles(JQGrid.Helpers.GridSettings grid, string uid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListUserRoles);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "uid", uid);
            ListUserRolesOverride(callInfo, grid, uid);
            return callInfo;
        }

        partial void UpdateRolesIsIncludeOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string UserId, string RoleId, bool isInclude);

        public override System.Web.Mvc.ActionResult UpdateRolesIsInclude(string UserId, string RoleId, bool isInclude)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateRolesIsInclude);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UserId", UserId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "RoleId", RoleId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "isInclude", isInclude);
            UpdateRolesIsIncludeOverride(callInfo, UserId, RoleId, isInclude);
            return callInfo;
        }

        partial void UpdateUserRolesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, string oper, string UserId, string RoleId, string isIncludes, string InterpreterCenter, string OrganizationName);

        public override System.Web.Mvc.ActionResult UpdateUserRoles(string id, string oper, string UserId, string RoleId, string isIncludes, string InterpreterCenter, string OrganizationName)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateUserRoles);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oper", oper);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UserId", UserId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "RoleId", RoleId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "isIncludes", isIncludes);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "InterpreterCenter", InterpreterCenter);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "OrganizationName", OrganizationName);
            UpdateUserRolesOverride(callInfo, id, oper, UserId, RoleId, isIncludes, InterpreterCenter, OrganizationName);
            return callInfo;
        }

        partial void SetupOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Setup()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Setup);
            SetupOverride(callInfo);
            return callInfo;
        }

        partial void ListSystemParametersOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, JQGrid.Helpers.GridSettings grid);

        public override System.Web.Mvc.JsonResult ListSystemParameters(JQGrid.Helpers.GridSettings grid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListSystemParameters);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "grid", grid);
            ListSystemParametersOverride(callInfo, grid);
            return callInfo;
        }

        partial void UpdateSystemParametersOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, string oper, Tax.Data.Models.SystemParameter r);

        public override System.Web.Mvc.ActionResult UpdateSystemParameters(string id, string oper, Tax.Data.Models.SystemParameter r)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSystemParameters);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oper", oper);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "r", r);
            UpdateSystemParametersOverride(callInfo, id, oper, r);
            return callInfo;
        }

        partial void UpdateSetupIspublicOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid Id, bool isPublic);

        public override System.Web.Mvc.ActionResult UpdateSetupIspublic(System.Guid Id, bool isPublic)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateSetupIspublic);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Id", Id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "isPublic", isPublic);
            UpdateSetupIspublicOverride(callInfo, Id, isPublic);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
