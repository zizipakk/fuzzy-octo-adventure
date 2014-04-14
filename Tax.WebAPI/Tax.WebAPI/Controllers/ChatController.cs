using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Threading.Tasks;
using log4net;
using Tax.WebAPI.Service;
using Tax.Data.Models;
using Tax.WebAPI.Query;

namespace Tax.WebAPI.Controllers
{

    public class ChatController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChatController));

        ChatService chatManager = ChatService.Instance;

        public async Task<ChatMessage> Get()
        {
            try
            {
                return await chatManager.ReceiveMessageAsync(User.Identity.Name);
            }
            catch (Exception ex)
            {
                log.Error("Message receiveing error: ", ex);
                throw ex;
            }
        }

        public void Post([FromBody]ChatMessage message)
        {
            if(String.IsNullOrEmpty(message.ReceiverUserName))
            {
                if(String.IsNullOrEmpty(message.ReceiverExtension))
                {
                    throw new Exception("No reveiver specified");
                }

                var receiver = UserQueries.GetUserByPBXExtension(context, message.ReceiverExtension);
                message.ReceiverUserName = receiver.UserName;
            }

            log.Info("Message posted by: " + User.Identity.Name + " to " + message.ReceiverUserName);

            try
            {
                log.Info("ChatMessage: " + JsonConvert.SerializeObject(message));
                if (!String.IsNullOrEmpty(message.MessageText))
                {
                    message.SenderUserName = User.Identity.Name;
                    message.Timestamp = DateTime.Now;
                    message.SentTimestamp = null;
                    message.DeliveredTimetamp = null;

                    chatManager.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                log.Error("Message processing error: ", ex);
                throw ex;
            }
        }


    }
}