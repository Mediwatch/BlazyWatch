using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingArticle.Model
{
     [Table("BlazingArticles")]
    public class BlazingArticleModel
    {
        private static readonly char delimiter = ';';

        [Key]
        public Guid Key {get; set;}
        public string Title {get; set;}
        public string Content {get; set;}

        public string PreviewImageURL {get;set;}
        public string PreviewTitle {get;set;}
        public string PreviewParagraph {get;set;}
        public string _tags { get; set; }

        [NotMapped]
        public string[] Tags
        {
            get { return _tags == null ? null : _tags.Split(delimiter); }
            set { _tags = string.Join($"{delimiter}", value); }
        }
    }
}