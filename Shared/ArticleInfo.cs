using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Shared
{
    public class ArticleInfo
    {
        public string Name {get; set;}
        public string Title {get;set;}
        public string[] Tags {get;set;}

        public string PreviewImageURL {get;set;}
        public string PreviewTitle {get;set;}
        public string PreviewParagraph {get;set;}
    }
}