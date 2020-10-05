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
        protected override void OnConfiguring (DbContextOptionsBuilder options) 
        {
             options.UseSqlite("Filename=data.db");
        }

    }
}