using System;
using System.Collections.Generic;

namespace Tax.WebAPI.Models
{
    public class ArticlesBindingModel
    {
        public string Id { get; set; }
        public string ImageURL { get; set; }
        public string ThumbnailURL { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Subtitle { get; set; }
        public string Body { get; set; }
        public IEnumerable<TagsBindingModel> Tags { get; set; }
        public string Date { get; set; }
    }

}
