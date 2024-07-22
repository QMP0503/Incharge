using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class EmployeePagingService : IPagingService<PaginatedList<Employee>>
    {
        readonly IFindRepository<Employee> _FindEmployeeRepository; //add search by client feature.
        public EmployeePagingService(IFindRepository<Employee> findEmployeeRepository)
        {
            _FindEmployeeRepository = findEmployeeRepository;
        }
        public PaginatedList<Employee> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var EmployeeQuery = _FindEmployeeRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                EmployeeQuery = EmployeeQuery.Where(c => c.FirstName.ToLower().Contains(searchString) || c.LastName.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "FirstName_desc":
                    EmployeeQuery = EmployeeQuery.OrderByDescending(c => c.FirstName);
                    break;
                case "LastName":
                    EmployeeQuery = EmployeeQuery.OrderBy(c => c.LastName);
                    break;
                case "LastName_desc":
                    EmployeeQuery = EmployeeQuery.OrderByDescending(c => c.LastName);
                    break;
                default: //firstName_asc
                    EmployeeQuery = EmployeeQuery.OrderBy(c => c.FirstName);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<Employee>.Create(EmployeeQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}
