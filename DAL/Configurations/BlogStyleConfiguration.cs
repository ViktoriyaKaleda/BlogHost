using DAL.Interface.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
	public class BlogStyleConfiguration : IEntityTypeConfiguration<BlogStyle>
	{
		public void Configure(EntityTypeBuilder<BlogStyle> builder)
		{
			builder
				.HasMany(m => m.Blogs)
				.WithOne(m => m.BlogStyle)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasData(new BlogStyle()
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

			builder.HasData(new BlogStyle()
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

			builder.HasData(new BlogStyle()
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

			builder.HasData(new BlogStyle()
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
