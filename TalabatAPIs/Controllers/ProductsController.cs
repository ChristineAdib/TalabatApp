using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifiations;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _TypeRepo;
        private readonly IGenericRepository<ProductBrand> _brandrepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,IMapper mapper,
                                  IGenericRepository<ProductType> TypeRepo,
                                  IGenericRepository<ProductBrand> BrandRepo)
        {
            _productRepo=ProductRepo;
            _mapper=mapper;
            _TypeRepo=TypeRepo;
            _brandrepo=BrandRepo;
        }
        [CachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params) 
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(Spec);
            var MappedProducts=_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProducts,Count));
        } 
       
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _productRepo.GetEntityWithSpecAsync(Spec);
            if (Product is null)
                return NotFound(new ApiResponse(404));
            var MappedProduct=_mapper.Map<Product,ProductToReturnDto>(Product);
            return Ok(MappedProduct); 
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _TypeRepo.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands=await _brandrepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}
