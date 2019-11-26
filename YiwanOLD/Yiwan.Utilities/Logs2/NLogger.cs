using NLog;

namespace Yiwan.Utilities
{
    public class NLogger
    {
        private static readonly object mutex = new object();
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static Logger Current
        {
            get
            {
                if (_logger == null)
                {
                    lock (mutex)
                    {
                        if (_logger == null) _logger = LogManager.GetCurrentClassLogger();
                    }
                }
                return _logger;
            }
        }
    }
}