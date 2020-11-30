namespace BlazingBlog.Shared
{
    public class LinkDescription
    {
        public int success {get; set;} = 1;
        public MetaLink meta {get; set;}
    }

    public class MetaLink 
    {
        public string title {get; set;}
        public string description {get; set;}
        public ImageUploadURLs image {get; set;}
    }


}