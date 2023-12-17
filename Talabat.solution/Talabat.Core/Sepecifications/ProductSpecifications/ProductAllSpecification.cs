using System.Linq.Expressions;
using Talabat.Core.Entites;

namespace Talabat.Core.Sepecifications.ProductSpecifications
{
    public class ProductAllSpecification : BaseSpecification<Product>
    {
        public ProductAllSpecification(ProductSpecsParams spec)
            : base(p =>
                        (!spec.CategoryId.HasValue || p.CategoryId == spec.CategoryId.Value) &&
                        (!spec.BrandId.HasValue || p.BrandId == spec.BrandId.Value) &&
                        (string.IsNullOrEmpty(spec.SearchText) || p.Name.ToLower().Contains(spec.SearchText.ToLower())))
        {
            SetIncludes();

            switch (spec.Sort)
            {
                case "price":
                    AddOrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name ?? "");
                    break;
            }

            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
        }

        public ProductAllSpecification(Expression<Func<Product, bool>> productExpression) : base(productExpression)
        {
            SetIncludes();
        }

        private void SetIncludes()
        {
            ObjsToInclude.Add(p => p.Brand);
            ObjsToInclude.Add(p => p.Category);
        }
    }
}