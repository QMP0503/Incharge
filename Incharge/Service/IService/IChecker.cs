namespace Incharge.Service.IService
{
    public interface IChecker
    {
        //Checking status column of different tables
        public void ClientCheck();
        public void LocationCheck();
        public void EquipmentCheck();
    }
}