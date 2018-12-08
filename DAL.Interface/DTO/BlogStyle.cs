using System.Collections.Generic;

namespace DAL.Interface.DTO
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

		public virtual List<Blog> Blogs { get; set; }
	}
}
