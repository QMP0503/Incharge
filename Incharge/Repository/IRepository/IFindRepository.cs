using System.Linq;

namespace Incharge.Repository.IRepository
{
    public interface IFindRepository<T> where T : class //make classes Async when possible
    {
        T FindBy(Func<T, bool> predicate);
        List<T> ListBy(Func<T, bool>? predicate);
        IQueryable<T> QueryBy(Func<T, bool> predicate);
    }
}
