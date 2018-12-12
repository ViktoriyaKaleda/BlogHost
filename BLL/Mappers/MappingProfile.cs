using AutoMapper;

namespace BLL.Mappers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Interface.Entities.ApplicationUser, DAL.Interface.DTO.ApplicationUser>()
				.ForMember(m => m.Blogs, opt => opt.MapFrom(src => src.Blogs))
				.ForMember(m => m.Posts, opt => opt.MapFrom(src => src.Posts));

			CreateMap<DAL.Interface.DTO.ApplicationUser, Interface.Entities.ApplicationUser>()
				.ForMember(m => m.Blogs, opt => opt.MapFrom(src => src.Blogs))
				.ForMember(m => m.Posts, opt => opt.MapFrom(src => src.Posts));

			CreateMap<Interface.Entities.Blog, DAL.Interface.DTO.Blog>()
				.ForMember(m => m.Author, opt => opt.MapFrom(src => src.Author))
				.ForMember(m => m.BlogStyle, opt => opt.MapFrom(src => src.BlogStyle))
				.ForMember(m => m.Posts, opt => opt.MapFrom(src => src.Posts));

			CreateMap<Interface.Entities.Post, DAL.Interface.DTO.Post>()
				.ForMember(m => m.Author, opt => opt.MapFrom(src => src.Author))
				.ForMember(m => m.Blog, opt => opt.MapFrom(src => src.Blog))
				.ForMember(m => m.Tags, opt => opt.MapFrom(src => src.Tags))
				.ForMember(m => m.Likes, opt => opt.MapFrom(src => src.Likes))
				.ForMember(m => m.Comments, opt => opt.MapFrom(src => src.Comments));

			CreateMap<Interface.Entities.BlogStyle, DAL.Interface.DTO.BlogStyle>()
				.ForMember(m => m.Blogs, opt => opt.MapFrom(src => src.Blogs));

			CreateMap<Interface.Entities.Comment, DAL.Interface.DTO.Comment>()
				.ForMember(m => m.ChildComments, opt => opt.MapFrom(src => src.ChildComments));

			CreateMap<Interface.Entities.Like, DAL.Interface.DTO.Like>();

			CreateMap<Interface.Entities.Tag, DAL.Interface.DTO.Tag>();

			//Mapper.AssertConfigurationIsValid();
		}
	}
}
