using Incharge.Service.IService;
using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.ViewModels;
using AutoMapper;
using System.Data;
namespace Incharge.Service
{
    public class SalesService:IService<SaleVM, Sale>, IDropDownOptions<SaleVM>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Product> _productRepository;


        private readonly IFindRepository<Sale> _findSaleRepository;
        private readonly IFindRepository<Client> _findClientRepository;
        private readonly IFindRepository<Product> _findProductRepository;
        private readonly IFindRepository<Employee> _findEmployeeRepository;

        public SalesService(IRepository<Employee> employeeRepository, IRepository<Product> productRepository, IMapper mapper, IRepository<Sale> saleRepository, IFindRepository<Sale> findSaleRepository, IFindRepository<Client> findClientRepository, IFindRepository<Product> findProductRepository, IFindRepository<Employee> findEmployeeRepository)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _findSaleRepository = findSaleRepository;
            _findClientRepository = findClientRepository;
            _findProductRepository = findProductRepository;
            _findEmployeeRepository = findEmployeeRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
        }

        public List<SaleVM> ListItem(Func<Sale, bool> predicate)
        {
            var saleList = _findSaleRepository.ListBy(predicate);
            var saleVMList = _mapper.Map<List<SaleVM>>(saleList); 
            return saleVMList;
        }
        public SaleVM GetItem(Func<Sale, bool> predicate)
        {
            var sale = _findSaleRepository.FindBy(predicate);
            var saleVM = _mapper.Map<SaleVM>(sale);
            return saleVM;
        }
        public void AddService(SaleVM entity)
        {
            if(entity == null) { throw new ArgumentNullException("Input Empty."); }
            var sale = _mapper.Map<Sale>(entity);
            if(entity.ClientId == 0) { throw new ArgumentNullException("Client Empty.");}
            if(entity.ProductId == 0) { throw new ArgumentNullException("Product Empty."); }
            if(entity.EmployeeId == 0) { throw new ArgumentNullException("Employee Empty."); }
            var product = _findProductRepository.FindBy(x => x.Id == entity.ProductId);
            if(product == null) { throw new Exception("Product don't exist."); }
            sale.Product = product;
            var employee = _findEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
            if(employee == null) { throw new ArgumentException("Employee don't exist."); }
            sale.Employee = employee;
            var client = _findClientRepository.FindBy(x => x.Id == entity.ClientId);
            if (client == null) { throw new Exception("Client don't exist."); }
            sale.Client = client;
            sale.Date = DateTime.Now; //track sales date

            if (product.ProductType.Name.Contains("Training"))
            {
                employee.Clients.Add(client);
                _employeeRepository.Update(employee);
            }

            product.Clients.Add(client);

            _saleRepository.Add(sale);
            _productRepository.Update(product);
            _saleRepository.Save();
            
        }
        public void UpdateService(SaleVM entity)//should NEVER use unless there is a massive error
        {
            if(entity == null) { throw new ArgumentNullException("Input Empty."); }

            var saleToUpdate = _findSaleRepository.FindBy(x => x.Uuid == entity.Uuid);
            if(saleToUpdate == null) { throw new Exception("Sale don't exist."); }

            var product = _findProductRepository.FindBy(x => x.Id == entity.ProductId);
            if (product == null) { throw new Exception("Product don't exist."); }

            var employee = _findEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
            if (employee == null) { throw new ArgumentException("Employee don't exist."); }
            
            var client = _findClientRepository.FindBy(x => x.Id == entity.ClientId);
            if (client == null) { throw new Exception("Client don't exist."); }
            

            if (product.ProductType.Name.Contains("Training"))
            {
                employee.Clients.Remove(saleToUpdate.Client);
                _employeeRepository.Update(employee);
            }

            saleToUpdate.Product = product;
            saleToUpdate.Client = client;
            saleToUpdate.Employee = employee;

            if (entity.Date != DateTime.MinValue) //incase it is null
            {
                saleToUpdate.Date = entity.Date;
            }

            _saleRepository.Update(saleToUpdate);
            _saleRepository.Save();
        }
        public void DeleteService(SaleVM entity)//ONLY USED IN THE EVENT OF HUGE ERROR.
        {
            if(entity == null) { throw new ArgumentNullException("Input Empty."); }
            var saleToDelete = _findSaleRepository.FindBy(x => x.Uuid == entity.Uuid);
            if(saleToDelete == null) { throw new Exception("Sale don't exist."); }
            _saleRepository.Delete(saleToDelete);
            _saleRepository.Save();
        }

        public SaleVM DropDownOptions() //for view purposes only
        {
            var salveVM = new SaleVM()
            {
                ClientOptions = _findClientRepository.ListBy(x => x.Id > 0),
                EmployeeOptions = _findEmployeeRepository.ListBy(x => x.Id > 0),
                ProductOptions = _findProductRepository.ListBy(x => x.Id > 0)
            };

            return salveVM;
        }
    }
}
