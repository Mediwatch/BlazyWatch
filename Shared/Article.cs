using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Shared
{
    public class Article
    {
        public string Name {get; set;}
        public string Title {get;set;}
        public List<string> Tags {get;set;}
        public string Content {get;set;}
        public string PreviewImageURL {get;set;}
        public string PreviewTitle {get;set;}
        public string PreviewParagraph {get;set;}
        public string ViewLevel {get;set;} = "friend";
    }
}