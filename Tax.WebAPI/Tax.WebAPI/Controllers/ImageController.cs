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
    public class ImageController : TaxWebAPIBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImageController));

        //[ResponseType(typeof(UserProfile))]
        //public IHttpActionResult Get()
        //{
        //    try
        //    {
        //        log.Info("Get profile for: " + User.Identity.Name);

        //        var profileService = new ProfileService(context);
        //        var profile = profileService.GetUserProfile(User.Identity.Name);

        //        if (profile == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(profile);
        //    }
        //    catch(Exception ex)
        //    {
        //        log.Error("Profile access by username error: ", ex);
        //        throw;
        //    }
        //}

        //[ResponseType(typeof(UserProfile))]
        //[Authorize(Roles = "Jeltolmácsok, Diszpécser")]
        //public IHttpActionResult GetByExtension(string extension)
        //{
        //    try
        //    {
        //        log.Info(string.Format("Get profile for extension: {0}",extension));
        //        var profileService = new ProfileService(context);

        //        var profile = profileService.GetUserProfileByExtension(extension: extension);

        //        if (profile == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Profile access by extension error: ", ex);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Access the profile picture of a user
        ///// </summary>
        ///// <returns >An image file (binary) with mimetype 'image/[extension]'.</returns>
        //[Authorize(Roles = "Jeltolmácsok, Diszpécser")]
        //[Route("api/ProfilePhoto")]
        //public IHttpActionResult GetPhoto(string extension)
        //{
        //    try
        //    {
        //        log.Info(string.Format("Get photo for extension: {0}", extension));
        //        var profileService = new ProfileService(context);

        //        var photoFile = profileService.GetUserPhotoByPBXExtension(extension);

        //        if (photoFile == null)
        //        {
        //            return NotFound();
        //        }
        //        return new FileResult(photoFile.file_stream, photoFile.name, "image/" + photoFile.file_type);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Profile photo access by extension error: ", ex);
        //        throw;
        //    }
        //}
    }
}