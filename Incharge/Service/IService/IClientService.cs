
using Incharge.Models;
using Incharge.ViewModels;
using Incharge.DTO;
using Incharge.ViewModels;
namespace Incharge.Service.IService
{
    public interface IClientService //only for client personal information
    {
        //get
        public ClientDTO FindClient(ClientVM clientVM);
        public List<ClientDTO> ListClients(ClientVM clientVM);
        //create
        public void AddClient(ClientVM clientVM);
        //update (do when VM is created)
        public void EditClient(ClientVM clientVM);

        //updateStatus - for checkin purposes
        public void UpdateStatus(ClientVM clientVM);

        //delete
        public void DeleteClient(ClientVM clientVM);
    }
}
