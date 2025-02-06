using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifiations;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;
        public GenericRepository(StoreContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        #region without specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //{
            //    return (IReadOnlyList<T>)await _dbContext
            //            .Products.Include(p => p.ProductBrand)
            //            .Include(p => p.ProductType).ToListAsync();
            //}
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
            //return await _dbContext.Set<T>().Where(P => P.Id == id)
            //                       .Include(P => P.ProductBrand)
            //                       .Include(P => P.ProductType);
        }
        #endregion

        #region with specifications
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApllySpecification(Spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApllySpecification(Spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApllySpecification(ISpecifications<T> Spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApllySpecification(Spec).CountAsync();
        }

        public async Task AddAsync(T item)
        => await _dbContext.Set<T>().AddAsync(item);

        public void UpdateAsync(T item)
        => _dbContext.Set<T>().Update(item);

        public void DeleteAsync(T item)
        => _dbContext.Set<T>().Remove(item);

        #endregion
    }
}
