using Incharge.Service.IService;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.ViewModels;
using AutoMapper;
using static Dapper.SqlMapper;


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
            //mapping gym classes
            //var gymClasses = _FindGymClassRepository.ListBy(x => x.Clients.Contains(client));
            //foreach(var gymClass in gymClasses)
            //{
            //    if(gymClass != null)
            //    {
            //        clientVM.Gymclasses.Add(gymClass);
            //    }
            //}

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

            //checking phone and email
            PhoneChecker(clientVM);
            EmailChecker(clientVM);

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
            if (clientToUpdate == null) { throw new NullReferenceException("Client Empty."); }

            //checking phone and email

            if (clientVM.Phone != clientToUpdate.Phone)
            {
                PhoneChecker(clientVM);
            }
            if (clientVM.Email != clientToUpdate.Email)
            {
                EmailChecker(clientVM);
            }


            if (clientVM.PicutreInput != null)
            {
                if (clientToUpdate.ProfilePicture != null && clientVM.PicutreInput != null && clientToUpdate.ProfilePicture != "https://res.cloudinary.com/dmmlhlebe/image/upload/v1721294595/default_z7dhuq.png")
                {
                    var delete = _photoService.DeletePhotoAsync(clientToUpdate.ProfilePicture).Result;
                    Console.WriteLine(delete.ToString());
                }

                var result = _photoService.AddPhotoAsync(clientVM.PicutreInput).Result;
                clientVM.ProfilePicture = result.Url.ToString();
            }
            
			var phone = clientToUpdate.Phone;
            var membershipStatus = clientToUpdate.MembershipStatus;
            var membershipType = clientToUpdate.MembershipName;
            var membershipStartDate = clientToUpdate.MembershipStartDate;
            var membershipEndDate = clientToUpdate.MembershipExpiryDate;
            var TotalTrainingSessions = clientToUpdate.TotalTrainingSessions;

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
            

            if (string.IsNullOrEmpty(clientToUpdate.MembershipStatus))
            {
                clientToUpdate.MembershipStatus = membershipStatus;
            }

            if (string.IsNullOrEmpty(clientToUpdate.MembershipName))
            {
                clientToUpdate.MembershipName = membershipType;
            }

            if (clientToUpdate.MembershipStartDate == default(DateTime))
            {
                clientToUpdate.MembershipStartDate = membershipStartDate;
            }

            if (clientToUpdate.MembershipExpiryDate == default(DateTime))
            {
                clientToUpdate.MembershipExpiryDate = membershipEndDate;
            }
            clientToUpdate.TotalTrainingSessions = TotalTrainingSessions;

            if (clientVM.Gymclasses != null)
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
            if(clientToDelete.ProfilePicture != null && clientToDelete.ProfilePicture != "https://res.cloudinary.com/dmmlhlebe/image/upload/v1721294595/default_z7dhuq.png")
            {
                var delete = _photoService.DeletePhotoAsync(clientToDelete.ProfilePicture).Result;
            }
            _ClientRepository.Delete(clientToDelete);
            _ClientRepository.Save();
        }

        //Make generic method tmr 
        public void PhoneChecker(ClientVM entity)
        {
            //phone checker
            var clientPhoneCheck = _FindClientRepository.FindBy(x => x.Phone == entity.Phone);
            var employeePhoneCheck = _FindEmployeeRepository.FindBy(x => x.Phone == entity.Phone);
            if (clientPhoneCheck != null || employeePhoneCheck != null) { throw new Exception("Phone number already exist."); }

        }
        public void EmailChecker(ClientVM entity)
        {
            //email checker
            var clientEmailCheck = _FindClientRepository.FindBy(x => x.Email == entity.Email);
            var employeeEmailCheck = _FindEmployeeRepository.FindBy(x => x.Email == entity.Email);
            if (clientEmailCheck != null || employeeEmailCheck != null) { throw new Exception("Email already exist."); }
        }



    } //consider adding save after actiong is executed in controller.
}
