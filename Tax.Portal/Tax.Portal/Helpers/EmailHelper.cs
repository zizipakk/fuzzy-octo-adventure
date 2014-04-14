using Tax.Portal.Mailers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;

namespace Tax.Portal.Helpers
{
    public class QueueItem<M>
        where M : IMessageData
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Message<M> message { get; set; }
        public IEnumerable<Addressee> addresses { get; set; }
        public string url { get; set; }

        public QueueItem(Message<M> message, IEnumerable<Addressee> addresses, UrlHelper Url)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("QueueItem"))
            {
                log.Info("begin");
                this.message = message;
                this.addresses = addresses;
                this.url = HtmlHelpers.AppBaseUrl(Url);
                log.Info("end");
            }
        }
    }

    public static class MessageHelper<T>
        where T : IMessageData
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Queue<QueueItem<T>> _MessageQueue;
        private static readonly object QueueLock = new object();

        static MessageHelper()
        {
            _MessageQueue = new Queue<QueueItem<T>>();
        }

        public static void SendMessageToQueue(Message<T> message, IEnumerable<Addressee> addresses, UrlHelper Url)
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("SendMessageToQueue<M>"))
            {
                log.Info("begin");

                lock (QueueLock)
                {
                    _MessageQueue.Enqueue(new QueueItem<T>(message, addresses, Url));
                }

                // ezt nem lockoljuk, mert azt majd ő megteszi, ha kell
                InitSendMessageThread(SendMessage);
                //SendMessage();

                log.Info("end");
            }
        }

        private static Thread WorkerThread;
        public static void InitSendMessageThread(SendMessageWorker SendMessage)
        {
            //http://stackoverflow.com/a/1825267/208922
            WorkerThread = new Thread(new ThreadStart(SendMessage));
            //http://www.andrewdenhertog.com/asp-net/c-mvc-fixing-an-asynchronous-operation-cannot-be-started-at-this-time-asynchronous-operations-may-only-be-started-within-an-asynchronous-handler-or-module-or-during-certain-events-in-the-page-life/
            WorkerThread.IsBackground = true;
            WorkerThread.Start();
            //http://stackoverflow.com/a/7896766/208922
            //WorkerThread.Join();
        }

        public delegate void SendMessageWorker();
        public static void SendMessage()
        {
            using (log4net.ThreadContext.Stacks["NDC"].Push("SendMessage"))
            {
                try
                {
                    log.Info("begin");
                    QueueItem<T> item;

                    lock (QueueLock)
                    {
                        if (0 >= _MessageQueue.Count)
                        {//ilyen nem lehet
                            throw new ApplicationException("Üres az üzenetlista, pedig levelet szeretnénk küldeni");
                        }
                        item = _MessageQueue.Dequeue();
                    }
                    //ez pedig nem megy Thread-ből indítva
                    //var wc = new WebhookController();
                    //var result = wc.EmailPost(JsonConvert.SerializeObject(item));

                    log.Info(string.Format("item: {0}", JsonConvert.SerializeObject(item)));
                    var client = new RestClient(string.Format("{0}", item.url));
                    var request = new RestRequest("/Mailer/EmailPost", Method.POST);
                    log.Info(string.Format("request: {0}", JsonConvert.SerializeObject(request)));
                    request.AddParameter("m", JsonConvert.SerializeObject(item.message));
                    request.AddParameter("a", JsonConvert.SerializeObject(item.addresses));
                    //request.AddParameter("t", typeof(T).ToString());
                    var response = client.Execute(request);
                    log.Info(string.Format("response: {0}", JsonConvert.SerializeObject(response)));
                    log.Info("end");
                }
                catch (Exception e)
                {
                    log.Error("Nem sikerült a levélküldés", e);
                }

            }
        }

    }
}