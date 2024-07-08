using System.Linq;

namespace Incharge.Repository.IRepository
{
    public interface IFindRepository<T> where T : class //make classes Async when possible
    {
        public T FindBy(Func<T, bool> predicate);
        public List<T> ListBy(Func<T, bool>? predicate);
        public IQueryable<T> QueryBy(Func<T, bool> predicate);
    }
}
