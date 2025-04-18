﻿namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? categoryId { get; set; }
    }
}
