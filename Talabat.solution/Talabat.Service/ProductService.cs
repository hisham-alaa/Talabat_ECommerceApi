using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Sepecifications;
using Talabat.Core.Sepecifications.ProductSpecifications;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecsParams specParams)
        {
            var spec = new ProductAllSpecification(specParams);

            var products = await _unitOfWork.Repository<Product>().GetAllAsyncWithSpec(spec);

            return products;
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductAllSpecification(p => p.Id == productId);

            var product = await _unitOfWork.Repository<Product>().GetAsyncWithSpec(spec);

            return product;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

        public async Task<int> GetCountAsync(ProductSpecsParams spec)
        {
            var countSpec = new CountSpecs<Product>(
                p => (!spec.CategoryId.HasValue || p.CategoryId == spec.CategoryId.Value) &&
                     (!spec.BrandId.HasValue || p.BrandId == spec.BrandId.Value) &&
                     (string.IsNullOrEmpty(spec.SearchText) || p.Name.ToLower().Contains(spec.SearchText.ToLower())));

            var count = await _unitOfWork.Repository<Product>().GetCountAsyncWithSpec(countSpec);

            return count;
        }
    }
}
