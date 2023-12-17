using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Sepecifications.ProductSpecifications
{
    public class ProductSpecsParams
    {
        public const int MaxPageSize = 5;

        private int pageSize = MaxPageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize || value == 0) ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;

        public string? Sort { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }
        public string? SearchText { get; set; }

    }
}
