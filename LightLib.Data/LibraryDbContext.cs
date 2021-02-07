using System.Collections.Generic;
using LightLib.Data.Models;
using LightLib.Data.Models.Assets;
using LightLib.Data.Models.Assets.Tags;
using LightLib.Models;
using Microsoft.EntityFrameworkCore;

namespace LightLib.Data {
    public class LibraryDbContext : DbContext {
        public LibraryDbContext() { }

        public LibraryDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<AudioBook> AudioBooks { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BranchHours> BranchHours { get; set; }
        public virtual DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public virtual DbSet<Checkout> Checkouts { get; set; }
        public virtual DbSet<AudioCd> AudioCds { get; set; }
        public virtual DbSet<DVD> Dvds { get; set; }
        public virtual DbSet<Hold> Holds { get; set; }
        public virtual DbSet<Asset> LibraryAssets { get; set; }
        public virtual DbSet<LibraryBranch> LibraryBranches { get; set; }
        public virtual DbSet<LibraryCard> LibraryCards { get; set; }
        public virtual DbSet<Patron> Patrons { get; set; }
        public virtual DbSet<Periodical> Periodicals { get; set; }
        public virtual DbSet<AvailabilityStatus> Statuses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        
         protected override void OnModelCreating(ModelBuilder modelBuilder) {
             base.OnModelCreating(modelBuilder);
             
             SeedInitialAssetStatuses(modelBuilder);
             LinkAssets(modelBuilder);
             LinkAssetTags(modelBuilder);
         }

         private static void LinkAssets(ModelBuilder modelBuilder) {
             modelBuilder.Entity<Book>().HasOne(book => book.Asset);
             modelBuilder.Entity<AudioCd>().HasOne(cd => cd.Asset);
             modelBuilder.Entity<DVD>().HasOne(dvd => dvd.Asset);
             modelBuilder.Entity<Periodical>().HasOne(p => p.Asset);
             modelBuilder.Entity<AudioBook>().HasOne(ab => ab.Asset);
         }

         private static void LinkAssetTags(ModelBuilder modelBuilder) {
             // An Asset has many Tags
             // There is a join table AssetTag with a composite key
             modelBuilder.Entity<AssetTag>().HasKey(at => new {at.AssetId, at.TagId});

             // Every AssetTag relates to an Asset 
             modelBuilder.Entity<AssetTag>()
                 .HasOne(at => at.Asset)
                 .WithMany(a => a.AssetTags)
                 .HasForeignKey(at => at.AssetId);

             // Every AssetTag relates to a Tag
             modelBuilder.Entity<AssetTag>()
                 .HasOne(at => at.Tag)
                 .WithMany(a => a.AssetTags)
                 .HasForeignKey(at => at.TagId);
         }

         private static void SeedInitialAssetStatuses(ModelBuilder modelBuilder) {
             var defaultStatuses = new List<AvailabilityStatus> {
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
             modelBuilder.Entity<AvailabilityStatus>().HasData(defaultStatuses);
         }
    }
}