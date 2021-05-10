using System.IO;
using BlazingArticle.Model;
using Mediwatch.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Server {
    public class DbContextMediwatch : DbContext {
        public DbContextMediwatch (DbContextOptions<DbContextMediwatch> options) :
        base(options) { }
 
        protected DbContextMediwatch()
        {
        }

        //entities
        public DbSet<compagny> compagnies { get; set; }
        public DbSet<formation> formations { get; set; }
        public DbSet<applicant_session> applicant_sessions { get; set; }
        public DbSet<tag> tags { get; set; }

        public DbSet<orderInfo> orderInfos {get; set;}

        public DbSet<BlazingArticleModel> articleModels {get; set;}
        protected override void OnConfiguring (DbContextOptionsBuilder options) 
        {   
            var folderName = "Data";
            var PathSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(PathSave, "data.db");
            options.UseSqlite("Data Source=" + fullPath);
        }

    }
}