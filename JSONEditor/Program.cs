using log4net;
using log4net.Config;
using System.Reflection;

namespace JSONEditor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }
    }
}