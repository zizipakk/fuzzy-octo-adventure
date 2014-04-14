using Tax.WebAPI.Models;
using Tax.WebAPI.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Tax.WebAPI.Controllers
{

    [Authorize(Roles = "Jeltolmácsok, Diszpécser")]
    public class CommentController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CommentController));

        public IHttpActionResult Post([FromBody] CommentSaveModel model)
        {
            try
            {
                log.Info("Saving comment by " + User.Identity.Name + ", on " + model.ClientExtension);

                var commentService = new CommentService(context);
                commentService.SaveInterpreterComment(model.Comment, User.Identity.Name, model.ClientExtension);
                context.SaveChanges();

                return Ok();
            }
            catch(Exception ex)
            {
                log.Error("Error saving comment: ", ex);
                throw;
            }
        }

        [ResponseType(typeof(CommentQueryResult))]
        public IHttpActionResult Get(string clientExtension)
        {
            try
            {
                log.Info("Getting comment for " + clientExtension);

                var commentService = new CommentService(context);
                var comments = commentService.GetCommentsForClient(clientExtension);

                if(comments == null)
                {
                    return NotFound();
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                log.Error("Getting comments: ", ex);
                throw;
            }
        }
    }
}
