using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tax.Data.Models;
using Tax.WebAPI.Models;

namespace Tax.WebAPI.Query
{
    public class UserQueries
    {
        public static ApplicationUser GetUserByName(ApplicationDbContext context, string username)
        {
            return context.Users.Where(u => u.UserName == username).Single();
        }

        //public static IEnumerable<AddressBookEntry> SearchAddressBookByName(ApplicationDbContext context, string name)
        //{
        //    return context.Users.Where(u => String.Concat(u.KontaktUser.FirstName, " ", u.KontaktUser.LastName).Contains(name))
        //             .Join(context.PBXExtensionData.Where(p => p.isDroped == false), u => u.Id, p => p.ApplicationUser.Id,
        //             (u, p) => new AddressBookEntry
        //             {
        //                 FirstName = u.KontaktUser.FirstName,
        //                 LastName = u.KontaktUser.LastName,
        //                 Extension = p.PhoneNumber.InnerPhoneNumber
        //             });
        //}

    }
}