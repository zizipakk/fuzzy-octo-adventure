using JQGrid.Helpers;
using Tax.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Tax.Portal.Helpers;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Tax.Portal.Models;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Android;
using PushSharp.Apple;
using PushSharp.Core;
using System.IO;


namespace Tax.Portal.Controllers
{
    public partial class MessageController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Index()
        {
            log.Info("begin");
            log.Info("end");
            return View();
        }

        #region list

        /// <summary>
        /// A karbantartó grid adatforrása
        /// </summary>
        /// <param name="grid">Grid szűrés és lapozás paraméterek</param>
        /// <returns>Grid adatforrás</returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual System.Web.Mvc.JsonResult ListMessages(GridSettings grid)
        {
            JQGrid.Helpers.JsonResult result;

            string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Guid lguid = LocalisationHelpers.GetLanguageId(lid, db);
            Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
            Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);

            var rs = db.MessagesGlobal
                                .SelectMany(x => x.MessagesLocal.Where(y => y.LanguageId == lguidEn)
                                    , (x, y) => new { x, y })
                                .SelectMany(x => x.x.MessagesLocal.Where(y => y.LanguageId == lguidHu)
                                    , (x, y) => new { x, y })
                                .SelectMany(z => z.x.x.NewsStatus.NewsStatusesLocal.Where(v => v.LanguageId == lguid)
                                    , (z, v) => new { z, v })
                                .SelectMany(a => a.z.x.x.MessagesLocalDeviceType.Where(v => v.DeviceType.Name == "Ios").DefaultIfEmpty()
                                    , (a, b) => new { a, b })
                                .SelectMany(a => a.a.z.x.x.MessagesLocalDeviceType.Where(v => v.DeviceType.Name == "Android").DefaultIfEmpty()
                                    , (a, b) => new { a, b })
                        .ToList()
                        .Select(s => new 
                        {
                            Id = s.a.a.z.x.x.Id,
                            Status = s.a.a.v.Name,
                            MessageEn = s.a.a.z.x.y.Message,
                            MessageHu = s.a.a.z.y.Message,
                            PublishingDate = s.a.a.z.x.x.PublishingDate,
                            //Response = s.z.x.x.ServiceResponse
                            OkIos = null == s.a.b ? false : s.a.b.isOK,
                            ServiceResponseIos = null == s.a.b ? "" : s.a.b.ServiceResponse,
                            OkAndriod = null == s.b ? false : s.b.isOK,
                            ServiceResponseAndroid = null == s.b ? "" : s.b.ServiceResponse
                        })
                        .AsQueryable().GridPage(grid, out result);

