using Incharge.Service.IService;
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
        public PaginatedList<Equipment> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var EquipmentQuery = _FindEquipmentRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                EquipmentQuery = EquipmentQuery.Where(c => c.Name.Contains(searchString) || c.GymClass.Name.ToString().Contains(searchString) || c.Status.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    EquipmentQuery = EquipmentQuery.OrderByDescending(c => c.Name);
                    break;
                case "GymClass.Name_asc":
                    EquipmentQuery = EquipmentQuery.OrderBy(c => c.GymClass.Name);
                    break;
                case "GymClass.Name_desc":
                    EquipmentQuery = EquipmentQuery.OrderByDescending(c => c.GymClass.Name);
                    break;
                case "Status_asc":
                    EquipmentQuery = EquipmentQuery.OrderBy(c => c.Status);
                    break;
                case "Status_desc":
                    EquipmentQuery = EquipmentQuery.OrderByDescending(c => c.Status);
                    break;
                default: //firstName_asc
                    EquipmentQuery = EquipmentQuery.OrderBy(c => c.Name);
                    break;
            }
            const int pageSize = 10;
            return PaginatedList<Equipment>.Create(EquipmentQuery.AsNoTracking(), pageNumber ?? 1, pageSize);
        }
    }
}
