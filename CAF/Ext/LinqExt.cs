using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Ext
{
    public static class LinqExt
    {
        public static List<T> GetPagingList<T>(this IQueryable<T> query, PagingInfo pagingInfo)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            pagingInfo.TotalRecords = query.Count();

            var list = query.Skip(pagingInfo.PageIndex * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();

            if (list == null || list.Count == 0)
            {
                if (pagingInfo.PageIndex > 0 && pagingInfo.TotalRecords > 0)
                {
                    pagingInfo.PageIndex = 0;
                    list = query.Skip(pagingInfo.PageIndex * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();
                }
            }

            return list;
        }

    }

    public class PagingInfo
    {
        public PagingInfo() { }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
    }
}