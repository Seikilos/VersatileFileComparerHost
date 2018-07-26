using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost.Implementation
{
    /// <summary>
    /// Versatile logger handling the output formatting but agnostic to the output medium
    /// </summary>
    public class CommonOutputLogger : ILogger
    {
        private readonly Action< string > _outputCallback;
        private readonly Action< string > _errorCallback;

        public CommonOutputLogger( Action< string > outputCallback, Action< string > errorCallback )
        {
            _outputCallback = outputCallback;
            _errorCallback = errorCallback;

            if (_outputCallback == null)
            {
                throw new ArgumentNullException(nameof(outputCallback), "Logger requires valid output callback");
            }
            if (_errorCallback == null)
            {
                throw new ArgumentNullException(nameof(errorCallback), "Logger requires valid error output callback");
            }
        }

        protected enum LogType
        {
            INFO,
            WARNING,
            ERROR,
            EXCEPTION
        }

        public bool HadErrors { get; set; }

        public void Info( string msg, params object[] parameters )
        {
            log(LogType.INFO, msg, parameters);
        }

        public void Warning( string msg, params object[] parameters )
        {
            log(LogType.WARNING, msg, parameters);
        }

        public void Error( string msg, params object[] parameters )
        {
            log(LogType.ERROR, msg, parameters);
        }

        public void Exception( string msg, Exception innerException )
        {
            var sb = new StringBuilder();
            sb.AppendLine( msg );

            var e = innerException;
            while ( e != null )
            {
                sb.AppendLine( "with inner exception:" );
                sb.AppendLine( e.ToString() );
                e = innerException.InnerException;
            }


            log( LogType.EXCEPTION, sb.ToString() );
        }

        protected virtual void log(LogType type, string msg, params object[] parameters)
        {
            switch ( type )
            {
                case LogType.INFO:
                case LogType.WARNING:
                    _outputCallback(string.Format(msg, parameters));
                    break;

                case LogType.ERROR:
                case LogType.EXCEPTION:
                    HadErrors = true;
                    _errorCallback(string.Format(msg, parameters));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
