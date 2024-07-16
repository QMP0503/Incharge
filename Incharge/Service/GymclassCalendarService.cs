using AutoMapper;
using Incharge.ViewModels;
using Incharge.Models;
using Incharge.Repository.IRepository;
using System.Globalization;
using System.Runtime.InteropServices;
using Incharge.Service.IService;
using Incharge.ViewModels.Calendar;

namespace Incharge.Service
{
    public class GymclassCalendarService : IGymclassCalendarService
    {
        private readonly IFindRepository<Gymclass> _FindGymclassRepository;
        private readonly IMapper _mapper;
        public GymclassCalendarService(IFindRepository<Gymclass> FindGymclassRepository, IMapper mapper)
        {
            _FindGymclassRepository = FindGymclassRepository;
            _mapper = mapper;
        }

        public List<WeekdayItem> CreateItemList() //parameter date is by default current date but set for filter function
        {
            var weekayItemList = new List<WeekdayItem>();
            var now = DateTime.Now.Date;
            var monday = GetMonday(now);
            var sunday = GetSunday(now);
            var weekClasses = _FindGymclassRepository.ListBy(x => x.Date >= monday && x.Date <= sunday);

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

            return weekayItemList;
        }
        public DateTime GetMonday(DateTime date)
        {
            int difference = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-difference).Date;
        }
        public DateTime GetSunday(DateTime date)
        {
            int difference = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(difference).Date;
        }
        public List<TimeSpan> AssignTimeSlots(DateTime start, DateTime end)
        {
            var timeSlots = new List<TimeSpan>();
            while (start < end)
            {
                timeSlots.Add(start.TimeOfDay);
                start = start.AddMinutes(30);
            }
            return timeSlots;
        }
    }
}
