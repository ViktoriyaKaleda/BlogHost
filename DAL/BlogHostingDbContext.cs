using DAL.Interface.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
	public class BlogHostingDbContext : IdentityDbContext<ApplicationUser>
	{
		public BlogHostingDbContext(DbContextOptions<BlogHostingDbContext> options)
			: base(options) { }

		public DbSet<Blog> Blog { get; set; }
		public DbSet<Post> Post { get; set; }
		public DbSet<Like> Like { get; set; }
		public DbSet<Comment> Comment { get; set; }
		public DbSet<Tag> Tag { get; set; }
		public DbSet<BlogModerator> BlogModerator { get; set; }
		public DbSet<BlogStyle> BlogStyle { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(BlogHostingDbContext).Assembly);
		}
	}
}
