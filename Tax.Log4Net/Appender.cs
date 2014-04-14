using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;

namespace Kontakt.Log4Net
{
    public class Appender : AppenderSkeleton
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _HostBaseUrl;
        public string HostBaseUrl 
        { 
            get { return _HostBaseUrl; } 
            set { _HostBaseUrl = value; } 
        }

        private string _Action;
        public string Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        private string _ProjectName;
        public string ProjectName
        {
            get { return _ProjectName; }
            set { _ProjectName = value; }
        }

        private string _AreaName;
        public string AreaName
        {
            get { return _AreaName; }
            set { _AreaName = value; }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                var client = new RestClient(this._HostBaseUrl);
                var request = new RestRequest(this.Action, Method.POST);

                request.AddParameter("UserName", "BugSend");
                request.AddParameter("ProjectName", this.ProjectName);
                request.AddParameter("AreaName", this.AreaName);

                var EventInfosArray = loggingEvent.RenderedMessage.Split(new string[] { "(ToExtraInfo:)" }, StringSplitOptions.RemoveEmptyEntries);


                request.AddParameter("Description", string.Format("{0} ({1})",EventInfosArray[0],loggingEvent.GetExceptionString()));

                var properties = string.Empty;
                foreach (DictionaryEntry item in loggingEvent.GetProperties())
	            {
                    properties += (0<properties.Length?",":string.Empty) + string.Format("{0}={1}",item.Key,item.Value);
	            }

                var separator = Environment.NewLine;//string.Format("{0}->", Environment.NewLine);
                var locinfo = string.Join(separator, loggingEvent.LocationInformation.StackFrames.Select(x=>x.FullInfo));

                var extraInfo = 
                    string.Format("Level: {0}{1} Timestamp: {2}{1} UserName: {3}{1} Domain: {4}{1} Exception: {5}{1} Properties: ({6}){1} Identity: {7}{1} Location: {8}{1} Stack: {1}{9}{1} LoggerName: {10}{1} Config: {11}{1} Version: {12}{1} Info: {13}{1}",
                    loggingEvent.Level.DisplayName,
                    Environment.NewLine,
                    loggingEvent.TimeStamp,
                    loggingEvent.UserName,
                    loggingEvent.Domain,
                    loggingEvent.GetExceptionString(),
                    properties,
                    loggingEvent.Identity,
                    loggingEvent.LocationInformation.FullInfo,
                    locinfo,
                    loggingEvent.LoggerName,
                    IsInDebugMode(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8))?"Debug":"Release",
                    this.Version(),
                    1<EventInfosArray.Length
                        ?EventInfosArray[1]
                        :string.Empty
                    );
                System.Diagnostics.Trace.WriteLine(extraInfo);
                request.AddParameter("ExtraInformation", extraInfo);
                request.AddParameter("EmailAddress ", "");
                request.AddParameter("IsForceNewBug", false);
                request.AddParameter("IsFriendlyResponse", false);
                client.ExecuteAsync(request, ExecuteCompleted);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine("An error occurred while connecting to the logging service." + e.ToString());
                base.ErrorHandler.Error("An error occurred while connecting to the logging service.", e);
                throw;
            }
        }

        private void ExecuteCompleted(IRestResponse response)
        {
            if (null!=response.ErrorException)
            {
                System.Diagnostics.Trace.WriteLine("An error occurred while connecting to the funky server." + response.ErrorException.ToString());
                base.ErrorHandler.Error("Error in response.", response.ErrorException);
            }
        }

        private string Version()
        {
            var version = string.Empty;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                version = ad.CurrentVersion.ToString();
            }
            else
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            return string.Format("{0}", version); 
        }

        private bool IsInDebugMode(string FileName)
        {
            return IsInDebugMode(FileName, false);
        }

        private bool IsInDebugMode(string FileName, bool IsAssemlbyName)
        {
            System.Reflection.Assembly assembly;
            if (IsAssemlbyName)
                assembly = System.Reflection.Assembly.Load(FileName);
            else
                assembly = System.Reflection.Assembly.LoadFile(FileName);
            return IsInDebugMode(assembly);
        }

        private bool IsInDebugMode(System.Reflection.Assembly Assembly)
        {
            var attributes = Assembly.GetCustomAttributes(typeof(System.Diagnostics.DebuggableAttribute), false);
            if (attributes.Length > 0)
            {
                var debuggable = attributes[0] as System.Diagnostics.DebuggableAttribute;
                if (debuggable != null)
                    return (debuggable.DebuggingFlags & System.Diagnostics.DebuggableAttribute.DebuggingModes.Default) == System.Diagnostics.DebuggableAttribute.DebuggingModes.Default;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
