using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Sepecifications.ProductSpecifications;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        public Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecsParams specParams);

        public Task<Product?> GetProductAsync(int productId);

        public Task<int> GetCountAsync(ProductSpecsParams specParams);

        public Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

        public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

    }
}
