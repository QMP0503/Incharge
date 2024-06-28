using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;
namespace Incharge.Repository
{
    public class SalesRepository : IFindRepository<Sale>
    {
        private readonly InchargeContext _context;
        public SalesRepository(InchargeContext context)
        {
            _context = context;
        }

        public Sale FindBy(Func<Sale, bool> predicate)
        {
            return _context.Sales
                .Include(x => x.Client)
                .Include(x => x.Employee)
                .Include(x => x.Product)
                .FirstOrDefault(predicate);
        }
        public List<Sale> ListBy(Func<Sale, bool>? predicate)
        {
            return _context.Sales
                .Include(x => x.Client)
                .Include(x => x.Employee)
                .Include(x => x.Product)
                .Where(predicate)
                .ToList();
        }
        public IQueryable<Sale> QueryBy(Func<Sale, bool> predicate)
        {
            return _context.Sales
                .Include(x => x.Client)
                .Include(x => x.Employee)
                .Include(x => x.Product)
                .Where(predicate)
                .AsQueryable();
        }
    }
}
