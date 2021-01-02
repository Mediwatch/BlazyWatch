using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingArticle.Model;
using BlazingBlog.Shared;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// get information about articles
        /// GET: /Articles/GetArticlesInfo
        /// </summary>
        /// <returns>return all articles informations</returns>
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

        /// <summary>
        /// get content of article
        /// Get: /Articles/GetArticle?name={string}
        /// </summary>
        /// <param name="name">id of the article</param>
        /// <returns>return Article</returns>
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
        /// <summary>
        /// create an article from an Arcticle Object
        /// API
        /// GET: /Articles/CreateArticle
        /// </summary>
        /// <param name="article">article to add to data base</param>
        /// <returns>return OK 200 if it work</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Tutor")]
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