using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mediwatch.Server.Areas.Identity.Data
{
    public class UserCustom : IdentityUser<Guid>
    {
    
        public string Address {get; set;}
        public string Job {get; set;}
        public string Message {get; set;}
        public int CountryNumber {get; set;}

    }
    public class IdentityDataContext : IdentityDbContext<UserCustom, IdentityRole<Guid>, Guid>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
