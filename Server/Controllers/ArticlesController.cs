using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingArticle.Model;
using BlazingBlog.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server;

/// <summary>
/// Fichier avec les fonctions relatif aux controleurs des Articles. 
/// </summary>
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
        /// obtenir des informations sur les articles
        /// GET: /Articles/GetArticlesInfo
        /// </summary>
        /// <returns>retourner toutes les informations sur les articles</returns>
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
        /// obtenir le contenu de l'article
        /// Get: /Articles/GetArticle?name={string}
        /// </summary>
        /// <param name="name">id de l'article</param>
        /// <returns>retour article</returns>
        [HttpGet]
        public async Task<ActionResult<Article>>GetArticle([FromQuery]string name)
        {
            var article = await _context.articleModels.FindAsync(System.Guid.Parse(name));
            return new Article{
                    Name = article.Key.ToString(),
                    Title = article.Title,
                    Content = article.Content
                };
        }
        /// <summary>
        /// créer un article à partir d'un objet article
        /// API
        /// GET: /Articles/CreateArticle
        /// </summary>
        /// <param name="article">article à ajouter à la base de données</param>
        /// <returns>retourne OK 200 si ça marche</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Tutor")]
        public async Task<System.Guid> CreateArticle([FromBody]Article article) {
            
            var dbArticle = new BlazingArticleModel {
                Key = new System.Guid(),
                 Title = article.Title,
                    Content = article.Content,
                    PreviewImageURL = article.PreviewImageURL,
                    PreviewParagraph = article.PreviewParagraph,
                    PreviewTitle = article.PreviewTitle,
            };
            _context.articleModels.Add(dbArticle);
            await _context.SaveChangesAsync();
            return dbArticle.Key;
        }
    }
}