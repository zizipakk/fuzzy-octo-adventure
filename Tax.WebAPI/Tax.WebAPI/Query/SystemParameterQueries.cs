using Tax.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Query
{
    public static class SystemParameterQueries
    {
        public static IEnumerable<SystemParameter> GetAllPublicParameters(ApplicationDbContext context)
        {
            return context.SystemParameter.Where(p => p.Public == true).ToList();
        }

        public static SystemParameter GetPublicParameterByName(ApplicationDbContext context, string name)
        {
            return context.SystemParameter
                        .Where(p => p.Name == name && p.Public == true)
                        .SingleOrDefault();
        }
    }
}