namespace Incharge.Service.IService
{
    public interface IChecker<T> where T : class
    {
        public void Check();
    }
}