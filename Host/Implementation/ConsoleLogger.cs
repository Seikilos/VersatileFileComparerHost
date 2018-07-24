using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost.Implementation
{
    class ConsoleLogger : ILogger
    {
        public bool HadErrors { get; private set; } = false;

        private enum LogType
        {
            INFO,
            WARNING,
            ERROR,
            EXCEPTION
        }

        public void Info( string msg, params object[] parameters )
        {
           _log(LogType.INFO, msg, parameters);
        }

        public void Warning( string msg, params object[] parameters )
        {
            _log(LogType.WARNING, msg, parameters);
        }

        public void Error( string msg, params object[] parameters )
        {
            _log(LogType.ERROR, msg, parameters);
        }

        public void Exception( string msg, Exception innerException )
        {
            var sb = new StringBuilder();
           sb.AppendLine(msg);

            var e = innerException;
            while ( e != null )
            {
                sb.AppendLine("with inner exception:");
                sb.AppendLine(e.ToString());
                e = innerException.InnerException;
            }


            _log(LogType.EXCEPTION, msg);
        }

        private void _log(LogType type, string msg, params object[] parameters)
        {
            switch ( type )
            {
                case LogType.INFO:
                case LogType.WARNING:
                    Console.WriteLine(msg, parameters);
                    break;

                case LogType.ERROR:
                case LogType.EXCEPTION:
                    HadErrors = true;
                    Console.Error.WriteLine(msg, parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
