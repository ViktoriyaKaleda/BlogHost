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
		public DbSet<BlogStyle> BlogStyle { get; set; }

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

			builder.Entity<BlogStyle>()
				.HasMany(m => m.Blogs)
				.WithOne(m => m.BlogStyle)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BlogStyle>().HasData(new BlogStyle()
			{
				BlogStyleId = 1,
				BlogStyleName = "Default",
				DefaultImagePath = "/images/slider-1.jpg",
				FisrtColor = "#000000",
				SecondColor = "",
				TitlesFontColor = "#",
				TitlesFontName = "",
				BackgrounsColor = ""
			});

			builder.Entity<BlogStyle>().HasData(new BlogStyle()
			{
				BlogStyleId = 2,
				BlogStyleName = "Soft",
				DefaultImagePath = "/images/blog-header6.jpg",
				FisrtColor = "#000000",
				SecondColor = "#FFA500",
				TitlesFontColor = "#763eb6",
				TitlesFontName = "Concert One, cursive",
				BackgrounsColor = "#eeeeee"
			});

			builder.Entity<BlogStyle>().HasData(new BlogStyle()
			{
				BlogStyleId = 3,
				BlogStyleName = "Gentle",
				DefaultImagePath = "/images/blog-header3.jpg",
				FisrtColor = "#000000",
				SecondColor = "#B6798F",
				TitlesFontColor = "#763eb6",
				TitlesFontName = "Cormorant, serif",
				BackgrounsColor = "#F9F2FA"
			});

			builder.Entity<BlogStyle>().HasData(new BlogStyle()
			{
				BlogStyleId = 4,
				BlogStyleName = "Ultraviolet",
				DefaultImagePath = "/images/blog-header4.jpg",
				FisrtColor = "#000000",
				SecondColor = "#001b8a",
				TitlesFontColor = "#763eb6",
				TitlesFontName = "PT Sans, sans-serif",
				BackgrounsColor = "#C7C0E0"
			});

		}
	}
}
