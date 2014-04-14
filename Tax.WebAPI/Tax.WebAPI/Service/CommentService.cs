using Tax.Data.Models;
using Tax.WebAPI.Models;
using Tax.WebAPI.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Service
{
    public class CommentService
    {
        ApplicationDbContext context;

        public CommentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void SaveInterpreterComment(string comment, string interpreterName, string clientExtension)
        {
            var interpreter = UserQueries.GetUserByName(context, interpreterName);
            var client = UserQueries.GetUserByPBXExtension(context, clientExtension);

            context.Comments.Add(new Comment
            {
                Interperter = interpreter,
                Client = client,
                Timestamp = DateTime.Now,
                Text = comment
            });
        }

        public IEnumerable<CommentQueryResult> GetCommentsForClient(string clientExtension)
        {
            var client = UserQueries.GetUserByPBXExtension(context, clientExtension);
            if(client == null)
            {
                return null;
            }

            return context.Comments
                .Where(c => c.Client.Id == client.Id).OrderBy(c => c.Timestamp)
                .Select(c => new CommentQueryResult
                {
                    Comment = c.Text,
                    Timestamp = c.Timestamp,
                    ClientId = c.Client.Id,
                    ClientFirstName = c.Client.KontaktUser.FirstName,
                    ClientLastName = c.Client.KontaktUser.LastName,
                    InterpeterId = c.Interperter.Id,
                    InterpeterFirstName = c.Interperter.KontaktUser.FirstName,
                    InterpeterLastName = c.Interperter.KontaktUser.LastName
                }).ToList();
        }
    }
}