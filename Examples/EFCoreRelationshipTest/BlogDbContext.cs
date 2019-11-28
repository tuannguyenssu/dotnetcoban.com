using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreRelationshipTest.ManyToMany;
using EFCoreRelationshipTest.OneToOne;
using Microsoft.EntityFrameworkCore;
using Blog = EFCoreRelationshipTest.OneToOne.Blog;
using Post = EFCoreRelationshipTest.OneToMany.Post;

namespace EFCoreRelationshipTest
{
    //https://docs.microsoft.com/en-us/ef/core/modeling/relationships
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        public DbSet<OneToOne.Blog> OneToOneBlogs { get; set; }
        public DbSet<OneToOne.BlogImage> OneToOneBlogImages { get; set; }

        public DbSet<OneToMany.Blog> OneToManyBlogs { get; set; }
        public DbSet<OneToMany.Post> OneToManyPosts { get; set; }

        public DbSet<ManyToMany.Post> ManyToManyPosts { get; set; } 
        public DbSet<ManyToMany.Tag> ManyToManyTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // One Blog has one BlogImage
            modelBuilder.Entity<OneToOne.Blog>()
                .HasOne(p => p.BlogImage)
                .WithOne(i => i.Blog)
                .HasForeignKey<OneToOne.BlogImage>(b => b.BlogId);

            // One Blog has many Posts
            modelBuilder.Entity<OneToMany.Post>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<ManyToMany.PostTag>()
                .HasKey(t => new {t.PostId, t.TagId});

            // Many Posts and many Tags
            // Định nghĩa thông qua PostTag
            modelBuilder.Entity<ManyToMany.PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<ManyToMany.PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}
