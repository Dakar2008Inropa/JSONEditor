using log4net;
using System.Reflection;

namespace JSONEditor.Classes.Tools
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Log => log;
    }
}