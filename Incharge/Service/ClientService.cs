using Incharge.Parameters;
using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using K4os.Compression.LZ4;
using Incharge.ViewModel;
using ZstdSharp.Unsafe;

namespace Incharge.Service
{
    public class ClientService: IClientService //make sure surface interface use data from the same place.
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<Client> _clientRepository;
        public ClientService(IFindRepository<Client> FindClientRepository, IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
            _FindClientRepository = FindClientRepository;
        }
        public Client FindClient(ClientParam clientParam)
        {
            if (clientParam == null) { throw new NullReferenceException("client"); }
            return _FindClientRepository.FindBy(x => x.FirstName == clientParam.FirstName 
            || x.LastName == clientParam.LastName 
            || x.Email == clientParam.Email 
            || x.Phone == clientParam.Phone
            || x.Id == clientParam.Id);
        }
        public List<Client> ListClients(ClientParam clientParam)
        {
            if(clientParam == null) { throw new NullReferenceException("Input Empty."); }
            return _FindClientRepository.ListBy(null); //might need to find better way to get all.
        }
        public void AddClient(ClientVM clientVM)
        {
            var t = new Client
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = clientVM.FirstName,
                LastName = clientVM.LastName,
                Email = clientVM.Email,
                Phone = clientVM.Phone
            };
            _clientRepository.Add(t) ;
            _clientRepository.Save();
        }
        public void EditClient(ClientVM clientVM) //email, phone, firstname and lastname are required
        {
            var clientToUpdate = _FindClientRepository.FindBy(x => (x.FirstName == clientVM.FirstName && x.LastName == clientVM.LastName) || (x.Email == clientVM.Email || x.Phone == clientVM.Phone));
            if(clientToUpdate == null) { throw new NullReferenceException("Client Empty."); }
            clientToUpdate.FirstName = clientVM.FirstName;
            clientToUpdate.LastName = clientVM.LastName;
            clientToUpdate.Email = clientVM.Email;
            clientToUpdate.Phone = clientVM.Phone;
            if(clientVM.Status != null) clientToUpdate.Status = clientVM.Status;
            _clientRepository.Update(clientToUpdate);
        }
        public void RemoveClient(ClientVM clientVM)
        {
            if(clientVM == null) { throw new NullReferenceException("Input Empty."); }
            var clientToDelete = _FindClientRepository.FindBy(x => x.FirstName == clientVM.FirstName && x.LastName == clientVM.LastName && (x.Email == clientVM.Email || x.Phone == clientVM.Phone));
            if(clientToDelete == null) { throw new NullReferenceException("Client Empty."); }
            _clientRepository.Delete(clientToDelete);
        }
        
    } //consider adding save after actiong is executed in controller.
}
