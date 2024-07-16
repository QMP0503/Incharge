using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace Incharge.Service.PagingService
{
    public class ClientPagingService : IPagingService<PaginatedList<Client>>
    {
        readonly IFindRepository<Client> _findClientRepository;
        public ClientPagingService(IFindRepository<Client> findClientRepository)
        {
            _findClientRepository = findClientRepository;
        }
        public PaginatedList<Client> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var ClientQuery = _findClientRepository.QueryBy(x => true);
            

            if (!string.IsNullOrEmpty(searchString))
            {
                ClientQuery = ClientQuery.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "FirstName_desc":
                    ClientQuery = ClientQuery.OrderByDescending(c => c.FirstName);
                    break;
                case "LastName":
                    ClientQuery = ClientQuery.OrderBy(c => c.LastName);
                    break;
                case "LastName_desc":
                    ClientQuery = ClientQuery.OrderByDescending(c => c.LastName);
                    break;
                default: //firstName_asc
                    ClientQuery = ClientQuery.OrderBy(c => c.FirstName);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<Client>.Create(ClientQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}
