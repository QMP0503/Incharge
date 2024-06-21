using log4net.Config;
using log4net;

namespace BookAHotel.Logs
{
    public static class Log4netExtensions
    {
        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("C:\\Users\\intern.pmquang1\\C#\\Incharge\\Incharge\\Logs\\log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }
    }
}
