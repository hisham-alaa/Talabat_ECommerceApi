using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Sepecifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : EntityBase
    {
        public Expression<Func<T, bool>> WhereCondition { get; set; }
        public List<Expression<Func<T, object>>> ObjsToInclude { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool PaginationEnabled { get; set; }

        public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> InWherCondition)
        {
            WhereCondition = InWherCondition;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        public void ApplyPagination(int skip, int take)
        {
            PaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
