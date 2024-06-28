using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;

namespace Incharge.Repository
{
    public class ProductTypeRepository:IFindRepository<Producttype>
    {
        private readonly InchargeContext _context;
        public ProductTypeRepository(InchargeContext context)
        {
            _context = context;
        }
        public Producttype FindBy(Func<Producttype, bool> predicate)
        {
            return _context.Producttypes
                .Include(x => x.Products)
                .ThenInclude(x => x.Clients)
                .FirstOrDefault(predicate);
        }
        public List<Producttype> ListBy(Func<Producttype, bool>? predicate)
        {
            return _context.Producttypes
                .Include(x => x.Products)
                .ThenInclude(x => x.Clients)
                .Where(predicate)
                .ToList();
        }
        public IQueryable<Producttype> QueryBy(Func<Producttype, bool> predicate)
        {
            return _context.Producttypes
                .Include(x => x.Products)
                .ThenInclude(x => x.Clients)
                .Where(predicate)
                .AsQueryable();
            
        }
    }
}
