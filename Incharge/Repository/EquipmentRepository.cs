using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class EquipmentRepository:IFindRepository<Equipment>
    {
        private readonly InchargeContext _context;

        public EquipmentRepository(InchargeContext context)
        {
            _context = context;
        }

        public Equipment FindBy(Func<Equipment, bool> predicate)
        {
            return _context.Equipment
                .Include(x => x.GymClass)
                .ThenInclude(x => x.Employee)
                .FirstOrDefault(predicate);
        }

        public List<Equipment> ListBy(Func<Equipment, bool> predicate)
        {
            return _context.Equipment
                .Include(x => x.GymClass)
                .ThenInclude(x => x.Employee)
                .Where(predicate)
                .ToList();
        }
        public IQueryable<Equipment> QueryBy(Func<Equipment, bool> predicate)
        {
            return _context.Equipment.Where(predicate).AsQueryable(); //for index paging method. 
        }
    }
}
