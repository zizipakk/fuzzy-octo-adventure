using Tax.Portal.Mailers;
using Mvc.Mailer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Tax.Portal.Controllers
{
    public partial class MailerController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /Mailer/
        public virtual ActionResult Index()
        {
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false), AllowAnonymous, /*NonAction, ChildActionOnly*/]
        public virtual ActionResult EmailPost(string m, string a)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("EmailPost"))
            {
                log.Info("begin");
                try
                {
                    MvcMailMessage msg = null;

                    var addresses = JsonConvert.DeserializeObject<IEnumerable<Addressee>>(a);

                    dynamic ms = JObject.Parse(m);
                    string TemplateName = ms.Data.TemplateName;

                    EmailTypes et;

                    if (!Enum.TryParse<EmailTypes>(TemplateName, true, out et))
                    {
                        throw new ArgumentOutOfRangeException("ms.Data.TemplateName");
                    }

                    switch (et)
                    {
                        case EmailTypes.Welcome:
                            {
                                var message = JsonConvert.DeserializeObject<Message<WelcomeData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.ValidateEmail:
                            {
                                var message = JsonConvert.DeserializeObject<Message<ValidateEmailData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.RulesEmail:
                            {
                                var message = JsonConvert.DeserializeObject<Message<RulesEmailData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.ResetPassword:
                            {
                                var message = JsonConvert.DeserializeObject<Message<ResetPasswordData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.ResetPasswordCompleted:
                            {
                                var message = JsonConvert.DeserializeObject<Message<ResetPasswordCompletedData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.WithDeviceElectedEmail:
                            {
                                var message = JsonConvert.DeserializeObject<Message<WithDeviceElectedEmailData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.WithoutDeviceElectedEmail:
                            {
                                var message = JsonConvert.DeserializeObject<Message<WithoutDeviceElectedEmailData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        case EmailTypes.ScheduleClosed:
                            {
                                var message = JsonConvert.DeserializeObject<Message<ScheduleClosedEmailData>>(m);
                                msg = new UserMailer().CreateMailMessage(message, addresses);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("et");
                    }

                    System.Net.Mail.SmtpClient clnt = new SmtpClient();
                    var client = new SmtpClientWrapper(clnt);
                    msg.IsBodyHtml = true;
                    msg.Sender = msg.From;
                    msg.Send(client);
                    msg.Dispose();
                    client.Dispose();
                    log.Info("end");
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        var failedRecipEx = innerEx as SmtpFailedRecipientException;
                        if (failedRecipEx != null)
                        {
                            log.Error(string.Format("Nem sikerült a levélküldés erre az e-mail címre: {0}",ex.FailedRecipient), ex);
                        }
                    }
                    log.Info("end");
                    return null;
                }
                catch (SmtpFailedRecipientException ex)
                {
                    log.Error(string.Format("Nem sikerült a levélküldés erre az e-mail címre: {0}",ex.FailedRecipient), ex);
                    log.Info("end");
                    return null;
                }
                catch (Exception e)
                {
                    log.Error("Nem sikerült a levélküldés", e);
                    log.Info("end");
                    return null;
                }
                log.Info("end");
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }
	}
}