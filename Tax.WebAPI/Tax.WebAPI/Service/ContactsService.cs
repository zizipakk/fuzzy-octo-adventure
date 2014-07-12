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

        //public IEnumerable<ContactsBindingModel> GetContacts(string[] tags, string lang, string url)
        public IEnumerable<ContactsBindingModel> GetContacts(string lang)
        {
            string baseurl = HtmlHelpers.AppBaseUrl("/api/Image?id=");

            //List<Guid> tagsGL = new List<Guid>();
            //foreach (string t in tags.ToList())
            //{
            //    tagsGL.Add(Guid.Parse(t));
            //}
            //Guid[] tagsGA = tagsGL.ToArray();

            var res = context.ContactsGlobal
                        .Where(x => x.NewsStatus.NameGlobal == "Published"
                                //&& x.TagsGlobal.Select(y => y.Id).Intersect(tagsGA).Any()
                        )
                        .Include(x => x.TagsGlobal)
                        .SelectMany(x => x.ContactsLocal.Where(y =>
                                                            y.ContactsGlobalId == x.Id
                                                            && y.Language.ShortName == lang), (x, y) => new { x, y })
                        .ToList()
                        .Select(s => new ContactsBindingModel
                            {
                                Id = s.x.Id.ToString(),
                                FirstName = s.x.First_name,
                                LastName = s.x.Last_name,
                                Department = s.y.Department,
                                Position = s.y.Position,
                                Profil = s.y.Profile,
                                ImageURL = string.Format("{0}{1}", baseurl, null == s.x.Photo ? "" : s.x.Photo.stream_id.ToString()),
                                PhoneNumbers = new List<PhonenumbersBindingModel>() { 
                                    new PhonenumbersBindingModel() { Label = "Phone", Number = s.x.Phone }, 
                                    new PhonenumbersBindingModel() { Label = "Mobile", Number = s.x.Mobile ?? "" } },
                                Email = s.x.Email,
                                //Tags = s.x.TagsGlobal
                                //            .SelectMany(v => context.TagsLocal.Where(z =>
                                //                                                        z.TagsGlobal.Id == v.Id
                                //                                                        && z.Language.ShortName == lang)
                                //                , (v, z) => new TagsBindingModel { Id = v.Id.ToString(), Name = z.Name }),
                                Tags = s.x.TagsGlobal.Select(v => v.Id.ToString()).ToArray(),
                                linkedinURL = s.x.Linkedin
                            }
                        )
                        .OrderByDescending(o => o.LastName)
                        .ThenBy(t => t.FirstName);

            return res; 
        }
    
    }
}
