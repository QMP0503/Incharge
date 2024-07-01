using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Service.IService;
using Incharge.Repository.IRepository;
using AutoMapper;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace Incharge.Service
{
    public class ProductService:IService<ProductVM, Product>
    {
        public readonly IRepository<Product> _ProductRepository;
        public readonly IFindRepository<Product> _FindProductRepository;
        public readonly IFindRepository<Producttype> _FindProductTypeRepository;
        public readonly IFindRepository<Client> _FindClientRepository;
        public readonly IFindRepository<Discount> _FindDiscountRepository;
        public readonly IMapper _Mapper;

        public ProductService(IRepository<Product> productRepo, IFindRepository<Product> findProductRepo,  IMapper mapper, IFindRepository<Producttype> findProductTypeRepo, IFindRepository<Client> findClientRepo, IFindRepository<Discount> findDiscountRepo)
        {
            _ProductRepository = productRepo;
            _Mapper = mapper;
            _FindProductRepository = findProductRepo;
            _FindClientRepository = findClientRepo;
            _FindDiscountRepository = findDiscountRepo;
            _FindProductTypeRepository = findProductTypeRepo;
        }

        public List<ProductVM> ListItem(Func<Product, bool> predicate)
        {
            var products = _FindProductRepository.ListBy(predicate);
            var productVMList = _Mapper.Map<List<ProductVM>>(products);
            return productVMList;
        }
        public ProductVM GetItem(Func<Product, bool> predicate)
        {
            var product = _FindProductRepository.FindBy(predicate);
            var productVM = _Mapper.Map<ProductVM>(product);
            return productVM;
        }
        public void AddService(ProductVM entity) //only add creation of new products
        {
            if (entity == null) throw new ArgumentNullException("Empty Input");
            if (entity.ProductTypeId == 0 && entity.ProductType==null) throw new ArgumentNullException("Empty productType, product need a type");
            var check = _FindProductRepository.FindBy(x => x.Name == entity.Name || x.Id == entity.Id);
            if (check != null) { throw new Exception("Product already exist"); }
            var productToAdd = new Product
            {
                Name = entity.Name
            };
            productToAdd.ProductType = _FindProductTypeRepository.FindBy(x => x.Id == entity.ProductTypeId);
            if (productToAdd.ProductType == null) { throw new Exception("ProductType don't exist"); }
            foreach (var discount in entity.DiscountsId)
            {
                var findDiscount = _FindDiscountRepository.FindBy(x => x.Id == discount);
                if (findDiscount != null) { productToAdd.Discounts.Add(findDiscount); }
            }

            productToAdd.TotalPrice = productToAdd.ProductType.Price * (double)productToAdd.Discounts.Sum(x => x.Discount1);
            //make sure to trouble shoot because this might have a ton of problems
            _ProductRepository.Add(productToAdd);
            _ProductRepository.Save();

        }
        public void UpdateService(ProductVM entity)//add new entries or edit existing entries into EXISTING products
        { //no sales in update or add service, sales will have its own class that adds itself to product-sales table
            if (entity == null) throw new ArgumentNullException("Empty Input");
            var product = _FindProductRepository.FindBy(x => x.Id == entity.Id);
            if (product == null) { throw new NullReferenceException("Product cannot be found."); }
            product.Name = entity.Name ?? product.Name;
            foreach (var client in entity.ClientsId) // will only add new clients who are not already apart of this product
            {
                var findClient = _FindClientRepository.FindBy(x => x.Id == client);
                if (findClient != null && product.Clients.Contains(findClient)) { product.Clients.Add(findClient); }
            }
            foreach (var discount in entity.DiscountsId)
            {
                var findDiscount = _FindDiscountRepository.FindBy(x => x.Id == discount);
                if(findDiscount != null && product.Discounts.Contains(findDiscount)) { product.Discounts.Add(findDiscount); }
            }
            product.ProductType = _FindProductTypeRepository.FindBy(x => x.Id == entity.ProductTypeId) ?? product.ProductType;
            product.TotalPrice = entity.TotalPrice * (double)product.Discounts.Sum(x => x.Discount1) ?? product.ProductType.Price * (double)product.Discounts.Sum(x => x.Discount1);
        }
        public void DeleteService(ProductVM entity)
        {
            if(entity == null) { throw new ArgumentNullException("Empty Input"); }
            var product = _FindProductRepository.FindBy(x => x.Id == entity.Id);
            if(product == null) { throw new NullReferenceException("Product cannot be found."); }
            _ProductRepository.Delete(product);
            _ProductRepository.Save();
        }
    }
}
