using System;
using System.Configuration;
using Tax.Data.Models;
using System.Linq;


namespace Tax.Portal.Helpers
{
    public class LocalisationHelpers
    {
        public static Guid GetLanguageId(string lid, ApplicationDbContext context)
        {
            ApplicationDbContext db = context;
            var res = db.Language.SingleOrDefault(x => x.ShortName == lid);

            if (null == res)
            {
                return Guid.Empty;
            }
            else
            {
                return res.Id;
            }            
        }
    }
}