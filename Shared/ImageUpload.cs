namespace BlazingBlog.Shared
{
    public class ImageUpload
    {
        public int success {get; set;}
        public ImageUploadURLs file {get; set;}

    }

    public class ImageUploadURLs
    {
        public string url {get; set;}
    }
}