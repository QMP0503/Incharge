namespace Incharge.Repository.IRepository
{
    public interface IFindRepository<T> where T : class
    {
        T FindBy(Func<T, bool> predicate);
        List<T> ListBy(Func<T, bool>? predicate);
    }
}
