﻿using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Data;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class ProductPagingService:IPagingService<PaginatedList<Product>>
    {
        readonly IFindRepository<Product> _FindProductRepository; //add search by client feature.
        public ProductPagingService(IFindRepository<Product> findProductRepository)
        {
            _FindProductRepository = findProductRepository;
        }
        public PaginatedList<Product> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var ProductQuery = _FindProductRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ProductQuery = ProductQuery.Where(c => c.Name.ToLower().Contains(searchString) || c.ProductType.Name.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    ProductQuery = ProductQuery.OrderByDescending(c => c.Name);
                    break;
                case "Price":
                    ProductQuery = ProductQuery.OrderBy(c => c.TotalPrice);
                    break;
                case "Price_desc":
                    ProductQuery = ProductQuery.OrderByDescending(c => c.TotalPrice);
                    break;
                default: //firstName_asc
                    ProductQuery = ProductQuery.OrderBy(c => c.Name);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;

            return PaginatedList<Product>.Create(ProductQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }

    }
}
