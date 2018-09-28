using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogH.Models;

namespace BlogHosting.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<BlogH.Models.Blog> Blog { get; set; }
		public DbSet<BlogH.Models.Post> Post { get; set; }
		public DbSet<BlogH.Models.Like> Like { get; set; }
		public DbSet<BlogH.Models.Comment> Comment { get; set; }
	}
}
