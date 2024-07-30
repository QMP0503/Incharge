using Incharge.Service.IService;
using Incharge.Repository.IRepository;
using Incharge.Models;
using Incharge.ViewModels;
using AutoMapper;


namespace Incharge.Service
{
    public class SalesService:IService<SaleVM, Sale>, IDropDownOptions<SaleVM>, IConfirmation<SaleVM>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Client> _clientRepository;

        private readonly IFindRepository<Sale> _findSaleRepository;
        private readonly IFindRepository<Client> _findClientRepository;
        private readonly IFindRepository<Product> _findProductRepository;
        private readonly IFindRepository<Employee> _findEmployeeRepository;
        private readonly IFindRepository<Discount> _findDiscountRepository;
        private readonly IFindRepository<BusinessReport> _FindBusinessReportRepository;

        public SalesService(IFindRepository<BusinessReport> FindBusinessReportRepository, IRepository<Client> clientRepository, IRepository<Employee> employeeRepository, IRepository<Product> productRepository, IMapper mapper, IRepository<Sale> saleRepository, IFindRepository<Sale> findSaleRepository, IFindRepository<Client> findClientRepository, IFindRepository<Product> findProductRepository, IFindRepository<Employee> findEmployeeRepository, IFindRepository<Discount> findDiscountRepository)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _findSaleRepository = findSaleRepository;
            _findClientRepository = findClientRepository;
            _findProductRepository = findProductRepository;
            _findEmployeeRepository = findEmployeeRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
            _findDiscountRepository = findDiscountRepository;
			_clientRepository = clientRepository;
			_FindBusinessReportRepository = FindBusinessReportRepository;
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
        public void AddService(SaleVM entity) //NEED TO ADD DISCOUNTS
        {
            if (entity.Quantity == 0) { throw new ArgumentNullException("Quantity is 0"); }
            var sale = _mapper.Map<Sale>(entity);
            if (entity.ClientId == 0) { throw new ArgumentNullException("Client Empty."); }
            if (entity.ProductId == 0) { throw new ArgumentNullException("Product Empty."); }
            if (entity.EmployeeId == 0) { throw new ArgumentNullException("Employee Empty."); }

            var product = _findProductRepository.FindBy(x => x.Id == entity.ProductId);
            if (product == null) { throw new Exception("Product don't exist."); }
            sale.Product = product;

            var employee = _findEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
            if (employee == null) { throw new ArgumentException("Employee don't exist."); }
            sale.Employee = employee;

            var client = _findClientRepository.FindBy(x => x.Id == entity.ClientId);
            if (client == null) { throw new Exception("Client don't exist."); }
            sale.Client = client;

            //find a way to add tax (think it will be set by employee)
            sale.Date = DateTime.Now; //track sales date

            //CHECKING FOR DUPLICATE MEMBERSHIP
            if (client.MembershipStatus == "Active")
            {
                throw new Exception("Client already have this product.");
            }

            if (product.ProductType.Name.Contains("Training"))
            {
                employee.Clients.Add(client);
                _employeeRepository.Update(employee);
                client.TotalTrainingSessions += sale.Quantity; //track total training sessions
            }

            //CLIENT CAN ONLY BUY 1 MONTH MEMBERSHIP!
            if (product.ProductType.Name.Contains("Membership"))
            {
                sale.Quantity = 1;
                client.MembershipStatus = "Active";
                client.MembershipExpiryDate = sale.Date.AddMonths(1); //Set number of months members bought
                client.MembershipName = product.Name;
                client.MembershipProductId = product.Id;
                client.MembershipStartDate = sale.Date;
            }

            //DISCOUNTS and pricing
            if (entity.DiscountId != null)
            {
                foreach (var discountId in entity.DiscountId)
                {
                    var dsicount = _findDiscountRepository.FindBy(x => x.Id == discountId);
                    sale.Discounts.Add(dsicount);
                }
                var discountSum = sale.Discounts.Sum(x => x.DiscountValue);
                var price = product.ProductType.Price * sale.Quantity;
                sale.TotalPrice = price - (price * discountSum);
            }
            else
            {
                sale.TotalPrice = product.ProductType.Price * sale.Quantity;
            }


            //adding product to client
            client.Products.Add(product);
            _clientRepository.Update(client);

            product.Clients.Add(client);

            //Add product to business report
            var businessReport = _FindBusinessReportRepository.FindBy(x => x.Date.Month == sale.Date.Month && x.Date.Year == sale.Date.Year);
            sale.BusinessReport = businessReport;
            sale.BusinessReportId = businessReport.Id;

            _saleRepository.Add(sale);
            _productRepository.Update(product);
            _saleRepository.Save();

        }
        public void UpdateService(SaleVM entity)//should NEVER use unless there is a massive error
        {
             
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
                employee.Clients.Add(client);
                _employeeRepository.Update(employee);
            }

            
            saleToUpdate.Product = product;
            saleToUpdate.Client = client;
            saleToUpdate.Employee = employee;
            saleToUpdate.PaymentType = entity.PaymentType;

