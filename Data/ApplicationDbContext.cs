using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogH.Models;
using BlogHosting.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogHosting.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		
		public DbSet<BlogH.Models.Blog> Blog { get; set; }
		public DbSet<BlogH.Models.Post> Post { get; set; }
		public DbSet<BlogH.Models.Like> Like { get; set; }
		public DbSet<BlogH.Models.Comment> Comment { get; set; }
		public DbSet<BlogH.Models.Tag> Tag { get; set; }
		public DbSet<BlogModerator> BlogModerator { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>()
				.HasMany(m => m.Blogs)
				.WithOne(m => m.Author)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<ApplicationUser>()
				.HasMany(m => m.Posts)
				.WithOne(m => m.Author)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Post>()
				.HasOne(m => m.Blog)
				.WithMany(m => m.Posts)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Like>()
				.HasOne(m => m.Post)
				.WithMany(m => m.Likes)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Comment>()
				.HasOne(m => m.Post)
				.WithMany(m => m.Comments)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BlogModerator>()
			.HasKey(m => new { m.BlogId, m.ModeratorId });

			builder.Entity<BlogModerator>()
				.HasOne(m => m.Blog)
				.WithMany(m => m.BlogModerators)
				.HasForeignKey(m => m.BlogId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<BlogModerator>()
				.HasOne(m => m.Moderator)
				.WithMany(m => m.BlogModerator)
				.HasForeignKey(m => m.ModeratorId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Blog>()
				.HasMany(m => m.BlogModerators)
				.WithOne(m => m.Blog)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Tag>().Property(m => m.PostId).IsRequired();

			builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });
		}
	}
}
