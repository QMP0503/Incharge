using Incharge.Models;


namespace Incharge.ViewModels
{
    public class HomeVM //List data for all to be displayed on home INDEX
    {
        public List<ClientVM> Clients { get; set; }
        public List<GymClassVM> GymClasses { get; set; }
        public List<EmployeeVM> Employees { get; set; } //should be trainers but thats fine
        public List<EquipmentVM> Equipment { get; set; }
        public List<LocationVM> Locations { get; set; }

    }
}
