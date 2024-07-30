using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class BusinessReportRepository:IFindRepository<BusinessReport>
    {
        private readonly InchargeContext _context;
        public BusinessReportRepository(InchargeContext context)
        {
            _context = context;
        }
        public BusinessReport FindBy(Func<BusinessReport, bool> predicate)
        {
            return _context.BusinessReports
                .Include(x => x.Sales)
                .ThenInclude(x => x.Client)
                .Include(x => x.Sales)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductType)
                .Include(x => x.Expenses)
                .FirstOrDefault(predicate);
        }
        public List<BusinessReport> ListBy(Func<BusinessReport, bool> predicate)
        {
            return _context.BusinessReports
                .Include(x => x.Sales)
                .ThenInclude(x => x.Client)
                .Include(x => x.Sales)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductType)
                .Include(x => x.Expenses)
                .Where(predicate)
                .ToList();
        }
        public IQueryable<BusinessReport> QueryBy(Func<BusinessReport, bool> predicate)
        {
            return _context.BusinessReports
                .Include(x => x.Sales)
                .ThenInclude(x => x.Client)
                .Include(x => x.Sales)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductType)  
                .Include(x => x.Expenses)
                .Where(predicate)
                .AsQueryable();
        }
    }
}
