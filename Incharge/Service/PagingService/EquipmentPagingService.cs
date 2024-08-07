﻿using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class EquipmentPagingService : IPagingService<PaginatedList<Equipment>>
    {
        private readonly IFindRepository<Equipment> _FindEquipmentRepository;
        public EquipmentPagingService(IFindRepository<Equipment> findEquipmentRepository)
        {
            _FindEquipmentRepository = findEquipmentRepository;
        }
        public PaginatedList<Equipment> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var EquipmentQuery = _FindEquipmentRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                EquipmentQuery = EquipmentQuery.Where(c => c.Name.ToLower().Contains(searchString) || c.Status.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    EquipmentQuery = EquipmentQuery.OrderByDescending(c => c.Name);
                    break;
                case "Status_asc":
                    EquipmentQuery = EquipmentQuery.OrderBy(c => c.Status);
                    break;
                case "Status_desc":
                    EquipmentQuery = EquipmentQuery.OrderByDescending(c => c.Status);
                    break;
                default: //Name_asc
                    EquipmentQuery = EquipmentQuery.OrderBy(c => c.Name);
                    break;
            }
            int setPageSize = pageSize>0 ? pageSize : 10;
            return PaginatedList<Equipment>.Create(EquipmentQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}
