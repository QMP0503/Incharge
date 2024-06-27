using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using K4os.Compression.LZ4;
using Incharge.ViewModels;
using ZstdSharp.Unsafe;
using Incharge.DTO;
using AutoMapper;

namespace Incharge.Service
{
    public class ClientService: IClientService //make sure surface interface use data from the same place.
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IMapper _mapper;
        public ClientService(IFindRepository<Client> FindClientRepository, IRepository<Client> clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _FindClientRepository = FindClientRepository;
            _mapper = mapper;
        }
        public ClientVM FindClient(ClientVM clientVM)
        {
            if (clientVM == null) { throw new NullReferenceException("client"); }
            var client = _FindClientRepository.FindBy(x => x.FirstName == clientVM.FirstName 
            || x.LastName == clientVM.LastName 
            || x.Email == clientVM.Email 
            || x.Phone == clientVM.Phone
            || x.Uuid == clientVM.Uuid);
            
            var clientFound = _mapper.Map<ClientVM>(client);
            return (clientFound);
        }
        public List<ClientDTO> ListClients(ClientVM clientVM)
        {
            if(clientVM == null) { throw new NullReferenceException("Input Empty."); }
            return new List<ClientDTO>();//_FindClientRepository.ListBy(null); //might need to find better way to get all.
        }
        public void AddClient(ClientVM clientVM)
        {
            var client = new Client
            {
                FirstName = clientVM.FirstName,
                LastName = clientVM.LastName,
                Email = clientVM.Email,
                Phone = clientVM.Phone
            };
            _clientRepository.Add(client) ;
            _clientRepository.Save();
        }
        public void EditClient(ClientVM clientVM) //email, phone, firstname and lastname are required
        {
            var clientToUpdate = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if (clientToUpdate == null) { throw new NullReferenceException("Client Empty."); }
            clientToUpdate.FirstName = clientVM.FirstName;
            clientToUpdate.LastName = clientVM.LastName;
            clientToUpdate.Email = clientVM.Email;
            clientToUpdate.Phone = clientVM.Phone;
            _clientRepository.Update(clientToUpdate);
            _clientRepository.Save();
        }
        public void DeleteClient(ClientVM clientVM)
        {
            if(clientVM == null) { throw new NullReferenceException("Input Empty."); }
            var clientToDelete = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if(clientToDelete == null) { throw new NullReferenceException("Client Empty."); }
            _clientRepository.Delete(clientToDelete);
            _clientRepository.Save();
        }
        public void UpdateStatus(ClientVM clientVM) //ONLY used when client have been found.
        {
            if (clientVM.Status == null && clientVM.Uuid==null) { throw new NullReferenceException("Input Empty."); }
            var client = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if (client == null) { throw new NullReferenceException("Client Empty."); }
            client.Status = clientVM.Status;
            _clientRepository.Update(client);
            _clientRepository.Save();
        }
        
    } //consider adding save after actiong is executed in controller.
}
