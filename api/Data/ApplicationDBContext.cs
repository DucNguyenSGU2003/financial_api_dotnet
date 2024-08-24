using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Migrations;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //portfolio many-to-many
            builder.Entity<Portfolio>(x=> x.HasKey(p => new {p.AppUserId,p.StockId}));

            builder.Entity<Portfolio>()
            .HasOne(u=>u.AppUser)
            .WithMany(u => u.portfolios)
            .HasForeignKey(p => p.AppUserId);
            
            builder.Entity<Portfolio>()
            .HasOne(u=> u.Stock)
            .WithMany (u => u.portfolios)
            .HasForeignKey(p => p.StockId);




            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole{
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

        
    }
}