            if (entity.Date != DateTime.MinValue) //incase it is null
            {
                saleToUpdate.Date = entity.Date;
            }

            List<Discount> discount = new List<Discount>();

            if (entity.DiscountId != null) //if theres none - ignore
            {
                
                foreach (var discountId in entity.DiscountId)
                {
                    var discountToAdd = _findDiscountRepository.FindBy(x => x.Id == discountId);
                    if (discountToAdd == null) { throw new Exception("discount don't exist"); } //delete once complete cause this should never happen
                    discount.Add(discountToAdd);
                }
                saleToUpdate.Discounts.Clear();

                foreach (var discountToAdd in discount)
                {
                    saleToUpdate.Discounts.Add(discountToAdd);
                }
            }

            var totalDiscount = discount.Sum(x => x.DiscountValue);
            if (totalDiscount >= 1) { throw new Exception("Discount cannot be equal or greater than 100%"); }


            if (totalDiscount > 0)
            {
                saleToUpdate.TotalPrice = product.ProductType.Price * totalDiscount;
            }
            else
            {
                saleToUpdate.TotalPrice = product.ProductType.Price;
            }

            _saleRepository.Update(saleToUpdate);
            
        }
        public void DeleteService(SaleVM entity)//ONLY USED IN THE EVENT OF HUGE ERROR.
        {
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
                ProductOptions = _findProductRepository.ListBy(x => x.Id > 0),
                DiscountOptions = _findDiscountRepository.ListBy(x => x.Id > 0)
            };

            return salveVM;
        }

        public SaleVM paymentConfirmation(SaleVM entity)
        {
            if (entity.Quantity == 0) { throw new ArgumentNullException("Quantity is 0"); }
            
            if (entity.ClientId == 0) { throw new ArgumentNullException("Client Empty."); }
            if (entity.ProductId == 0) { throw new ArgumentNullException("Product Empty."); }
            if (entity.EmployeeId == 0) { throw new ArgumentNullException("Employee Empty."); }

            var product = _findProductRepository.FindBy(x => x.Id == entity.ProductId);
            if (product == null) { throw new Exception("Product don't exist."); }
            entity.Product = product;

            var employee = _findEmployeeRepository.FindBy(x => x.Id == entity.EmployeeId);
            if (employee == null) { throw new ArgumentException("Employee don't exist."); }
            entity.Employee = employee;

            var client = _findClientRepository.FindBy(x => x.Id == entity.ClientId);
            if (client == null) { throw new Exception("Client don't exist."); }

            entity.Client = client;

            //find a way to add tax (think it will be set by employee)
            entity.Date = DateTime.Now; //track sales date

            //CHECKING FOR DUPLICATE MEMBERSHIP
            if (client.MembershipStatus == "Active")
            {
                throw new Exception("Client already have this product.");
            }

            //might change later
            if(entity.Quantity>1 && entity.Product.ProductType.Name.Contains("Membership"))
            {
                throw new Exception("Membership can only be bought for one month at a time");
            }

            //DISCOUNTS and pricing
            if (entity.DiscountId != null)
            {
                foreach (var discountId in entity.DiscountId)
                {
                    var discount = _findDiscountRepository.FindBy(x => x.Id == discountId);
                    entity.Discount.Add(discount);
                }
                var discountSum = entity.Discount.Sum(x => x.DiscountValue);
                if (discountSum >= 1) { throw new Exception("Discount cannot be equal or greater than 100%"); }
                var price = product.ProductType.Price * entity.Quantity;
                entity.TotalPrice = price - (price * discountSum);
            }
            else
            {
                entity.TotalPrice = product.ProductType.Price * entity.Quantity;
            }

            return entity;
        }
     
    }
}
