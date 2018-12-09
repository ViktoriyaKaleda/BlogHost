using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class BlogConfiguration : IEntityTypeConfiguration<Blog>
	{
		public void Configure(EntityTypeBuilder<Blog> builder)
		{
			builder
				.HasMany(m => m.BlogModerators)
				.WithOne(m => m.Blog)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
