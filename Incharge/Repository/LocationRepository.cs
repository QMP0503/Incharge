using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class LocationRepository:IFindRepository<Location>
    {
        private readonly InchargeContext _context;
        public LocationRepository(InchargeContext context)
        {
            _context = context;
        }

        public List<Location> ListBy(Func<Location, bool> predicate)
        {
            return _context.Locations
                .Include(x => x.Gymclasses)
                .ThenInclude(x => x.Employee)
                .Where(predicate)
                .ToList();
        }
        public Location FindBy(Func<Location, bool> predicate)
        {
            return _context.Locations
                .Include(x => x.Gymclasses)
                .ThenInclude(x => x.Employee)
                .FirstOrDefault(predicate);
        }
    }
}
