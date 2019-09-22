using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplicationFramework.Models
{
    public class FightsContext : DbContext
    {
        public FightsContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FightsContext, WebApplicationFramework.Migrations.Configuration>());
        }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Fighter> Fighters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fight>().Property(f => f.Time).HasColumnType("date");
            modelBuilder.Entity<Fight>()
                .HasRequired(s => s.Winner)
                .WithMany(g => g.Fights)
                .HasForeignKey<int>(s => s.Winner_Id);
        }
    }
}