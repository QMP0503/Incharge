using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class BusinessReportPagingService: IPagingService<PaginatedList<BusinessReport>>
    {
        readonly IFindRepository<BusinessReport> _FindBusinessReportRepository;
        public BusinessReportPagingService(IFindRepository<BusinessReport> FindBusinessReportRepository)
        {
            _FindBusinessReportRepository = FindBusinessReportRepository;
        }
        public PaginatedList<BusinessReport> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var BusinessreportQuery = _FindBusinessReportRepository.QueryBy(x => true);


            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                BusinessreportQuery = BusinessreportQuery.Where(c => true);
            }

            switch (sortOrder)
            {
                case "Date_asc":
                    BusinessreportQuery = BusinessreportQuery.OrderBy(c => c.Date);
                    break;
                case "Date_desc":
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.Date);
                    break;                    
                case "Profit_asc":
                    BusinessreportQuery = BusinessreportQuery.OrderBy(c => c.Profit);
                    break;
                case "Profit_desc":
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.Profit);
                    break;
                case "Cost_asc":
                    BusinessreportQuery = BusinessreportQuery.OrderBy(c => c.Cost);
                    break;
                case "Cost_desc":
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.Cost);
                    break;
                case "Revenue_asc":
                    BusinessreportQuery = BusinessreportQuery.OrderBy(c => c.Revenue);
                    break;
                case "Revenue_desc":
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.Revenue);
                    break;
                case "MonthlyMembers_asc":
                    BusinessreportQuery = BusinessreportQuery.OrderBy(c => c.MonthlyMembers);
                    break;
                case "MonthlyMembers_desc":
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.MonthlyMembers);
                    break;
                case "NewMembershipsSold_asc":
                    BusinessreportQuery = BusinessreportQuery.OrderBy(c => c.NewMembershipSales);
                    break;
                case "NewMembershipsSold_desc":
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.NewMembershipSales);
                    break;
                default:
                    BusinessreportQuery = BusinessreportQuery.OrderByDescending(c => c.Date);
                    break;
            }

        int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<BusinessReport>.Create(BusinessreportQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}
