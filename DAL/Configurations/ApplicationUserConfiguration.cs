using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder
				.HasMany(m => m.Blogs)
				.WithOne(m => m.Author)
				.OnDelete(DeleteBehavior.Cascade);

			builder
				.HasMany(m => m.Posts)
				.WithOne(m => m.Author)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
