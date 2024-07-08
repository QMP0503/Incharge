using Incharge.Repository.IRepository;
using Incharge.Data;
using Incharge.Models;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class DiscountRepository:IFindRepository<Discount>
    {
        private readonly InchargeContext _context;
        public DiscountRepository(InchargeContext context)
        {
            _context = context;
        }

        public Discount FindBy(Func<Discount, bool> predicate)
        {
            return _context.Discounts.Include(x => x.Sales).FirstOrDefault(predicate);
        }
        public List<Discount> ListBy(Func<Discount, bool>? predicate)
        {
            return _context.Discounts.Include(x => x.Sales).Where(predicate).ToList();
        }
        public IQueryable<Discount> QueryBy(Func<Discount, bool> predicate)
        {
            return _context.Discounts.Include(x => x.Sales).Where(predicate).AsQueryable();
        }

    }
}
