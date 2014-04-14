using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using log4net;
using Tax.Data.Models;

namespace Tax.WebAPI.Service
{
    public class ChatService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChatService));

        private static readonly ChatService _instance = new ChatService();
        public static ChatService Instance { get { return _instance; } }

        private ConcurrentDictionary<string, ConcurrentBag<TaskCompletionSource<Guid>>> registredChatClients;

        private Timer dbPollScheduler;

        private ChatService()
        {
            registredChatClients = new ConcurrentDictionary<string, ConcurrentBag<TaskCompletionSource<Guid>>>();

            dbPollScheduler = new Timer(250);
            dbPollScheduler.Elapsed += dbPollScheduler_Elapsed;
            dbPollScheduler.Start();
        }

        public async Task<ChatMessage> ReceiveMessageAsync(string user)
        {
            var waitingClientsForUser = registredChatClients.GetOrAdd(user, new ConcurrentBag<TaskCompletionSource<Guid>>());

            var newMessageSignal = new TaskCompletionSource<Guid>();
            waitingClientsForUser.Add(newMessageSignal);
            var messageId = await newMessageSignal.Task;

            using (var context = new ApplicationDbContext())
            {
                var message = context.ChatMessages.Find(messageId);
                FillPBXExtensionData(message, context);

                if (message.SenderUserName == user)
                {
                    message.SentTimestamp = DateTime.Now;
                }
                if (message.ReceiverUserName == user)
                {
                    message.DeliveredTimetamp = DateTime.Now;
                }
                context.SaveChanges();
                return message;
            }
        }

        public void SendMessage(ChatMessage message)
        {
            using (var context = new ApplicationDbContext())
            {
                context.ChatMessages.Add(message);
                context.SaveChanges();
            }
        }

        private void CheckForNewMessages()
        {
            using (var context = new ApplicationDbContext())
            {
                var messagesToSend = context.ChatMessages.Where(m => m.SentTimestamp == null).ToList();
                foreach (var user in messagesToSend.Select(m => m.SenderUserName).Distinct())
                {
                    var oldestMessageOfUser = messagesToSend.Where(m => m.SenderUserName == user).OrderBy(m => m.Timestamp).First();
                    NotifyClients(user, oldestMessageOfUser.Id);
                }

                var messagesToDeliver = context.ChatMessages.Where(m => m.DeliveredTimetamp == null).ToList();
                foreach (var user in messagesToDeliver.Select(m => m.ReceiverUserName).Distinct())
                {
                    var oldestMessageOfUser = messagesToDeliver.Where(m => m.ReceiverUserName == user).OrderBy(m => m.Timestamp).First();
                    NotifyClients(user, oldestMessageOfUser.Id);
                }
            }
        }

        private void dbPollScheduler_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                CheckForNewMessages();
            }
            catch (Exception ex)
            {
                log.Error("Scheduled database poll failed: ", ex);
            }
        }

        private void NotifyClients(string user, Guid messageId)
        {
            ConcurrentBag<TaskCompletionSource<Guid>> clientsToNotify;
            registredChatClients.TryRemove(user, out clientsToNotify);
            if (clientsToNotify != null)
            {
                foreach (var client in clientsToNotify)
                {
                    client.SetResult(messageId);
                }
            }
        }

        private void FillPBXExtensionData(ChatMessage message, ApplicationDbContext context)
        {
            message.SenderExtension = context.Users.Where(u => u.UserName == message.SenderUserName)
                .Join(context.PBXExtensionData.Where(p => p.isDroped == false), 
                    u => u.Id, p => p.ApplicationUser.Id, 
                    (u, p) => p.PhoneNumber.InnerPhoneNumber)
                .SingleOrDefault();

            message.ReceiverExtension = context.Users.Where(u => u.UserName == message.ReceiverUserName)
                .Join(context.PBXExtensionData.Where(p => p.isDroped == false), 
                    u => u.Id, p => p.ApplicationUser.Id, 
                    (u, p) => p.PhoneNumber.InnerPhoneNumber)
                .SingleOrDefault();
        }


    }
}