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
    public class ClientService: IService<ClientVM, Client> //make sure surface interface use data from the same place.
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<Client> _ClientRepository;
        private readonly IFindRepository<Employee> _FindEmployeeRepository;
        private readonly IFindRepository<Gymclass> _FindGymClassRepository;
        private readonly IFindRepository<Product> _FindProductRepository;
        private readonly IFindRepository<Sale> _FindSaleRepository;
        private readonly IMapper _mapper;
        public ClientService(IFindRepository<Product> findProductRepository, IFindRepository<Sale> findSaleRepository, IFindRepository<Gymclass> findGymClassRepository, IFindRepository<Employee> findEmployeeRepository, IFindRepository<Client> FindClientRepository, IRepository<Client> clientRepository, IMapper mapper)
        {
            _ClientRepository = clientRepository;
            _FindClientRepository = FindClientRepository;
            _mapper = mapper;
            _FindEmployeeRepository = findEmployeeRepository;
            _FindGymClassRepository = findGymClassRepository;
            _FindProductRepository = findProductRepository;
            _FindSaleRepository = findSaleRepository;
        }
        public ClientVM GetItem(Func<Client, bool> predicate)
        {
            var client = _FindClientRepository.FindBy(predicate);
            if(client == null) { throw new NullReferenceException("Client Empty"); }
            return _mapper.Map<ClientVM>(client);
        }
        public List<ClientVM> ListItem(Func<Client, bool> predicate) //test if automapper work with lists
        {
            var client = _FindClientRepository.ListBy(predicate);
            if (client == null) { throw new NullReferenceException("Client Empty"); }
            return _mapper.Map<List<ClientVM>>(client);
        }
        public void AddService(ClientVM clientVM)
        {
            if(clientVM == null) { throw new NullReferenceException("Input Empty."); }
            var client = _mapper.Map<Client>(clientVM); //some is null so check if this is true
            _ClientRepository.Add(client) ;
            _ClientRepository.Save();
        }
        public void UpdateService(ClientVM clientVM) //email, phone, firstname and lastname are required
        {
            var clientToUpdate = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if (clientToUpdate == null) { throw new NullReferenceException("Client Empty."); }
            clientToUpdate.FirstName = clientVM.FirstName ?? clientToUpdate.FirstName;
            clientToUpdate.LastName = clientVM.LastName ?? clientToUpdate.LastName;
            clientToUpdate.Email = clientVM.Email ?? clientToUpdate.Email;
            clientToUpdate.Phone = clientVM.Phone ?? clientToUpdate.Phone;
            clientToUpdate.Status = clientVM.Status ?? clientToUpdate.Status;
            clientToUpdate.PaymentRecord = clientVM.PaymentRecord ?? clientToUpdate.PaymentRecord;
            if(clientVM.Sales != null)
            {
                foreach (var clientSales in clientVM.SalesID)
                {
                    var sales = _FindSaleRepository.FindBy(x => x.Id == clientSales);
                    clientToUpdate.Sales.Add(sales);
                }
            }
            if(clientVM.Products != null)
            {
                foreach (var clientProducts in clientVM.ProductID)
                {
                    var products = _FindProductRepository.FindBy(x => x.Id == clientProducts);
                    clientToUpdate.Products.Add(products);
                }
            }
            if(clientVM.Gymclasses != null)
            {
                foreach (var clientGymclasses in clientVM.GymClassID)
                {
                    var gymclasses = _FindGymClassRepository.FindBy(x => x.Id == clientGymclasses);
                    clientToUpdate.Gymclasses.Add(gymclasses);
                }
            }
            if(clientVM.Employees != null)
            {
                foreach (var clientEmployees in clientVM.EmployeeID)
                {
                    var employees = _FindEmployeeRepository.FindBy(x => x.Uuid == clientEmployees);
                    clientToUpdate.Employees.Add(employees);
                }
            }
            _ClientRepository.Update(clientToUpdate);
            _ClientRepository.Save();
        }
        public void DeleteService(ClientVM clientVM)
        {
            if(clientVM == null) { throw new NullReferenceException("Input Empty."); }
            var clientToDelete = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if(clientToDelete == null) { throw new NullReferenceException("Client Empty."); }
            _ClientRepository.Delete(clientToDelete);
            _ClientRepository.Save();
        }
        
    } //consider adding save after actiong is executed in controller.
}
