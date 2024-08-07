﻿using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class SalesPagingService : IPagingService<PaginatedList<Sale>>
    {
        private readonly IFindRepository<Sale> _FindSaleRepository;
        public SalesPagingService(IFindRepository<Sale> findSaleRepository)
        {
            _FindSaleRepository = findSaleRepository;
        }
        public PaginatedList<Sale> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var SaleQuery = _FindSaleRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                SaleQuery = SaleQuery.Where(c => c.PaymentType.ToLower().Contains(searchString) || c.Client.FirstName.ToLower().Contains(searchString) || c.Client.LastName.ToString().ToLower().Contains(searchString) || c.Employee.FirstName.ToLower().Contains(searchString) || c.Employee.LastName.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {

                case "Date_asc":
                    SaleQuery = SaleQuery.OrderBy(c => c.Date);
                    break;
                case "ProductName_asc":
                    SaleQuery = SaleQuery.OrderBy(c => c.Product.Name);
                    break;
                case "ProductName_desc":
                    SaleQuery = SaleQuery.OrderByDescending(c => c.Product.Name);
                    break;
                case "PaymentType_asc":
                    SaleQuery = SaleQuery.OrderBy(c => c.PaymentType);
                    break;
                case "PaymentType_desc":
                    SaleQuery = SaleQuery.OrderByDescending(c => c.PaymentType);
                    break;
                case "TotalPrice_desc":
                    SaleQuery = SaleQuery.OrderByDescending(c => c.TotalPrice);
                    break;
                case "TotalPrice_asc":
                    SaleQuery = SaleQuery.OrderBy(c => c.TotalPrice);
                    break;  
                default: //Date_desc
                    SaleQuery = SaleQuery.OrderByDescending(c => c.Date);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<Sale>.Create(SaleQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}
