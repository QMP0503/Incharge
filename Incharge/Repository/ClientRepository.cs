using Incharge.Data;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class ClientRepository: IFindRepository<Client>
    {
        private readonly InchargeContext _context;
        public ClientRepository(InchargeContext context) 
        {
            _context = context;
        }
        public Client FindBy(Func<Client, bool> predicate)
        {
            return _context.Clients
                .Include(x => x.Sales)
                .Include(x => x.Employees)
                .Include(x => x.Gymclasses) //Include location if direct link to gymclass page doesn't work
                .ThenInclude (x => x.Location)
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductType)
                .Include(x => x.PaymentRecord)
                .FirstOrDefault(predicate);
        }
        public List<Client> ListBy(Func<Client, bool>? predicate)
        {
            return _context.Clients
                .Include(x => x.Sales)
                .Include(x => x.Employees)
                .Include(x => x.Gymclasses) //Include location if direct link to gymclass page doesn't work
                .ThenInclude(x => x.Location)
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductType)
                .Include(x => x.PaymentRecord)
                .Where(predicate) //if theres issue use LinQ command in seperate method
                .ToList();
        }
        public IQueryable<Client> QueryBy(Func<Client, bool> predicate)
        {
            return _context.Clients
                .Include(x => x.Sales)
                .Include(x => x.Employees)
                .Include(x => x.Gymclasses) //Include location if direct link to gymclass page doesn't work
                .ThenInclude(x => x.Location)
                .Include(x => x.Products)
                .ThenInclude(x => x.ProductType)
                .Include(x => x.PaymentRecord)
                .Where(predicate)
                .AsQueryable(); //for index paging method.
        }
    }
}
