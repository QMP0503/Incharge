using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class GymclassPagingService:IPagingService<PaginatedList<Gymclass>>
    {
        readonly IFindRepository<Gymclass> _findGymClassRepository;
        public GymclassPagingService(IFindRepository<Gymclass> findGymClassRepository)
        {
            _findGymClassRepository = findGymClassRepository;
        }
        public PaginatedList<Gymclass> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var GymClassQuery = _findGymClassRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                GymClassQuery = GymClassQuery.Where(c => c.Name.ToLower().Contains(searchString) || c.Date.ToString().ToLower().Contains(searchString) );
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    GymClassQuery = GymClassQuery.OrderByDescending(c => c.Name);
                    break;
                case "Name_asc":
                    GymClassQuery = GymClassQuery.OrderBy(c => c.Name);
                    break;
                case "ClassDate_asc":
                    GymClassQuery = GymClassQuery.OrderBy(c => c.Date);
                    break;
                default: //Date_desc
                    GymClassQuery = GymClassQuery.OrderByDescending(c => c.Date);
                    break;
            }
            int setPageSize = pageSize>0? pageSize: 10;
            return PaginatedList<Gymclass>.Create(GymClassQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);

        }
    }
}
