using System;
using System.Collections.Generic;
using LightLib.Data.Models;
using LightLib.Models;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Data {
    public class LibraryDbContext : DbContext {
        public LibraryDbContext() { }

        public LibraryDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Checkout> Checkouts { get; set; }
        public virtual DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public virtual DbSet<LibraryBranch> LibraryBranches { get; set; }
        public virtual DbSet<BranchHours> BranchHours { get; set; }
        public virtual DbSet<LibraryCard> LibraryCards { get; set; }
        public virtual DbSet<Patron> Patrons { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<LibraryAsset> LibraryAssets { get; set; }
        public virtual DbSet<Hold> Holds { get; set; }
        
         protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            
            var now = DateTime.UtcNow;

            var defaultStatuses = new List<Status> {
                new() {
                    Id = 1,
                    Name = AssetStatus.Lost,
                    Description = "The item is lost."
                },
                new() {
                    Id = 2,
                    Name = AssetStatus.GoodCondition,
                    Description = "The item is in good condition."
                },
                new() {
                    Id = 3,
                    Name = AssetStatus.Unknown,
                    Description = "The item is in unknown whereabouts and condition."
                },
                new() {
                    Id = 4,
                    Name = AssetStatus.Destroyed,
                    Description = "The item has been destroyed."
                },
            };

            // Seeding initial Asset Statuses
            modelBuilder.Entity<Status>().HasData(defaultStatuses);
        }
    }
}