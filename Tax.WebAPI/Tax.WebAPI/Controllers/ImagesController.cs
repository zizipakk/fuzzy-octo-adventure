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

namespace Tax.WebAPI.Controllers
{
    public class ImagesController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImagesController));

        /// <summary>
        /// Get picture
        /// </summary>
        /// <returns >An image file (binary) with mimetype 'image/[extension]'.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/Image")]
        public IHttpActionResult GetImage(string id)
        {
            try
            {
                log.Info(string.Format("Get picture by id: {0}", id));
                var imagesService = new ImagesService(context);

                var image = imagesService.GetImage(id);

                if (null == image)
                {
                    return NotFound();
                }
                if (null == image.file_type)
                {
                    return new FileResult(image.file_stream, image.name, "application/octet-stream");
                }
                else
                {
                    return new FileResult(image.file_stream, image.name, "image/" + image.file_type);
                }
            }
            catch (Exception ex)
            {
                log.Error("Picture access by id error: ", ex);
                throw;
            }
        }
    }
}