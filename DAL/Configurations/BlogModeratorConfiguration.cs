using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class BlogModeratorConfiguration : IEntityTypeConfiguration<BlogModerator>
	{
		public void Configure(EntityTypeBuilder<BlogModerator> builder)
		{
			builder.HasKey(m => new { m.BlogId, m.ModeratorId });

			builder
				.HasOne(m => m.Blog)
				.WithMany(m => m.BlogModerators)
				.HasForeignKey(m => m.BlogId)
				.OnDelete(DeleteBehavior.Restrict);

			builder
				.HasOne(m => m.Moderator)
				.WithMany(m => m.BlogModerator)
				.HasForeignKey(m => m.ModeratorId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
