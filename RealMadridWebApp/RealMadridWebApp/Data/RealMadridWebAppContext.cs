using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Models;

namespace RealMadridWebApp.Data {

    public class RealMadridWebAppContext : DbContext {

        public RealMadridWebAppContext (DbContextOptions<RealMadridWebAppContext> options) : base(options) { }

        public DbSet<RealMadridWebApp.Models.Stadium> Stadium { get; set; }
   
         public DbSet<RealMadridWebApp.Models.Player> Player { get; set; }

        public DbSet<RealMadridWebApp.Models.Country> Country { get; set; }

        public DbSet<RealMadridWebApp.Models.User> User { get; set; }
        
        public DbSet<RealMadridWebApp.Models.Team> Team { get; set; }
         
        public DbSet<RealMadridWebApp.Models.Match> Match { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            // Using Entity Framework, adding a unique index on the "IsHome" column,
            // which allows only one team to be considered as the home team.
            modelBuilder.Entity<Team>().HasIndex(t => new { t.IsHome })
                                       .IsUnique()
                                       .HasFilter($"{nameof(Models.Team.IsHome)} = 1");
        }

        public DbSet<RealMadridWebApp.Models.Competition> Competition { get; set; }

        public DbSet<RealMadridWebApp.Models.Position> Position { get; set; }

    }
}
