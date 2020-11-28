using System;
using CAFFShop.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CAFFShop.Dal
{
    public class CaffShopContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Animation> Animations { get; set; }
        public DbSet<AnimationPurchase> AnimationPurchases { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        public CaffShopContext(DbContextOptions<CaffShopContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasMany(x => x.AnimationPurchases)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);

                builder.HasMany(x => x.Comments)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);

                builder.HasMany(x => x.UploadedAnimations)
                    .WithOne(x => x.Author)
                    .HasForeignKey(x => x.AuthorId);
            });

            modelBuilder.Entity<Animation>(builder =>
            {
                builder.HasOne(x => x.File)
                    .WithMany()
                    .HasForeignKey(x => x.FileId);

                builder.HasOne(x => x.Preview)
                    .WithMany()
                    .HasForeignKey(x => x.PreviewId);

                builder.HasMany(x => x.Comments)
                    .WithOne(x => x.Animation)
                    .HasForeignKey(x => x.AnimationId);

                builder.HasMany(x => x.AnimationPurchases)
                    .WithOne(x => x.Animation)
                    .HasForeignKey(x => x.AnimationId);

                builder.HasOne(x => x.ApprovedBy)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedById);
            });
        }
    }
}