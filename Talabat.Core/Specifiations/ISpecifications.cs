using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifiations
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T,object>>> Includes { get; set; }
        //prop for orderby
        public Expression<Func<T,object>> OrderBy { get; set; }
        //prop for orderbedesc
        public Expression<Func<T,object>> OrderByDescending { get; set; }
        //take
        public int Take { get; set; }
        //skip
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
