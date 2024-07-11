using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Service.PagingService
{
    public class ExpensesPagingService : IPagingService<PaginatedList<Expense>>
    {
        private readonly IFindRepository<Expense> _FindExpenseRepository;
        public ExpensesPagingService(IFindRepository<Expense> findExpenseRepository)
        {
            _FindExpenseRepository = findExpenseRepository;
        }
        public PaginatedList<Expense> IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var ExpenseQuery = _FindExpenseRepository.QueryBy(x => true);

            if (!string.IsNullOrEmpty(searchString))
            {
                ExpenseQuery = ExpenseQuery.Where(c => c.Name.Contains(searchString) || c.Type.Contains(searchString));           }

            switch (sortOrder)
            {
                case "Date_asc":
                    ExpenseQuery = ExpenseQuery.OrderBy(c => c.Date);
                    break;
                case "Cost_asc":
                    ExpenseQuery = ExpenseQuery.OrderBy(c => c.Cost);
                    break;
                case "Cost_desc":
                    ExpenseQuery = ExpenseQuery.OrderByDescending(c => c.Cost);
                    break;
                case "Type_asc":
                    ExpenseQuery = ExpenseQuery.OrderBy(c => c.Type);
                    break;
                case "Type_desc":
                    ExpenseQuery = ExpenseQuery.OrderByDescending(c => c.Type);
                    break;
                case "Name_asc":
                    ExpenseQuery = ExpenseQuery.OrderBy(c => c.Name);
                    break;
                case "Name_desc":
                    ExpenseQuery = ExpenseQuery.OrderByDescending(c => c.Name);
                    break;
                default: //Date_desc
                    ExpenseQuery = ExpenseQuery.OrderByDescending(c => c.Date);
                    break;
            }
            int setPageSize = pageSize > 0 ? pageSize : 10;
            return PaginatedList<Expense>.Create(ExpenseQuery.AsNoTracking(), pageNumber ?? 1, setPageSize);
        }
    }
}
