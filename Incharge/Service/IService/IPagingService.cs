using Incharge.Service;
namespace Incharge.Service.IService
{
    public interface IPagingService<T> where T : class
    {
        T IndexPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber);
    }
}
