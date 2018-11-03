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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Post>()
				.HasOne(m => m.Blog)
				.WithMany(m => m.Posts)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Like>()
				.HasOne(m => m.Post)
				.WithMany(m => m.Likes)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Comment>()
				.HasOne(m => m.Post)
				.WithMany(m => m.Comments)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Tag>().Property(m => m.PostId).IsRequired();

			builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });
		}
	}
}
