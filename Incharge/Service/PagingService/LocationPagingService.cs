﻿using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class LocationPagingService : IPagingService<PaginatedList<Location>>
    {
        private readonly IFindRepository<Location> _FindEquipmentRepository;
        public LocationPagingService(IFindRepository<Location> findEquipmentRepository)
        {
            _FindEquipmentRepository = findEquipmentRepository;
        }
        public PaginatedList<Location> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var LocationQuery = _FindEquipmentRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                LocationQuery = LocationQuery.Where(l=> l.Name.ToLower().Contains(searchString) || l.Status.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    LocationQuery = LocationQuery.OrderByDescending(l => l.Name);
                    break;
                case "Capacity_asc":
                    LocationQuery = LocationQuery.OrderBy(l => l.Capacity);
                    break;
                case "Capacity_desc":
                    LocationQuery = LocationQuery.OrderByDescending(l => l.Capacity);
                    break;
                case "Status_asc":
                    LocationQuery = LocationQuery.OrderBy(l => l.Status);
                    break;
                case "Status_desc":
                    LocationQuery = LocationQuery.OrderByDescending(l => l.Status);
                    break;
                default: //Name_asc
                    LocationQuery = LocationQuery.OrderBy(l => l.Name);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<Location>.Create(LocationQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}