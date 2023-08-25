﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Plant_Hub_Models.Models
{
   public partial class Plant_Hub_dbContext : IdentityDbContext<ApplicationUser>
    {
        public Plant_Hub_dbContext()
        {
        }

        public Plant_Hub_dbContext(DbContextOptions<Plant_Hub_dbContext> options)
            : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LikePost>()
                .HasOne(lp => lp.Post)
                .WithMany(p => p.LikePosts)
                .HasForeignKey(lp => lp.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Change this to ReferentialAction.Restrict

            modelBuilder.Entity<Comment>()
                 .HasOne(c => c.User)
                 .WithMany(u => u.Comments)
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<SavePlant> SavePlants { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikePost> LikePosts { get; set; }








    }
}
