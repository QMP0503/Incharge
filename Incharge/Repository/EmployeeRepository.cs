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
				.ThenInclude(x => x.Location)
				.Include(x => x.Gymclasses)
				.ThenInclude(x => x.Equipment)
				.Include(x => x.Role)
				.FirstOrDefault(predicate);
        }
        public List<Employee> ListBy(Func<Employee, bool> predicate)
        {
            return _context.Employees
                    .Include(x => x.Clients)
                    .Include(x => x.Gymclasses)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.Gymclasses)
                    .ThenInclude(x => x.Equipment)
					.Include(x => x.Role)
					.Where(predicate)
                    .ToList();
        }
        public IQueryable<Employee> QueryBy(Func<Employee, bool> predicate)
        {
            return _context.Employees
                .Include(x=> x.Role)
                .Include(x => x.Clients)
                .Include(x => x.Gymclasses)
                .ThenInclude(x => x.Location)
                .Include(x => x.Gymclasses)
                .ThenInclude(x => x.Equipment)
                .Include(x => x.Role)
                .Where(predicate)
                .AsQueryable(); //for index paging method.
        }
    }
}
