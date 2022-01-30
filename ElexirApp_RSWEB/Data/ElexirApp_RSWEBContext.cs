using ElexirApp_RSWEB.Areas.Identity.Data;
using ElexirApp_RSWEB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElexirApp_RSWEB.Data
{
    public class ElexirApp_RSWEBContext : IdentityDbContext<ElexirApp_RSWEBUser>
    {
        public ElexirApp_RSWEBContext(DbContextOptions<ElexirApp_RSWEBContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("AppDBContextConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Usluga>().HasOne(p => p.FirstEmployee).WithMany(p => p.Usluga1).HasForeignKey(p => p.FirstEmployeeId);
            builder.Entity<Usluga>().HasOne(p => p.SecondEmployee).WithMany(p => p.Usluga2).HasForeignKey(p => p.SecondEmployeeId);

            base.OnModelCreating(builder);
        }

        public DbSet<Models.Vraboten> Vraboten { get; set; }

        public DbSet<Models.Usluga> Usluga { get; set; }

        public DbSet<Models.Rezervacija> Rezervacija { get; set; }

        public DbSet<Models.Korisnik> Korisnik { get; set; }

    }
}
