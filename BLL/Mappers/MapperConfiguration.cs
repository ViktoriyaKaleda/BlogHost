using AutoMapper;

namespace BLL.Mappers
{
	public static class MapperInitializer
	{
		public static void MapperConfiguration()
		{
			Mapper.Initialize(c => c.AddProfile<MappingProfile>());
		}
	}
}
