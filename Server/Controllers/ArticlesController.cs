using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingArticle.Model;
using BlazingBlog.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server;

namespace BlazingBlog.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly DbContextMediwatch _context;

        public ArticlesController(DbContextMediwatch context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleInfo>>> GetArticlesInfo()
        {
            BlazingArticleModel[] articles = await _context.articleModels.ToArrayAsync();
            ArticleInfo[] articleInfos = new ArticleInfo[articles.Length];


            for (int i = 0; i < articles.Length; i++)
            {
                articleInfos[i] = new ArticleInfo {
                    Name = articles[i].Key.ToString(),
                    Title = articles[i].Title,
                    Tags = articles[i].Tags,
                    PreviewImageURL = articles[i].PreviewImageURL,
                    PreviewParagraph = articles[i].PreviewParagraph,
                    PreviewTitle = articles[i].PreviewTitle
                };
            }
            return articleInfos;
        }

        [HttpGet]
        public async Task<ActionResult<Article>>GetArticle([FromQuery]string name)
        {
            var article = await _context.articleModels.FindAsync(System.Guid.Parse(name));
            return new Article{
                    Name = article.Key.ToString(),
                    Title = article.Title,
                    Tags = article.Tags.ToList(),
                    Content = article.Content
                };
        }
        [HttpPost]
        public async Task<ActionResult> CreateArticle([FromBody]Article article) {
            
            var dbArticle = new BlazingArticleModel {
                Key = new System.Guid(),
                 Title = article.Title,
                    Tags = article.Tags.ToArray(),
                    Content = article.Content,
                    PreviewImageURL = article.PreviewImageURL,
                    PreviewParagraph = article.PreviewParagraph,
                    PreviewTitle = article.PreviewTitle,
            };
            _context.articleModels.Add(dbArticle);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}