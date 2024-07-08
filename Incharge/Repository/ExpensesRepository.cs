using Incharge.Models;
using Incharge.Data;
using Incharge.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Incharge.Repository
{
    public class ExpensesRepository:IFindRepository<Expense>
    {
        private readonly InchargeContext _context;
        public ExpensesRepository(InchargeContext context)
        {
            _context = context;
        }
        public Expense FindBy(Func<Expense, bool> predicate)
        {
            return _context.Expenses.Include(x => x.BusinessReport).FirstOrDefault(predicate);
        }
        public List<Expense> ListBy(Func<Expense, bool>? predicate)
        {
            return _context.Expenses.Include(x => x.BusinessReport).Where(predicate).ToList();
        }
        public IQueryable<Expense> QueryBy(Func<Expense, bool> predicate)
        {
            return _context.Expenses.Include(x => x.BusinessReport).Where(predicate).AsQueryable();
        }
    }
}
