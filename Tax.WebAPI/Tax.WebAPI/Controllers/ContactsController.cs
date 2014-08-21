using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tax.WebAPI.Models;
using Tax.WebAPI.Service;
using log4net;
using Tax.Data.Models;
using System.Web.Http.Description;
using Tax.WebAPI.Results;
using Newtonsoft.Json;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;

namespace Tax.WebAPI.Controllers
{
    public class ContactsController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContactsController));

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Contacts")]
        [ResponseType(typeof(IEnumerable<ContactsBindingModel>))]
        public IHttpActionResult Contacts([ModelBinder(typeof(ArrayModelBinderProvider))] string[] tags, string lang)
        {
            try
            {
                //log.Info(string.Format("Get all of Contacts in order of PublishedDate by Tags: {0}, on language: {1}", JsonConvert.SerializeObject(tags), lang));
                log.Info(string.Format("Get all of Contacts in order of PublishedDate by language: {0}", lang));

                var contactsService = new ContactsService(context);
                //var contacts = contactsService.GetContacts(tags, lang);
                var contacts = contactsService.GetContacts(lang);

                if (contacts == null)
                {
                    log.Info("Not found, end");
                    return NotFound();
                }
                else if (contacts.Count() == 0)
                {
                    log.Info("Empty result");
                    var empty = new Dictionary<string, string>();
                    return Ok(empty);
                }
                else
                {
                    log.Info(string.Format("model: {0}", JsonConvert.SerializeObject(contacts)));
                    log.Info("OK, end");
                    return Ok(contacts);
                }
            }
            catch (Exception ex)
            {
                log.Error("All of Contacts access by tags + language error: ", ex);
                throw;
            }
        }

    }
}