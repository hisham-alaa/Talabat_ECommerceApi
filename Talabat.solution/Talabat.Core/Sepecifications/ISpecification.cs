using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Sepecifications
{
    public interface ISpecification<T> where T : EntityBase
    {
        public Expression<Func<T, bool>> WhereCondition { get; set; }

        public List<Expression<Func<T, object>>> ObjsToInclude { get; set; }

        public Expression<Func<T, object>> OrderBy { get; set; }

        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public bool PaginationEnabled { get; set; }

    }
}
