﻿using Talabat.Core.Entites;

namespace TalabatAPIs.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PectureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }
        public string ProductBrand { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
    }
}
