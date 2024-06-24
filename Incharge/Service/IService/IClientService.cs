using Incharge.Parameters;
using Incharge.Models;
using Incharge.ViewModel;
namespace Incharge.Service.IService
{
    public interface IClientService
    {
        //get
        public Client FindClient(ClientParam clientParam);
        public List<Client> ListClients(ClientParam clientParam);
        //create
        public void AddClient(ClientVM clientVM);
        //update (do when VM is created)
        public void EditClient(ClientVM clientVM);

        //AddMemberShip

        //EditMembership

        //delete
        public void RemoveClient(ClientVM clientVM);
    }
}
