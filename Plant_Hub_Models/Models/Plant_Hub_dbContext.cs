using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            //modelBuilder.Entity<LikePost>()
            //    .HasOne(lp => lp.Post)
            //    .WithMany(p => p.LikePosts)
            //    .HasForeignKey(lp => lp.PostId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.Post)
            //    .WithMany(p => p.Comments)
            //    .HasForeignKey(c => c.PostId)
            //    .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Comment>()
            //     .HasOne(c => c.User)
            //     .WithMany(u => u.Comments)
            //     .HasForeignKey(c => c.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LikePost>()
          .HasOne(lp => lp.Post)
          .WithMany(p => p.LikePosts)
          .HasForeignKey(lp => lp.PostId)
          .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plant>()
                .HasMany(p => p.SavedPlants)
                .WithOne(sp => sp.Plant)
                .HasForeignKey(sp => sp.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavePlant>()
                .HasOne(sp => sp.User)
                .WithMany(u => u.SavedPlants)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.LikePosts)
                .WithOne(lp => lp.User)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Apply DeleteBehavior.Restrict to avoid multiple cascade paths
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                 .HasMany(p => p.Comments)
                 .WithOne(c => c.Post)
                 .HasForeignKey(c => c.PostId)
                 .OnDelete(DeleteBehavior.Restrict);

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<LikePost>()
        //        .HasOne(lp => lp.Post)
        //        .WithMany(p => p.LikePosts)
        //        .HasForeignKey(lp => lp.PostId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Comment>()
        //        .HasOne(c => c.Post)
        //        .WithMany(p => p.Comments)
        //        .HasForeignKey(c => c.PostId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Comment>()
        //        .HasOne(c => c.User)
        //        .WithMany(u => u.Comments)
        //        .HasForeignKey(c => c.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Plant>()
        //        .HasMany(p => p.SavedPlants)
        //        .WithOne(sp => sp.Plant)
        //        .HasForeignKey(sp => sp.PlantId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<SavePlant>()
        //        .HasOne(sp => sp.User)
        //        .WithMany(u => u.SavedPlants)
        //        .HasForeignKey(sp => sp.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<ApplicationUser>()
        //        .HasMany(u => u.LikePosts)
        //        .WithOne(lp => lp.User)
        //        .HasForeignKey(lp => lp.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // Additional configuration for the Post-Comment relationship
        //    modelBuilder.Entity<Post>()
        //        .HasMany(p => p.Comments)
        //        .WithOne(c => c.Post)
        //        .HasForeignKey(c => c.PostId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // Apply DeleteBehavior.Restrict to avoid multiple cascade paths
        //    modelBuilder.Entity<Comment>()
        //        .HasOne(c => c.Post)
        //        .WithMany(p => p.Comments)
        //        .HasForeignKey(c => c.PostId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}


        public DbSet<Category> Categories { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<SavePlant> SavePlants { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikePost> LikePosts { get; set; }
		public DbSet<NewPlant> NewPlants { get; set; }










	}
}
