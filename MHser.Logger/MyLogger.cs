using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace MHser.Logger
{
    public class MyLogger
    {
        private static MyLogger _instance;
        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        private MyLogger()
        {
                
        }

        public static MyLogger GetLogger()
        {
            return _instance ?? (_instance = new MyLogger());
        }

        public void WriteLog( string logLevel, string message )
        {
            _logger.Log(LogLevel.FromString(logLevel), message);
        }

        public void WriteLog( string logLevel, Exception exception )
        {
            _logger.Log( LogLevel.FromString( logLevel ), exception );
        }
    }
}
