using Tax.Data.Models;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;

namespace Tax.Portal.Mailers
{ 
    public class UserMailer : MailerBase
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}

        public virtual MvcMailMessage CreateMailMessage<M>(Message<M> message, IEnumerable<Addressee> addressees)
            where M : IMessageData
        {
            ViewData.Model = message.Data;
            return Populate(x =>
            {
                x.From = new MailAddress(message.From);
                x.Subject = message.Subject;
                x.ViewName = (message.Data as IMessageData).TemplateName;
                foreach (var addressee in addressees)
	            {
                    switch (addressee.Position)
	                {
                        case Addressee.Positions.To:
                            x.To.Add(new MailAddress(addressee.Email, addressee.FullName));
                            break;
                        case Addressee.Positions.Cc:
                            x.CC.Add(new MailAddress(addressee.Email, addressee.FullName));
                            break;
                        case Addressee.Positions.Bcc:
                            x.Bcc.Add(new MailAddress(addressee.Email, addressee.FullName));
                            break;
                        default:
                            break;
	                }
	            }

            });
        }
    }

    public static class EmailConstans
    {
        public static string MailFromEmail { get { return @ConfigurationManager.AppSettings["MailFrom.Email"]; } }
        public static string MailFromName { get { return @ConfigurationManager.AppSettings["MailFrom.Name"]; } }
        public const string SystemName = "System";
    }

    public enum EmailTypes
    {
        Welcome, 
        ValidateEmail,
        RulesEmail, 
        ResetPassword,
        ResetPasswordCompleted,
        WithDeviceElectedEmail,
        WithoutDeviceElectedEmail,
        ScheduleClosed
    }


    public interface IMessageData
    {
        string TemplateName { get; }
    }

    public class Message<T>
       where T : IMessageData
    {
        public string Id { get; set; }
        private string _from = EmailConstans.MailFromEmail;
        private string _name = EmailConstans.MailFromName;
        public string From { get { return new MailAddress( _from, _name).ToString(); } /*set;*/ }
        public string Subject { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

        /// <summary>
        /// This can be an anonymous object. It's properties will be used as replacable data in mail templates
        /// </summary>
        public T Data { get; set; }
    }

    public class Addressee
    {
        /// <summary>
        /// This property will be included in tracelinks. Can be anything but unique to the addressee.
        /// </summary>
        public string Id { get; set; }
        public Positions Position { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public enum Positions
        {
            To,Cc,Bcc
        }
    }

    public class WelcomeData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.Welcome.ToString(); } }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
    }

    public class ValidateEmailData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.ValidateEmail.ToString(); } }
        public string email { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public DateTime ValidUntil { get; set; }
        public string fullname { get; set; }
    }

    public class RulesEmailData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.RulesEmail.ToString(); } }
        public string email { get; set; }
        public string username { get; set; }
        public string rolename { get; set; }
        //public string password { get; set; }
        //public string extId { get; set; }
        public string innNumber { get; set; }
        public string extNumber { get; set; }
        public string fullname { get; set; }
        public bool? isDelete { get; set; }
    }

    public class ResetPasswordData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.ResetPassword.ToString(); } }
        public string email { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public DateTime ValidUntil { get; set; }
        public string FullName { get; set; }
    }

    public class ResetPasswordCompletedData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.ResetPasswordCompleted.ToString(); } }
        public string email { get; set; }
        public string username { get; set; }
        public DateTime now { get; set; }
        public string FullName { get; set; }
    }

    public class WithDeviceElectedEmailData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.WithDeviceElectedEmail.ToString(); } }
        //public string email { get; set; }
        //public string username { get; set; }
        public string fullname { get; set; }
        public string organizationname { get; set; }
        public string organizationaddress { get; set; }
    }

    public class WithoutDeviceElectedEmailData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.WithoutDeviceElectedEmail.ToString(); } }
        //public string email { get; set; }
        //public string username { get; set; }
        public string fullname { get; set; }
        public string organizationname { get; set; }
        public string organizationaddress { get; set; }
    }

    public class ScheduleClosedEmailData : IMessageData
    {
        public string TemplateName { get { return EmailTypes.ScheduleClosed.ToString(); } }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
    }
}