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
    public class EventsService
    {
        ApplicationDbContext context;

        public EventsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// All Events
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public IEnumerable<EventsBindingModel> GetEvents(string lang)
        {
            var resl = context.EventsGlobal
                        .Where(x => x.NewsStatus.NameGlobal == "Published")
                        .SelectMany(x => x.EventsLocal.Where(y =>
                                                                    y.EventsGlobalId == x.Id
                                                                    && y.Language.ShortName == lang)
                            , (x, y) => new { x, y })
                        .ToList()
                        .Select(s => new EventsBindingModel
                        {
                            Id = s.x.Id.ToString(),
                            Title1 = s.y.Title1,
                            Title2 = s.y.Title2,
                            Body = s.y.Body_text,
                            Date = TimestampHelpers.GetTimestamp((DateTime)s.x.Date)
                        })
                        .OrderByDescending(o => o.Date);

            if (null == resl)
            {
                return null;
            }
            else
            {
                return resl;
            }
        }    
    }
}
