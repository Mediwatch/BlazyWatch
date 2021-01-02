using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlazingBlog.Shared;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazingBlog.Server.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]

    public class BlogUtilsController : ControllerBase
    {
        /// <summary>
        /// API POST: /BlogUtils/UploadImage
        /// Upload Image to the server
        /// </summary>
        /// <returns>return the image informations</returns>
        [Authorize(Roles = "Admin,Tutor")]
        [HttpPost]
        public async Task<ImageUpload> UploadImage() {
            try {
                long size = 0;
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    var folderName = Path.Combine("Ressources", "Images");
                    var PathSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(PathSave, fileName);
                    size += file.Length;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                     return new ImageUpload {success = 1, file = new ImageUploadURLs{url = $@"https://localhost:5001/BlogUtils/GetImage?fileName={fileName}&contentType={file.ContentType}"}};
                }               
            } catch (Exception ex)
            {
                return new ImageUpload {success = 0, file = new ImageUploadURLs{url = ex.ToString()}};
            }
            return new ImageUpload {success = 0, file = null};
        }
        /// <summary>
        /// Get Image from image uploaded on the server
        /// GET: /BlogUtils/GetImage
        /// </summary>
        /// <param name="fileName">the path from the folder which the images are stocked</param>
        /// <param name="contentType">the type of image to get</param>
        /// <returns>the Image in raw</returns>
        [HttpGet]
        public IActionResult GetImage([FromQuery] string fileName, [FromQuery] string contentType)
        {
            var folderName = Path.Combine("Ressources", "Images");
            var PathSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(PathSave, fileName);
            var data = System.IO.File.ReadAllBytes(fullPath);
            var content = new System.IO.MemoryStream(data);

            Regex pattern = new Regex("[ ]|[+]{2}");
            contentType = pattern.Replace(contentType, "+");

            return File(content, contentType, fileName);
        }

        /// <summary>
        /// GET: /BlogUtils/GetLinInfo
        /// Give link info for preview
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>a link descrition base on the url</returns>
        [HttpGet]
        public LinkDescription GetLinInfo(string url) {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = WebRequestMethods.Http.Get;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            String responseString = reader.ReadToEnd();

            response.Close();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseString);

            String title = (from x in doc.DocumentNode.Descendants()
                        where x?.Name?.ToLower() == "title"
                        select x?.InnerText)?.FirstOrDefault();

            String desc = (from x in doc.DocumentNode.Descendants()
                    where x.Name.ToLower() == "meta"
                    && x.Attributes["name"] != null
                    && x?.Attributes["name"]?.Value.ToLower() == "description"
                    select x?.Attributes["content"]?.Value)?.FirstOrDefault();

            List<String> imgs = (from x in doc.DocumentNode.Descendants()
                     where x.Name.ToLower() == "img"
                     select x?.Attributes["src"]?.Value)?.ToList<String>();

   
            return new LinkDescription{success = 1, meta = new MetaLink{title = title, description = desc, image = new ImageUploadURLs{url= (imgs != null ? imgs[0] : null)}}};
        }
        
    }
}