using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.ViewModels;
using Incharge.Models;
using AutoMapper;

namespace Incharge.Service
{
    public class LocationService : IService<LocationVM, Location>
    {
        private readonly IFindRepository<Location> _FindLocationRepository;
        private readonly IRepository<Location> _LocationRepository;
        private readonly IFindRepository<Gymclass> _FindGymClassRepository;
        private readonly IMapper _Mapper;
        public LocationService(IMapper mapper, IFindRepository<Location> findLocationRepository, IRepository<Location> locationRepository, IFindRepository<Gymclass> findGymClassRepository)
        {
            _FindLocationRepository = findLocationRepository;
            _LocationRepository = locationRepository;
            _FindGymClassRepository = findGymClassRepository;
            _Mapper = mapper;
        }
        public List<LocationVM> ListItem(Func<Location, bool> predicate)
        {
            var locationList = _FindLocationRepository.ListBy(predicate);
            var locationVMList = _Mapper.Map<List<LocationVM>>(locationList);
            return locationVMList;
        }
        public LocationVM GetItem(Func<Location, bool> predicate)
        {
            var location = _FindLocationRepository.FindBy(predicate);
            if(location == null) { throw new ArgumentNullException("Location don't exist"); }
            var locationVM = _Mapper.Map<LocationVM>(location);
            return locationVM;
        }
        public void AddService(LocationVM locationVM)
        {
            if(locationVM == null) throw new ArgumentNullException("location don't exist");
            var location = _Mapper.Map<Location>(locationVM);
            _LocationRepository.Add(location);
            _LocationRepository.Save();
        }
        public void UpdateService(LocationVM LocationVM) //test with mapper in the event that entities are null
        {
            if(LocationVM == null) throw new ArgumentNullException("location don't exist");
            var location = _FindLocationRepository.FindBy(x => x.Id == LocationVM.Id);
            if(location == null) throw new ArgumentNullException("location don't exist");
            location.Name = LocationVM.Name ?? location.Name;
            location.Status = LocationVM.Status ?? location.Status;
            location.Description = LocationVM.Description ?? location.Description;
            location.Capacity = LocationVM.Capacity ?? location.Capacity;
            if(LocationVM.Gymclasses != null)
            {
                foreach(var gymclass in LocationVM.Gymclasses)
                {
                    var gymclassToAdd = _FindGymClassRepository.FindBy(x => x.Id == gymclass.Id);
                    location.Gymclasses.Add(gymclassToAdd);
                }
            }
            _LocationRepository.Update(location);
            _LocationRepository.Save();
        }
        public void DeleteService(LocationVM LocationVM)
        {
            if (LocationVM == null) throw new ArgumentNullException("location don't exist");
            var location = _FindLocationRepository.FindBy(x => x.Id == LocationVM.Id);
            if(location == null) throw new ArgumentNullException("location don't exist");
            _LocationRepository.Delete(location);
            _LocationRepository.Save();
        }
    }
}
