using System.Data.Entity;
using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax.WebAPI.Query;


namespace Tax.WebAPI.Service
{
    public class ProfileService
    {
        ApplicationDbContext context;

        public ProfileService(ApplicationDbContext context)
        {
            this.context = context;
        }


        ///// <summary>
        ///// Mellék alapján kérdezi le a kontakt felhasználó pbx profilját
        ///// </summary>
        ///// <param name="extension">a mellék, ami alapján keresünk</param>
        ///// <returns>Ha nincs ilyen mellék, akkor null-lal tér vissza, különben a mellékhez tartozó pbx profil-lal</returns>
        //public UserProfile GetUserProfileByExtension(string extension)
        //{
        //    var pbxProfile = context
        //                        .PBXExtensionData
        //                        .Include(x=>x.ApplicationUser)
        //                        .Include(x=>x.ApplicationUser.KontaktUser)
        //                        .Include(x=>x.ApplicationUser.SinoszUser)
        //                        .Include(x => x.ApplicationUser.SinoszUser.Organization)
        //                        .Include(x=>x.PhoneNumber)
        //                        .Where(p => 
        //                            null != p.PhoneNumber
        //                            && p.PhoneNumber.InnerPhoneNumber == extension
        //                            && !p.isDroped
        //                            )
        //                        .SingleOrDefault();

        //    if (null==pbxProfile)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        Organization organization = null;
        //        if (pbxProfile.ApplicationUser.SinoszUser != null)
        //        {
        //            organization = pbxProfile.ApplicationUser.SinoszUser.Organization;
        //        }
        //        return new UserProfile
        //        {
        //            Email = pbxProfile.ApplicationUser.Email,
        //            FirstName = null!=pbxProfile.ApplicationUser ? null!=pbxProfile.ApplicationUser.KontaktUser ? pbxProfile.ApplicationUser.KontaktUser.FirstName : null : null,
        //            LastName = null!=pbxProfile.ApplicationUser ? null!=pbxProfile.ApplicationUser.KontaktUser ? pbxProfile.ApplicationUser.KontaktUser.LastName : null : null,
        //            PBXExtension =  null!=pbxProfile.PhoneNumber ? pbxProfile.PhoneNumber.InnerPhoneNumber : null,
        //            PBXPassword = string.Empty, //user.Password
        //            OrganizationName  = organization != null ? organization.OrganizationName : string.Empty,
        //            OrganizationId = organization != null ? organization.Id.ToString() : string.Empty
        //        };
        //    }

        //}

        //public File GetUserPhotoByPBXExtension(string extensionID)
        //{
        //    File file = null;
        //    var user = UserQueries.GetUserByPBXExtension(context, extensionID);
        //    if (null!=user && user.SinoszUser != null)
        //    {
        //        var attachedFile = UserQueries.GetUserByPBXExtension(context, extensionID)
        //                            .SinoszUser.AttachedFile.Where(f => f.FileType == null)
        //                            .SingleOrDefault();
        //        if (attachedFile != null)
        //        {
        //            file = context.File.Find(attachedFile.FileId);
        //        }
        //    }
        //    return file;
        //}
    }
}