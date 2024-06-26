using Incharge.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Incharge.Service.IService
{
    public interface IService<T,G> where T : class, new() where G : class
    {
        public List<T> ListItem(Func<G, bool> predicate); //test if automapper work with lists
        public T GetItem(Func<G, bool> predicate);
        public void AddService(T entity);
        public void UpdateService(T entity);
        public void DeleteService(T entity);
    }
}
