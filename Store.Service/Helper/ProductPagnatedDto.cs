using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper
{
    public class ProductPagnatedDto<T>
    {
        public ProductPagnatedDto(int pageIndex, int pageSize, int totalCount, IReadOnlyList<object> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int  TotalCount { get; set; }

        public IReadOnlyList<object> Data { get; set; }
    }
}
