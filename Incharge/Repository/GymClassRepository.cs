using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class GymClassRepository: IFindRepository<Gymclass>
    {
        private readonly InchargeContext _context;

        public GymClassRepository(InchargeContext context)
        {
            _context = context;
        }

        public Gymclass FindBy(Func<Gymclass, bool> predicate)
        {
            return _context.Gymclasses
                .Include(x => x.Equipment)
                .Include(x => x.Location)
                .Include(x => x.Clients)
                .Include(x => x.Employee)
                .FirstOrDefault(predicate);
        }

        public List<Gymclass> ListBy(Func<Gymclass, bool> predicate)
        {
            return _context.Gymclasses
                .Include(x => x.Equipment)
                .Include(x => x.Location)
                .Include(x => x.Clients)
                .Include(x => x.Employee)
                .Where(predicate)
                .ToList();
        }

    }
}
