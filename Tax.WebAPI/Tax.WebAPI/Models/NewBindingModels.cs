using System;
using System.Collections.Generic;

namespace Tax.WebAPI.Models
{
    public class NewBindingModel
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public Guid Headline_pictureId { get; set; }
        public Guid ThumbnailId { get; set; }
        public IEnumerable<TagBindingModel> Tags { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        //TODO
        public string Subtitle { get; set; }
        public string Body { get; set; }
    }

}
