using Tax.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Service
{
    public class BugPostService
    {

        ApplicationDbContext context;

        public BugPostService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddBugPost(BugPost model)
        {
            context.BugPosts.Add(model);
        }
    }
}