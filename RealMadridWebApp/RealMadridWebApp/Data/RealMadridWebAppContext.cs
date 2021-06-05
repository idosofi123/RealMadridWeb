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
        
         public DbSet<RealMadridWebApp.Models.User> User { get; set; }
        
         public DbSet<RealMadridWebApp.Models.Team> Team { get; set; }
    }
}
