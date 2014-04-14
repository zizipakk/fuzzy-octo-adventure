using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Service
{
    public class PBXService
    {
        ApplicationDbContext context;

        public PBXService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public InterpreterCallerInfo GetInterpreterCallerInfo(string InterpreterName)
        {
            var user = context.Users.Where(u => u.UserName == InterpreterName).SingleOrDefault();
            if (user == null)
            {
                throw new ArgumentException("Invalid interpreter");
            }

            var pbxProfile = context.PBXExtensionData.Where(p => p.ApplicationUser.Id == user.Id && p.isDroped == false).Single();

            string InterpreterExtension = pbxProfile.PhoneNumber.InnerPhoneNumber;

            InterpreterCallerInfo data = context.Database.SqlQuery<InterpreterCallerInfo>(
                "EXECUTE dbo.InterpreterIncomingDialedNumber @InterpreterExtension"
                ,new SqlParameter("InterpreterExtension", InterpreterExtension)
                ).SingleOrDefault < InterpreterCallerInfo>();
            return data;
        }

        public string GetInterpreterExtensionForCall(string callId, string callerExtension)
        {
            var PBXSession = context.PBXSession.Where(s => s.CallerId == callerExtension).OrderByDescending(s => s.StartTime).FirstOrDefault();
            if (PBXSession == null)
                return "";
            return PBXSession.Destination;
        }

    }
}