using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Common.Pagination
{
    public static class PagingExtensions
    {
        #region IQueryable<T> extensions

		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
		{
			return new PagedList<T>(source, pageIndex, pageSize, totalCount);
		}

		#endregion

		#region IEnumerable<T> extensions

		public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
		{
			return new PagedList<T>(source, pageIndex, pageSize, totalCount);
		}

		#endregion
    }
}
