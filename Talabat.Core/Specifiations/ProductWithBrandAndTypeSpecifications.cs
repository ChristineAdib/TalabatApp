using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifiations
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product>
    {
        //is used for get all products
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params)
            : base(P =>
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params. TypeId)
            )
        {
            Includes.Add(P=>P.ProductType);
            Includes.Add(P => P.ProductBrand);

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }

        //is used for get product by id (with critria)
        public ProductWithBrandAndTypeSpecifications(int id):base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
