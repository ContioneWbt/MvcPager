using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PagerCheck.Extentions
{
    public class PaginationEntity<T> where T : class
    {
        public PaginationEntity(IEnumerable<T> source, Pagination pi)
        {
            this.Source = source.Skip((pi.PageIndex - 1) * pi.PageSize).Take(pi.PageSize);
            this.Pi = pi;
            this.Pi.ItemCount = source.Count();
        }
        public Pagination Pi { get; }
        public IEnumerable<T> Source { get; set; }
    }
    public class Pagination
    {
        public int ItemCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}