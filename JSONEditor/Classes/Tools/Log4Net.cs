using log4net;
using System.Reflection;

namespace JSONEditor.Classes.Tools
{
    public static class Log4net
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Log => log;
    }
}