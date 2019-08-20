using LifeGallery.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGallery.DAL.EF
{
    public class LifeGalleryContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        public LifeGalleryContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(x => x.UserProfile)
                .WithRequiredPrincipal(x => x.ApplicationUser)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.Liked)
                .WithRequired(x => x.UserProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.Comments)
                .WithRequired(x => x.UserProfile)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Photo>()
                .HasMany(x => x.Comments)
                .WithRequired(x => x.Photo)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Photo>()
                .HasMany(x => x.Likes)
                .WithRequired(x => x.Photo)
                .WillCascadeOnDelete(true);
        }
    }
}
