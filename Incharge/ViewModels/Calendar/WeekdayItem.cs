﻿using Incharge.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace Incharge.ViewModels.Calendar
{
    public class WeekdayItem //just stores data
    {
        public string Weekday { get; set; } // dateValue.ToString("dddd", new CultureInfo("es-ES")); //output is wednesday
        public GymClassVM Item { get; set; }
        public List<TimeSpan> TimeSlots { get; set; }
        //only round down (if 7:20 will go under 7:00)
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        

        //DROP DOWN VIEW OPTION ONLY
        public List<string> DropdownOption { get; set; } = new List<string>();
        public DateTime DateFilter { get; set; }
        public DateTime MondayOfFilter { get; set; }
        public DateTime SundayOfFilter { get; set; }

        //Error message
        [AllowNull]
        public string Error { get; set; }
    }
}
