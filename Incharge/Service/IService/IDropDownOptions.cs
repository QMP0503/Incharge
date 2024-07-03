namespace Incharge.Service.IService
{
    public interface IDropDownOptions<T> where T : class //might be straight up redundant 
    {
        public T DropDownOptions();
    }
}
