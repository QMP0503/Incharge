using Incharge.Parameters;
using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using K4os.Compression.LZ4;
using Incharge.ViewModel;

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
        public Client FindClient(ClientInfo clientInfo)
        {
            if (clientInfo == null) { throw new NullReferenceException("client"); }
            return _FindClientRepository.FindBy(x => x.FirstName == clientInfo.FirstName 
            && x.LastName == clientInfo.LastName 
            || x.Email == clientInfo.Email 
            || x.Phone == clientInfo.Phone);
        }
        public List<Client> ListClients(ClientInfo clientInfo)
        {
            if(clientInfo == null) { throw new NullReferenceException("Input Empty."); }
            return _FindClientRepository.ListBy(null); //might need to find better way to get all.
        }
        public void AddClient(ClientVM clientVM)
        {
            if (clientVM == null) { throw new NullReferenceException("Input Empty."); }
            _clientRepository.Add(new Client
            {
                FirstName = clientVM.FirstName,
                LastName = clientVM.LastName,
                Email = clientVM.Email,
                Phone = clientVM.Phone
            });
        }

        public void RemoveClient(ClientVM clientVM)
        {

        }
    }
}
