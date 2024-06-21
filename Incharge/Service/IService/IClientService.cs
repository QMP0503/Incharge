using Incharge.Parameters;
using Incharge.Models;
using Incharge.ViewModel;
namespace Incharge.Service.IService
{
    public interface IClientService
    {
        //get
        public Client FindClient(ClientInfo clientInfo);
        public List<Client> ListClients(ClientInfo clientInfo);
        //create
        public void AddClient(ClientVM clientVM);
        //update (do when VM is created)

        //AddMemberShip

        //EditMembership

        //delete
        public void RemoveClient(ClientVM clientVM);
    }
}
