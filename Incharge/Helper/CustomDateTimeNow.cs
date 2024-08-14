namespace Incharge.Helper
{
    public class CustomDateTimeNow
    {
        public static DateTime DateTimeNow
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            }
        }
    }
} 
