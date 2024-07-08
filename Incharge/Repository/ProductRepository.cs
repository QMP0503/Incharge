using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class ProductRepository:IFindRepository<Product>
    {
        private InchargeContext _context;
        public ProductRepository(InchargeContext context)
        {
            _context = context;
        }
        public List<Product> ListBy(Func<Product, bool> predicate)
        {
            return _context.Products
                .Include(x => x.Clients)
                .Include(x => x.ProductType)
                .Include(x => x.Sales)
                .Where(predicate)
                .ToList();
        }
        public Product FindBy(Func<Product, bool> predicate)
        {
            return _context.Products
                .Include(x => x.Clients)
                .Include(x => x.Sales)
                .Include(x => x.ProductType)
                .FirstOrDefault(predicate);
        }
        public IQueryable<Product> QueryBy(Func<Product, bool> predicate) 
        { 
            return _context.Products
                .Include(x => x.Clients)
                .Include(x => x.Sales)
                .Include(x => x.ProductType)
                .Where(predicate)
                .AsQueryable();
        }

    }
}
