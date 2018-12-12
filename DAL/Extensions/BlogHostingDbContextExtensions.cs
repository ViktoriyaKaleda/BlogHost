using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAL.Extensions
{
	public static class BlogHostingDbContextExtensions
	{
		public static void DetachLocal<T>(this BlogHostingDbContext context, T t, int entryId) where T : class
		{
			var local = context.Set<T>()
				.Local
				.FirstOrDefault(entry => entry.GetType().GetProperties()
					.Single(p => p.Name.EndsWith("Id"))
					.GetValue(entry).Equals(entryId));

			if (local != null)
			{
				context.Entry(local).State = EntityState.Detached;
			}

			context.Entry(t).State = EntityState.Modified;
		}
	}
}
