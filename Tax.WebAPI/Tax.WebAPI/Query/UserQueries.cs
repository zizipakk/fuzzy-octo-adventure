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

        public static ApplicationUser GetUserByPBXExtension(ApplicationDbContext context, string extensionID)
        {
            return        
                context.PBXExtensionData.Where(pbx => pbx.PhoneNumber.InnerPhoneNumber == extensionID && pbx.isDroped == false)
                   .Join(context.Users, p => p.ApplicationUser.Id, u => u.Id, (p,u) => u)
                   .SingleOrDefault();
        }

        public static IEnumerable<AddressBookEntry> SearchAddressBookByName(ApplicationDbContext context, string name)
        {
            return context.Users.Where(u => String.Concat(u.KontaktUser.FirstName, " ", u.KontaktUser.LastName).Contains(name))
                     .Join(context.PBXExtensionData.Where(p => p.isDroped == false), u => u.Id, p => p.ApplicationUser.Id,
                     (u, p) => new AddressBookEntry
                     {
                         FirstName = u.KontaktUser.FirstName,
                         LastName = u.KontaktUser.LastName,
                         Extension = p.PhoneNumber.InnerPhoneNumber
                     });
        }

        public static AddressBookEntry SearchAddressBookByExtension(ApplicationDbContext context, string extension)
        {
            return context.PhoneNumber.Where(p => p.InnerPhoneNumber == extension).Join(context.PBXExtensionData, p => p.Id, x => x.PhoneNumber.Id,
                (p,x) => new AddressBookEntry
                {
                    FirstName = x.ApplicationUser.KontaktUser.FirstName,
                    LastName = x.ApplicationUser.KontaktUser.LastName,
                    Extension = p.InnerPhoneNumber
                }
                ).FirstOrDefault();
            
        }

        public static IEnumerable<AddressBookEntry> GetAddressBook(ApplicationDbContext context)
        {
            return context.Users.Join(context.PBXExtensionData.Where(p => p.isDroped == false), u => u.Id, p => p.ApplicationUser.Id,
                     (u, p) => new AddressBookEntry
                     {
                         FirstName = u.KontaktUser.FirstName,
                         LastName = u.KontaktUser.LastName,
                         Extension = p.PhoneNumber.InnerPhoneNumber
                     });
        }
    }
}