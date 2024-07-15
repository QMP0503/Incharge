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
        private readonly IPhotoService _PhotoService;    
        public LocationService(IPhotoService photoService,IMapper mapper, IFindRepository<Location> findLocationRepository, IRepository<Location> locationRepository, IFindRepository<Gymclass> findGymClassRepository)
        {
            _FindLocationRepository = findLocationRepository;
            _LocationRepository = locationRepository;
            _FindGymClassRepository = findGymClassRepository;
            _Mapper = mapper;
            _PhotoService = photoService;
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
            if(locationVM == null) throw new ArgumentNullException("Location input don't exist");
            if(locationVM.ImageFile != null)
            {
                var result = _PhotoService.AddPhotoAsync(locationVM.ImageFile).Result;
                locationVM.Image = result.Url.ToString();
            }
            locationVM.Status = "Available";
            var location = _Mapper.Map<Location>(locationVM);
            _LocationRepository.Add(location);
            _LocationRepository.Save();
        }
        public void UpdateService(LocationVM LocationVM) //test with mapper in the event that entities are null
        {
            if(LocationVM == null) throw new ArgumentNullException("location don't exist");
            var location = _FindLocationRepository.FindBy(x => x.Id == LocationVM.Id);
            if(location == null) throw new ArgumentNullException("location don't exist");
            if(LocationVM.ImageFile != null)
            {
                if(location.Image != null)
                {
                    var delete = _PhotoService.DeletePhotoAsync(location.Image).Result;
                }
                var result = _PhotoService.AddPhotoAsync(LocationVM.ImageFile).Result;
                LocationVM.Image = result.Url.ToString();
            }
            _Mapper.Map(LocationVM, location);
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
