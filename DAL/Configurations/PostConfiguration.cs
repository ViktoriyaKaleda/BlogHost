using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class PostConfiguration : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder
				.HasOne(m => m.Blog)
				.WithMany(m => m.Posts)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
