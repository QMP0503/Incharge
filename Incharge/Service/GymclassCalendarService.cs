using AutoMapper;
using Incharge.ViewModels;
using Incharge.Models;
using Incharge.Repository.IRepository;
using Incharge.Service.IService;
using Incharge.ViewModels.Calendar;

namespace Incharge.Service
{
    public class GymclassCalendarService : IGymclassCalendarService
    {
        private readonly IFindRepository<Gymclass> _FindGymclassRepository;
        private readonly IFindRepository<Employee> _FindEmployeeRepository;
        private readonly IFindRepository<Location> _FindLocationRepository;
        private readonly IMapper _mapper;
        public GymclassCalendarService(IFindRepository<Gymclass> FindGymclassRepository, IMapper mapper, IFindRepository<Employee> FindEmployeeRepository, IFindRepository<Location> FindLocationRepository)
        {
            _FindGymclassRepository = FindGymclassRepository;
            _mapper = mapper;
            _FindEmployeeRepository = FindEmployeeRepository;
            _FindLocationRepository = FindLocationRepository;

        }

        public List<WeekdayItem> CreateItemList(string type ,string filter, DateTime selectedDate) //parameter date is by default current date but set for filter function
        {
            var weekayItemList = new List<WeekdayItem>();
            if(selectedDate == default(DateTime))
            {
                selectedDate = _FindGymclassRepository.QueryBy(x => x.Id > 0).OrderByDescending(x => x.Date).First().Date;
            }
            var monday = GetMonday(selectedDate);
            var sunday = GetSunday(selectedDate);
            IQueryable<Gymclass> weekClasses = null;

            //filter case
            switch (type.ToLower())
            {
                case "trainer":
                    var trainer = _FindEmployeeRepository.QueryBy(x => x.Role.Type == "Trainer" && x.Gymclasses.Any(g => g.Date >= monday && g.Date <= sunday));
                    if(trainer.Count() == 0) throw new Exception("No trainer with classes");
                    if (filter == null)
                    {
                        filter = $"{trainer.First().FirstName} {trainer.First().LastName}";
                    }
                    foreach (var item in trainer)
                    {
                        if (filter == $"{item.FirstName} {item.LastName}")
                        {
                            weekClasses = _FindGymclassRepository
                                .QueryBy(x => x.Employee.FirstName == item.FirstName && x.Employee.LastName == item.LastName && x.Date >= monday && x.Date <= sunday);
                            break;
                        }
                    }
                    break;
                case "location":
                    var location = _FindLocationRepository.QueryBy(x => x.Gymclasses.Count() > 0);
                    if(location.Count() == 0) throw new Exception("No Gymclass in any location");
                    if (filter == null)
                    {
                        filter = $"{location.First().Name}";
                    }
                    foreach (var item in location)
                    {
                        if (filter == $"{item.Name}")
                        {
                            weekClasses = _FindGymclassRepository
                                .QueryBy(x => x.Location.Name == item.Name && x.Date >= monday && x.Date <= sunday);
                            break;
                        }
                    }
                    break;

                default: 
                    throw new Exception("Invalid type");
                    
            }
            

            foreach (var item in weekClasses)
            {
                var startTime = item.Date.TimeOfDay;
                var timeSlots = AssignTimeSlots(item.Date, item.EndTime);
                weekayItemList.Add(new WeekdayItem()
                {
                    Item = _mapper.Map<GymClassVM>(item),
                    Weekday = item.Date.DayOfWeek.ToString(),
                    TimeSlots = timeSlots,
                    TimeStart = item.Date.TimeOfDay,
                    TimeEnd = item.EndTime.TimeOfDay,
                });
            }

            if (weekayItemList.Count == 0) throw new Exception("No gymclasses found.");

            //Last item in List will be dropdown option item
            weekayItemList.Add(DropDownOptions(filter, type, selectedDate));

            return weekayItemList;
        }
        public DateTime GetMonday(DateTime date)
        {
            int difference = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-difference).Date;
        }
        public DateTime GetSunday(DateTime date)
        {
            int difference = (7 + ((DayOfWeek.Sunday + 7) - date.DayOfWeek)) % 7; //Sunday is 0
            return date.AddDays(difference).Date;
        }
        public List<TimeSpan> AssignTimeSlots(DateTime start, DateTime end)
        {
            var timeSlots = new List<TimeSpan>();
            var roundStart = RoundTime(start);
            var roundEnd = RoundTime(end);
            while (roundStart < roundEnd)
            {
                timeSlots.Add(roundStart.TimeOfDay);
                roundStart = roundStart.AddMinutes(30);
            }
            return timeSlots;
        }
        public WeekdayItem DropDownOptions(string filter, string type, DateTime selectedDate)
        {
            var entity = new WeekdayItem()
            {
                Weekday = "DropDown"
            };

            switch (type.ToLower())
            {
                case "trainer":
                    var trainer = _FindEmployeeRepository.QueryBy(x => x.Role.Type == "Trainer");
                    foreach (var item in trainer)
                    {
                        entity.DropdownOption.Add($"{item.FirstName} {item.LastName}");
                    }
                    break;
                case "location":
                    var location = _FindLocationRepository.QueryBy(x => x.Gymclasses.Count > 0);
                    foreach (var item in location)
                    {
                        entity.DropdownOption.Add($"{item.Name}");
                    }
                    break;
                default:
                    throw new Exception("Invalid type");
            }

            if(filter != null)
            {
                var itemFilter = entity.DropdownOption.Find(x => x == filter);
                entity.DropdownOption.Remove(filter);
                entity.DropdownOption.Insert(0, itemFilter);
            }

            if(selectedDate != default(DateTime))
            {
                entity.DateFilter = selectedDate;
            }
            entity.MondayOfFilter = GetMonday(entity.DateFilter);
            entity.SundayOfFilter = GetSunday(entity.DateFilter);
             
            return entity;
        }
        public DateTime RoundTime(DateTime time)
        {
            var timeMinute = time.Minute;
            var SubtractTime = timeMinute % 30;
            return time.AddMinutes(-SubtractTime).AddSeconds(-time.Second).AddMicroseconds(-time.Microsecond);
        }
        
    }
}
