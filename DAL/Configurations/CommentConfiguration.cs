using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class CommentConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder
				.HasOne(m => m.Post)
				.WithMany(m => m.Comments)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
