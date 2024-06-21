using Incharge.Data;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class EmployeeRepository : IFindRepository<Employee>
    {
        private readonly InchargeContext _context;
        public EmployeeRepository(InchargeContext context)
        {
            _context = context;
        }
        public Employee FindBy(Func<Employee, bool> predicate)
        {
            return _context.Employees
                .Include(x => x.Clients)
                .Include(x => x.Gymclasses)
                .ThenInclude(x => x.Equipment)
                .FirstOrDefault(predicate);
        }
        public List<Employee> ListBy(Func<Employee, bool> predicate)
        {
            return _context.Employees
                    .Include(x => x.Clients)
                    .Include(x => x.Gymclasses)
                    .ThenInclude(x => x.Equipment)
                    .Where(predicate)
                    .ToList();
        }
    }
}
