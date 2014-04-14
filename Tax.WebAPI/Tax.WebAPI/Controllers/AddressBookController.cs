using Tax.WebAPI.Models;
using Tax.WebAPI.Query;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Tax.WebAPI.Controllers
{
    public class AddressBookController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AddressBookController));

        [AllowAnonymous]
        public IEnumerable<AddressBookEntry> Get()
        {
            return UserQueries.GetAddressBook(context);
        }

        [AllowAnonymous]
        public IEnumerable<AddressBookEntry> Get(string q)
        {
            return UserQueries.SearchAddressBookByName(context, q);
        }

        //Ey konkrét bejegyzés lekérése
        [AllowAnonymous]
        public AddressBookEntry Get(bool byExtension,string q)
        {
            if (byExtension)
                return UserQueries.SearchAddressBookByExtension(context, q);
            else
                return null;
        }

        // POST support is not needed here, it is just fot CORS testing
        [AllowAnonymous]
        public IEnumerable<AddressBookEntry> Post()
        {
            return UserQueries.GetAddressBook(context);
        }
    }
}
