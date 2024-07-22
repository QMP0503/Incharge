using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using K4os.Compression.LZ4;
using Incharge.ViewModels;
using ZstdSharp.Unsafe; 
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Net.WebRequestMethods;

namespace Incharge.Service
{
    public class ClientService: IService<ClientVM, Client>//make sure surface interface use data from the same place.
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<Client> _ClientRepository;
        private readonly IFindRepository<Employee> _FindEmployeeRepository;
        private readonly IFindRepository<Gymclass> _FindGymClassRepository;
        private readonly IFindRepository<Product> _FindProductRepository;
        private readonly IFindRepository<Sale> _FindSaleRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public ClientService(IPhotoService photoService, IFindRepository<Product> findProductRepository, IFindRepository<Sale> findSaleRepository, IFindRepository<Gymclass> findGymClassRepository, IFindRepository<Employee> findEmployeeRepository, IFindRepository<Client> FindClientRepository, IRepository<Client> clientRepository, IMapper mapper)
        {
            _ClientRepository = clientRepository;
            _FindClientRepository = FindClientRepository;
            _mapper = mapper;
            _FindEmployeeRepository = findEmployeeRepository;
            _FindGymClassRepository = findGymClassRepository;
            _FindProductRepository = findProductRepository;
            _FindSaleRepository = findSaleRepository;
            _photoService = photoService;
        }
        public ClientVM GetItem(Func<Client, bool> predicate)
        {
            var client = _FindClientRepository.FindBy(predicate);
            if(client == null) { throw new NullReferenceException("Client Empty"); }
            var clientVM = _mapper.Map<ClientVM>(client);
            if(client.Products.FirstOrDefault(x => x.ProductType.Name.Equals("Membership")) != null)
            {
				clientVM.GymMembership = client.Products.FirstOrDefault(x => x.ProductType.Name.Equals("Membership")); //this is kinda stupid but oh well haha
			}
            return clientVM;
        }
        public List<ClientVM> ListItem(Func<Client, bool> predicate) //test if automapper work with lists
        {
            var client = _FindClientRepository.ListBy(predicate);
            if (client == null) { throw new NullReferenceException("Client Empty"); }
            return _mapper.Map<List<ClientVM>>(client);
        }
        public void AddService(ClientVM clientVM) //cosider if there is a desire to add membership as soon as client profile is made..
        {
            if(clientVM == null || clientVM.FirstName == null || clientVM.LastName == null) { throw new NullReferenceException("Input Empty."); }
            var client = _mapper.Map<Client>(clientVM);
            if (clientVM.PicutreInput != null)
            {
                var result = _photoService.AddPhotoAsync(clientVM.PicutreInput).Result;
                client.ProfilePicture = result.Url.ToString();

            }
            else
            {
                client.ProfilePicture = "https://res.cloudinary.com/dmmlhlebe/image/upload/v1721294595/default_z7dhuq.png";
            }
            client.MembershipStatus = "No Membership";
            //some is null so check if this is true
            _ClientRepository.Add(client) ;
            _ClientRepository.Save();
        }
        public void UpdateService(ClientVM clientVM) //email, phone, firstname and lastname are required
        {
            var clientToUpdate = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if (clientVM.PicutreInput != null)
            {
                if (clientToUpdate.ProfilePicture != null && clientVM.PicutreInput != null)
                {
                    var delete = _photoService.DeletePhotoAsync(clientToUpdate.ProfilePicture).Result;
                    if (clientToUpdate == null) { throw new NullReferenceException("Client Empty."); }
                    Console.WriteLine(delete.ToString());
                }

                var result = _photoService.AddPhotoAsync(clientVM.PicutreInput).Result;
                clientVM.ProfilePicture = result.Url.ToString();
            }
            
			var phone = clientToUpdate.Phone;
			_mapper.Map(clientVM, clientToUpdate);
            if(clientToUpdate.Phone == 0) { clientToUpdate.Phone = phone; }

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

            //need a method to add gym membership record and when the gym membership would expire.
            //Find way to simulate different times to test the result for demo.

            _ClientRepository.Update(clientToUpdate);
            _ClientRepository.Save();
        }
        public void DeleteService(ClientVM clientVM)
        {
            if(clientVM == null) { throw new NullReferenceException("Input Empty."); }
            var clientToDelete = _FindClientRepository.FindBy(x => x.Uuid == clientVM.Uuid);
            if(clientToDelete == null) { throw new NullReferenceException("Client Empty."); }
			var delete = _photoService.DeletePhotoAsync(clientToDelete.ProfilePicture).Result;
			_ClientRepository.Delete(clientToDelete);
            _ClientRepository.Save();
        }

        //public DateTime GetEndDate(ClientVM entity)
        //{
        //    var product = _FindProductRepository.FindBy(x => x.ProductType.Id == entity.GymMembership.ProductTypeId);
        //    if (product == null) { throw new NullReferenceException("ProductType cannot be found."); }
        //    TimeSpan duration = TimeSpan.FromDays(1); //for trial membership

        //    //could use switch case
        //    if (product.ProductType.Recurance == "Weekly")
        //    {
        //        duration = TimeSpan.FromDays(7);
        //    }
        //    if (product.ProductType.Recurance == "Monthly")
        //    {
        //        duration = TimeSpan.FromDays(30); //assuming regular month
        //    }
        //    if (product.ProductType.Recurance == "Yearly")
        //    {
        //        duration = TimeSpan.FromDays(365); //assuming regular year
        //    }
        //    var EndDate = duration;

        //    return DateTime.Now.AddDays(EndDate.Days);
        //}


    } //consider adding save after actiong is executed in controller.
}
