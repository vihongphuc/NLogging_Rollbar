using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class Paged<T>
    {
        public int PageIndex { get; set; }
        public int CountPerPage { get; set; }

        public int TotalRecordCount { get; set; }
        public T[] Records { get; set; }

        public Paged(T[] records, int pageIndex, int countPerPage, int totalRecordCount)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException("pageIndex", "pageIndex cannot be less than 0");
            if (countPerPage <= 0)
                throw new ArgumentOutOfRangeException("countPerPage", "countPerPage cannot be less than or equal with 0");
            if (totalRecordCount < 0)
                throw new ArgumentOutOfRangeException("totalRecordCount", "totalRecordCount cannot be less than 0");
            if (records == null)
                throw new ArgumentNullException("records");

            this.Records = records;
            this.PageIndex = pageIndex;
            this.CountPerPage = countPerPage;
            this.TotalRecordCount = totalRecordCount;
        }
        public Paged()
        {
        }
    }
}
