using Incharge.ViewModels;
using Incharge.ViewModels.Calendar;

namespace Incharge.Service.IService
{
    public interface IGymclassCalendarService
    {
        public List<WeekdayItem> CreateItemList(string type ,string filter, DateTime selectedDate);
        public DateTime GetMonday(DateTime date);
        public DateTime GetSunday(DateTime date);
        public List<TimeSpan> AssignTimeSlots(DateTime start, DateTime end);
    }
}