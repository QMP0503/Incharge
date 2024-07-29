using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class DiscountPagingService : IPagingService<PaginatedList<Discount>>
    {
        private readonly IFindRepository<Discount> _FindDiscountRepository;
        public DiscountPagingService(IFindRepository<Discount> findDiscountRepository)
        {
         _FindDiscountRepository = findDiscountRepository;
        }
        public PaginatedList<Discount> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var DiscountQuery = _FindDiscountRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                DiscountQuery = DiscountQuery.Where(d => d.Name.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_asc":
                    DiscountQuery = DiscountQuery.OrderByDescending(c => c.Name);
                    break;
                case "Name_desc":
                    DiscountQuery = DiscountQuery.OrderByDescending(c => c.Name);
                    break;
                case "DiscountValue_asc":
                    DiscountQuery = DiscountQuery.OrderBy(c => c.DiscountValue);
                    break;
                default: //DiscountValue_asc
                    DiscountQuery = DiscountQuery.OrderByDescending(c => c.DiscountValue);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<Discount>.Create(DiscountQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}