            result.rows = (from r in rs
                           select new JsonRow
                           {
                               id = r.Id.ToString(),
                               cell = new string[] 
                               {
                                string.Empty
                                ,r.Id.ToString()
                                ,r.Status
                                ,r.MessageEn
                                ,r.MessageHu
                                ,r.PublishingDate.ToString()
                                //,r.Response
                                ,r.OkIos.ToString()
                                ,r.ServiceResponseIos
                                ,r.OkAndriod.ToString()
                                ,r.ServiceResponseAndroid
                               }
                           }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion list 

        #region create

        [HttpPost]
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Create(string messageEn, string messageHu)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Message/Create"))
            {
                log.Info("begin");
                if ("" == messageEn)
                {
                    return Json(new { success = false, error = false, response = "[MessageENG] is required" });
                }
                if ("" == messageHu)
                {
                    return Json(new { success = false, error = false, response = "[MessageHUN] is required" });
                }
                int minlenght = 3;
                if (messageEn.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The MessageENG must be at least {0} characters long.", minlenght) });
                }
                if (messageHu.Length < minlenght)
                {
                    return Json(new { success = false, error = true, response = string.Format("The MessageHUN must be at least {0} characters long.", minlenght) });
                }

                MessagesGlobal resg = db.MessagesGlobal.Create();
                resg.NewsStatus = db.NewsStatusesGlobal.FirstOrDefault(x => x.NameGlobal == "Editing");
                resg.PublishingDate = null;
                //resg.ServiceResponse = null;
                db.Entry(resg).State = EntityState.Added;

                //string lid = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
                MessagesLocal reslEn = db.MessagesLocal.Create();
                reslEn.MessagesGlobal = resg;
                reslEn.Language = db.Language.Find(lguidEn);
                reslEn.Message = messageEn;
                db.Entry(reslEn).State = EntityState.Added;
                Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);
                MessagesLocal reslHu = db.MessagesLocal.Create();
                reslHu.MessagesGlobal = resg;
                reslHu.Language = db.Language.Find(lguidHu);
                reslHu.Message = messageHu;
                db.Entry(reslHu).State = EntityState.Added;
                db.SaveChanges();
                log.Info("end with ok");

                return Json(new { success = true, error = false, response = "" });
            }
        }

        #endregion create

        #region edit

        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult Edit(string id, string oper, string MessageEn, string MessageHu)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("POST: Message/Edit"))
            {
                log.Info("begin");
                Guid gid = Guid.Parse(id);
                
                MessagesGlobal resg;

                switch (oper)
                {
                    case "edit":
                        resg = db.MessagesGlobal.Find(gid);
                        Guid lguidEn = LocalisationHelpers.GetLanguageId("en", db);
                        MessagesLocal reslEn = db.MessagesLocal.FirstOrDefault(x => x.MessagesGlobalId == gid && x.LanguageId == lguidEn);
                        if (reslEn.Message != MessageEn) { reslEn.Message = MessageEn; }
                        Guid lguidHu = LocalisationHelpers.GetLanguageId("hu", db);
                        MessagesLocal reslHu = db.MessagesLocal.FirstOrDefault(x => x.MessagesGlobalId == gid && x.LanguageId == lguidHu);
                        if (reslHu.Message != MessageHu) { reslHu.Message = MessageHu; }
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
                log.Info("end with ok");
                return Json(new { success = true, error = false, response = "" });
            }
        }

        #endregion edit

        #region delete

        /// <summary>
        /// Törlés
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult DeleteMessage(Guid id)
        {
            var mg = db.MessagesGlobal.Find(id);
            if (null != mg)
            {
                //először a lokális cucc a levesbe
                var ml = db.MessagesLocal.Where(x => x.MessagesGlobalId == id);
                foreach (var item in ml.ToList())
                { db.Entry(item).State = EntityState.Deleted; }                
                db.Entry(mg).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = false, response = "Not found" });
        }

        #endregion delete

        #region statusbuttons

        public static string successtext = null;
        public static string errortext = null;
        public static string warningtext = null;

        //Currently it will raise only for android devices
        static void DeviceSubscriptionChanged(object sender,
        string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Nem használjuk: mobil klines fel(le)telepítéskor be(ki)reg a szervizbe és delete(régi token)-put(új token) a webapin
            warningtext += DateTime.Now.ToString() + " warning: " + notification
                            + " oldSubscriptionId: " + oldSubscriptionId
                            + " newSubscriptionId: " + newSubscriptionId;
        }

        //this even raised when a notification is successfully sent
        static void NotificationSent(object sender, INotification notification)
        {
            //successtext for json
            //valamiért kétszer sül el, minden ok
            if (null == successtext || !successtext.Contains(notification.ToString()))
            {
                successtext += DateTime.Now.ToString() + " Ok: " + notification.ToString();
            }
        }

        //this is raised when a notification is failed due to some reason
        static void NotificationFailed(object sender,
        INotification notification, Exception notificationFailureException)
        {
            //errortext for json
            //errortext += DateTime.Now.ToString() + " Error: " + notification + " Cause: " + notificationFailureException;
            warningtext += DateTime.Now.ToString() + " Warning: " + notification + " Cause: " + notificationFailureException;
        }

        //this is fired when there is exception is raised by the channel
        static void ChannelException
            (object sender, IPushChannel channel, Exception exception)
        {
            //network errortext for json
            errortext += DateTime.Now.ToString() + " Error: " + channel + " Cause: " + exception;
        }

        //this is fired when there is exception is raised by the service
        static void ServiceException(object sender, Exception exception)
        {
            //service errortext for json
            errortext += DateTime.Now.ToString() + " Error: " + sender + " Cause: " + exception;
        }

        //this is raised when the particular device subscription is expired
        static void DeviceSubscriptionExpired(object sender,
        string expiredDeviceSubscriptionId,
            DateTime timestamp, INotification notification)
        {
            //lejárt token errortext for json
            warningtext = DateTime.Now.ToString() + " Warning: " + notification + " Token: " + expiredDeviceSubscriptionId;
        }

        //this is raised when the channel is destroyed
        static void ChannelDestroyed(object sender)
        {
            //network crash errortext for json
            errortext += DateTime.Now.ToString() + " Error - channel destroyed: " + sender;
        }

        //this is raised when the channel is created
        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //network init successtext for json
            successtext += DateTime.Now.ToString() + " Ok initialized: " + pushChannel;
        }
        
        /// <summary>
        /// státuszgombok
        /// </summary>
        /// <param name="id"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [Authorize(Roles = "SysAdmin, User")]
        public virtual ActionResult UpdateMessageStatus(Guid? id, string to)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("Message/UpdateMessageStatus"))
            {
                log.Info("begin");
                var sta = db.NewsStatusesGlobal.Where(x => x.NameGlobal == to).FirstOrDefault();
                if (null != id && null != sta)
                {
                    var m = db.MessagesGlobal.Find(id);
                    if (null != m)
                    {
                        var l = m.MessagesLocal;
                        if (l.Count != 0 && to == "Published")
                        {
                            //napló szervizenként, ha még nincs
                            var getmd = m.MessagesLocalDeviceType.Select(y => y.DeviceType.Id).ToList();
                            foreach (var dt in db.DeviceType.Where(x => !getmd.Contains(x.Id)))
                            {
                                var mgdt = db.MessagesLocalDeviceType.Create();
                                mgdt.MessagesGlobal = m;
                                mgdt.DeviceType = dt;
                                mgdt.isOK = false;
                                mgdt.ServiceResponse = null;
                                db.Entry(mgdt).State = EntityState.Added;
                            }
                            db.SaveChanges();

                            //create the puchbroker object
                            var push = new PushBroker();
                            //Wire up the events for all the services that the broker registers
                            push.OnNotificationSent += NotificationSent;
                            push.OnChannelException += ChannelException;
                            push.OnServiceException += ServiceException;
                            push.OnNotificationFailed += NotificationFailed;
                            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
                            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
                            push.OnChannelCreated += ChannelCreated;
                            push.OnChannelDestroyed += ChannelDestroyed;

                            var md = m.MessagesLocalDeviceType.Where(x => x.MessagesGlobal == m);
                            var mdIOS = md.FirstOrDefault(x => x.DeviceType.Name == "Ios");
                            var devIOS = db.Device.Where(x => x.DeviceType.Name == "Ios").ToList();
                            if (!mdIOS.isOK && devIOS.Count() > 0) //még nem ment ki ré üzenet sikeresen, és egyáltalán vannak eszközök
                            {
                                foreach (Device row in devIOS)
                                {
                                    var lm = l.FirstOrDefault(x => x.LanguageId == row.Language.Id);
                                    if (null != lm) //ha nincs lokalizált szöveg, itt sem vagyunk
                                    {
                                        //-------------------------
                                        // APPLE NOTIFICATIONS
                                        //-------------------------
                                        //Configure and start Apple APNS
                                        // IMPORTANT: Make sure you use the right Push certificate.  Apple allows you to
                                        //generate one for connecting to Sandbox, and one for connecting to Production.  You must
                                        // use the right one, to match the provisioning profile you build your
                                        //   app with!

                                        //IMPORTANT: If you are using a Development provisioning Profile, you must use
                                        // the Sandbox push notification server 
                                        //  (so you would leave the first arg in the ctor of ApplePushChannelSettings as
                                        // 'false')
                                        //  If you are using an AdHoc or AppStore provisioning profile, you must use the 
                                        //Production push notification server
                                        //  (so you would change the first arg in the ctor of ApplePushChannelSettings to 
                                        //'true')
                                        var prod = db.SystemParameter.FirstOrDefault(x => x.Name == "IOS Production");
                                        bool isProd = false;
                                        string certPath = "~/Resources/taxandlegalpushdevpublic.p12";
                                        if (null != prod && prod.Value == "true")
                                        {
                                            isProd = true;
                                            certPath = "~/Resources/taxandlegalpushprodpublic.p12";
                                        }
                                        var appleCert = System.IO.File.ReadAllBytes(Server.MapPath(certPath));
                                        push.RegisterAppleService(new ApplePushChannelSettings(isProd, appleCert, ""));//nincs pw
                                        //Extension method
                                        //Fluent construction of an iOS notification
                                        //IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets
                                        // generated within your iOS app itself when the Application Delegate
                                        //  for registered for remote notifications is called, 
                                        // and the device token is passed back to you
                                        push.QueueNotification(new AppleNotification()
                                                                    .ForDeviceToken(row.Token)//the recipient device id
                                                                    .WithAlert(lm.Message)//the message
                                                                    .WithBadge(1)
                                            //.WithSound("")
                                                                    );
                                    }
                                }
                                push.StopAllServices(waitForQueuesToFinish: true);
                                //validálunk
                                if (errortext == null)
                                {
                                    mdIOS.isOK = true;  
                                }
                                mdIOS.ServiceResponse =
                                    string.Format("{0}{1}{2}{3}", null == errortext ? "" : errortext,
                                                                    null == warningtext ? "" : (null == errortext ? "" : " / ") + warningtext,
                                                                    null == successtext ? "" : (null == warningtext ? "" : " / ") + successtext,
                                                                    null == mdIOS.ServiceResponse ? "" : (null == successtext ? "" : " | ") + mdIOS.ServiceResponse); //régieket a végére
                                errortext = null;
                                warningtext = null;
                                successtext = null;
                            }
                            if (devIOS.Count() == 0)
                            {
                                mdIOS.ServiceResponse = string.Format("{0}{1}", 
                                                            DateTime.Now.ToString() + " No IOS device",
                                                            null == mdIOS.ServiceResponse ? "" :  " | " + mdIOS.ServiceResponse); //régieket a végére
                            }

                            var mdANDROID = md.FirstOrDefault(x => x.DeviceType.Name == "Android");
                            var devANDROID = db.Device.Where(x => x.DeviceType.Name == "Android").ToList();
                            if (!mdANDROID.isOK && devANDROID.Count() > 0) //még nem ment ki ré üzenet sikeresen, és egyáltalán vannak eszközök
                            {
                                foreach (Device row in db.Device.Where(x => x.DeviceType.Name == "Android").ToList())
                                {
                                    var lm = l.FirstOrDefault(x => x.LanguageId == row.Language.Id);
                                    if (null != lm) //ha nincs lokalizált szöveg, itt sem vagyunk
                                    {
                                        //---------------------------
                                        // ANDROID GCM NOTIFICATIONS
                                        //---------------------------
                                        //Configure and start Android GCM
                                        //IMPORTANT: The API KEY comes from your Google APIs Console App, 
                                        //under the API Access section, 
                                        //  by choosing 'Create new Server key...'
                                        //  You must ensure the 'Google Cloud Messaging for Android' service is 
                                        //enabled in your APIs Console
                                        push.RegisterGcmService(new
                                            GcmPushChannelSettings("AIzaSyBlPPV4mQKZveyqjoMlhJVTrbbMDS1frC4"));
                                        //Fluent construction of an Android GCM Notification
                                        //IMPORTANT: For Android you MUST use your own RegistrationId 
                                        //here that gets generated within your Android app itself!
                                        push.QueueNotification(new GcmNotification()
                                            .ForDeviceRegistrationId(row.Token)
                                            .WithJson("{\"alert\":\"" + lm.Message + "\",\"badge\":7"
                                            //+ ",\"sound\":\"sound.caf\""
                                                        + "}")
                                                            );
                                    }
                                }
                                push.StopAllServices(waitForQueuesToFinish: true);
                                //validálunk
                                if (errortext == null)
                                {
                                    mdANDROID.isOK = true;
                                }
                                mdANDROID.ServiceResponse =
                                    string.Format("{0}{1}{2}{3}", null == errortext ? "" : errortext,
                                                                    null == warningtext ? "" : (null == errortext ? "" : " / ") + warningtext,
                                                                    null == successtext ? "" : (null == warningtext ? "" : " / ") + successtext,
                                                                    null == mdANDROID.ServiceResponse ? "" : (null == successtext ? "" : " | ") + mdANDROID.ServiceResponse); //régieket a végére
                                errortext = null;
                                warningtext = null;
                                successtext = null;
                            }
                            if (devIOS.Count() == 0)
                            {
                                mdANDROID.ServiceResponse = string.Format("{0}{1}",
                                                            DateTime.Now.ToString() + " No ANDRIOD device",
                                                            null == mdANDROID.ServiceResponse ? "" : " | " + mdANDROID.ServiceResponse); //régieket a végére
                            }

                            //mindenképpen mentem az üzenet naplót
                            db.SaveChanges();

                            if (!mdIOS.isOK || !mdANDROID.isOK)//hiba volt, kiszállok
                            {
                                return Json(new { success = false, error = true, response = "The cause of the error in service response!" });
                            }

                            //az üzenetet csak itt módosítom és majd mentem
                            //m.ServiceResponse = string.Format("{0}{1}", warningtext + " /// ", successtext);
                            m.PublishingDate = DateTime.Now.Date;
                            m.NewsStatus = sta;
                        }
                        db.SaveChanges();
                        log.Info("end: OK");
                        return Json(new { success = true, error = false, response = "OK" });
                    }
                    log.Info("end: Not found message");
                    return Json(new { success = false, error = false, response = "Not found" });
                }
                log.Info("end: bad parameters");
                return Json(new { success = false, error = true, response = "Bad request" });
            }
        }

        #endregion statusbuttons

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }


    }
}




