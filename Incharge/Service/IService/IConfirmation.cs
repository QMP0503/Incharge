using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Incharge.Service.IService
{
    public interface IConfirmation <T> where T : class
    {
        public T paymentConfirmation(T entity);
    }
}
