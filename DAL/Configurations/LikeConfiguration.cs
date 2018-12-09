using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class LikeConfiguration : IEntityTypeConfiguration<Like>
	{
		public void Configure(EntityTypeBuilder<Like> builder)
		{
			builder
				.HasOne(m => m.Post)
				.WithMany(m => m.Likes)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
