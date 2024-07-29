using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.Models;
using Incharge.ViewModels;
using Incharge.Migrations;

namespace Incharge.Service
{
    public class CheckerService : IChecker
    {
        private readonly IFindRepository<Client> _FindClientRepository;
        private readonly IRepository<Client> _ClientRepository;
        private readonly IRepository<Location> _LocationRepository;
        private readonly IFindRepository<Location> _FindLocationRepository;
        private readonly IFindRepository<Equipment> _FindEquipmentRepository;
        private readonly IRepository<Equipment> _EquipmentRepository;

        public CheckerService(IFindRepository<Client> FindClientRepository, IRepository<Client> ClientRepository, IFindRepository<Location> FindLocationRepository, IRepository<Location> LocationRepository, IFindRepository<Equipment> FindEquipmentRepository, IRepository<Equipment> EquipmentRepository)
        {
            _FindClientRepository = FindClientRepository;
            _ClientRepository = ClientRepository;
            _FindLocationRepository = FindLocationRepository;
            _LocationRepository = LocationRepository;
            _FindEquipmentRepository = FindEquipmentRepository;
            _EquipmentRepository = EquipmentRepository;

        }
        //seriously need to maek async
        public void ClientCheck()
        {
            var ListAllClients = _FindClientRepository.ListBy(x => x.Id > 0);
            foreach (var client in ListAllClients)
            {
                if (client.MembershipProductId != 0) //check if they even have a membership
                {
                    if (client.MembershipExpiryDate < DateTime.Now)
                    {
                        client.MembershipStatus = "Overdue";
                        _ClientRepository.Update(client);
                    }
                }
            }
            _ClientRepository.Save();
        }
        public void LocationCheck()
        {
            //find all location with class happening right now to change status to not available
            var checkLocationStatus = _FindLocationRepository.ListBy(x => x.Gymclasses.Any(y => y.Date <= DateTime.Now && y.EndTime >= DateTime.Now && y.Status == "Active"));
            foreach (var location in checkLocationStatus)
            {
                location.Status = "Unavailable";
                _LocationRepository.Update(location);
            }
            var checkLocationUnavailable = _FindLocationRepository.ListBy(x => x.Status == "Unavailable" && (x.Gymclasses.Any(y => y.Date <= DateTime.Now && y.EndTime >= DateTime.Now && y.Status == "Active") == false));
            foreach (var location in checkLocationUnavailable)
            {
                location.Status = "Available";
                _LocationRepository.Update(location);
            }

            _LocationRepository.Save();
        }
        public void EquipmentCheck()
        {
            var checkEquipmentStatus = _FindEquipmentRepository.ListBy(x => x.GymClasses.Any(y => y.Date <= DateTime.Now && y.EndTime >= DateTime.Now && y.Status == "Active"));

            foreach (var equipment in checkEquipmentStatus)
            {
                equipment.Status = "Reserved";
                _EquipmentRepository.Update(equipment);
            }

            var checkEquipmentReserved = _FindEquipmentRepository.ListBy(x => x.Status == "Reserved" && (x.GymClasses.Any(y => y.Date <= DateTime.Now && y.EndTime >= DateTime.Now && y.Status == "Active") == false));
            foreach (var equipment in checkEquipmentReserved)
            {
                equipment.Status = "Available";
                _EquipmentRepository.Update(equipment);
            }

            _EquipmentRepository.Save();
        }
    }
}
