using Incharge.ViewModels;
using Incharge.ViewModels.Calendar;

namespace Incharge.Service.IService
{
    public interface IGymclassCalendarService
    {
        public List<WeekdayItem> CreateItemList();
        public DateTime GetMonday(DateTime date);
        public DateTime GetSunday(DateTime date);
        public List<TimeSpan> AssignTimeSlots(DateTime start, DateTime end);
    }
}