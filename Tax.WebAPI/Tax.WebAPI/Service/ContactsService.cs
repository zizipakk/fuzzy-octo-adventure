using System.Data.Entity;
using Tax.Data.Models;
using Tax.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax.WebAPI.Helpers;
using System.Web.Mvc;

namespace Tax.WebAPI.Service
{
    public class ContactsService
    {
        ApplicationDbContext context;

        public ContactsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ContactsBindingModel> GetContacts(IEnumerable<string> tags, string lang, string url)
        {
            string baseurl = url;//ebben a környezetben nem jóHtmlHelpers.AppBaseUrl(url);

            var res = context.ContactsGlobal
                        .Where(x => x.NewsStatus.NameGlobal == "Published"
                                && x.TagsGlobal.Select(y => y.Id.ToString()).ToArray().Intersect(tags).Any()
                        )
                        .Include(x => x.TagsGlobal)
                        .SelectMany(x => x.ContactsLocal.Where(y =>
                                                            y.ContactsGlobalId == x.Id
                                                            && y.Language.ShortName == lang), (x, y) => new { x, y })
                        .OrderByDescending(o => o.x.PublishingDate)
                        .Select(s => new ContactsBindingModel
                            {
                                Id = s.x.Id.ToString(),
                                FisrtName = s.x.First_name,
                                LastName = s.x.Last_name,
                                Department = s.y.Department,
                                Position = s.y.Position,
                                Profil = s.y.Profile,
                                ImageURL = string.Format("{0}api/Image?id={1}", baseurl, null == s.x.Photo ? "" : s.x.Photo.stream_id.ToString()),
                                PhoneNumbers = new List<PhonenumbersBindingModel>() { 
                                    new PhonenumbersBindingModel() { Label = "Phone", Number = s.x.Phone }, 
                                    new PhonenumbersBindingModel() { Label = "Mobile", Number = s.x.Mobile } },
                                Email = s.x.Email,
                                Tags = s.x.TagsGlobal
                                            .SelectMany(v => context.TagsLocal.Where(z =>
                                                                                        z.TagsGlobal.Id == v.Id
                                                                                        && z.Language.ShortName == lang)
                                                , (v, z) => new TagsBindingModel { Id = v.Id.ToString(), Name = z.Name }),
                                linkedinURL = s.x.Linkedin
                            }
                        );

            return res; 
        }
    
    }
}
