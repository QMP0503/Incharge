using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Data;

namespace Incharge.Repository
{
    public class EmployeeTypeRepository:IFindRepository<EmployeeType>
    {
        private readonly InchargeContext _context;
        public EmployeeTypeRepository(InchargeContext context)
        {
            _context = context;
        }
        public EmployeeType FindBy(Func<EmployeeType, bool> predicate)
        {
            return _context.EmployeeTypes.FirstOrDefault(predicate);
        }
        public List<EmployeeType> ListBy(Func<EmployeeType, bool> predicate)
        {
            return _context.EmployeeTypes.Where(predicate).ToList();
        }
        public IQueryable<EmployeeType> QueryBy(Func<EmployeeType, bool> predicate)
        {
            return _context.EmployeeTypes.Where(predicate).AsQueryable(); //for index paging method. 
        }
    }
}
