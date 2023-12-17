using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Sepecifications
{
    public class CountSpecs<T> : BaseSpecification<T> where T : EntityBase
    {
        public CountSpecs(Expression<Func<T, bool>> criteria)
        {
            WhereCondition = criteria;
        }
    }
}
