using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using AutoMapper;
using System.Runtime.Serialization;

namespace Incharge.Service
{
    public class DiscountService : IService<DiscountVM, Discount>
    {
        readonly IFindRepository<Discount> _FindDiscountRepository;
        readonly IRepository<Discount> _DiscountRepository;
        readonly IMapper _mapper;

        public DiscountService(IMapper mapper, IFindRepository<Discount> FindDiscountRepository, IRepository<Discount> DiscountRepository)
        {
            _FindDiscountRepository = FindDiscountRepository;
            _DiscountRepository = DiscountRepository;
            _mapper = mapper;
        }
        public List<DiscountVM> ListItem(Func<Discount, bool> predicate)
        {
            var DiscountList = _FindDiscountRepository.ListBy(predicate);
            var DiscountVMList = _mapper.Map<List<DiscountVM>>(DiscountList);
            return DiscountVMList;
        }
        public DiscountVM GetItem(Func<Discount, bool> predicate)
        {
            var discount = _FindDiscountRepository.FindBy(predicate);
            var discountVM = _mapper.Map<DiscountVM>(discount);
            return discountVM;
        }
        public void AddService(DiscountVM entity)
        {
            var discountCheck = _FindDiscountRepository.FindBy(x => x.Name == entity.Name);
            if(discountCheck != null) { throw new Exception("Discount already exist."); }

            var discountToAdd = _mapper.Map<Discount>(entity);
            _DiscountRepository.Add(discountToAdd);
            _DiscountRepository.Save();
        }
        public void UpdateService(DiscountVM entity)
        {
            var discountToUpdate = _FindDiscountRepository.FindBy(x => x.Id == entity.Id);
            if(discountToUpdate == null) { throw new Exception("Discount don't exist."); }
            _mapper.Map(entity, discountToUpdate);
            _DiscountRepository.Update(discountToUpdate);
            _DiscountRepository.Save();
        }
        public void DeleteService(DiscountVM entity)
        {
            var discountToDelete = _FindDiscountRepository.FindBy(x => x.Id == entity.Id);
            if(discountToDelete == null) { throw new Exception("Discount don't exist."); }
            _DiscountRepository.Delete(discountToDelete);
            _DiscountRepository.Save();
        }
    }
}
