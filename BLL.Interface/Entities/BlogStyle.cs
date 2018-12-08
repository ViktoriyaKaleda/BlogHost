using System.Collections.Generic;

namespace BLL.Interface.Entities
{
	public class BlogStyle
	{
		public int BlogStyleId { get; set; }

		public string BlogStyleName { get; set; }

		public string DefaultImagePath { get; set; }

		public string BackgrounsColor { get; set; }

		public string FisrtColor { get; set; }

		public string SecondColor { get; set; }

		public string TitlesFontName { get; set; }

		public string TitlesFontColor { get; set; }

		public List<Blog> Blogs { get; set; }

		public BlogStyle()
		{
			Blogs = new List<Blog>();
		}
	}
}
