using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost.Implementation
{
    /// <summary>
    /// Simple logger that logs into a string builder.
    /// Caller can obtain and manipulate the string builder
    /// </summary>
    public class StringBuilderLogger : ILogger
    {

        public StringBuilder StringBuilderOutput { get; set; } = new StringBuilder();
        public StringBuilder StringBuilderError { get; set; } = new StringBuilder();

        private readonly CommonOutputLogger _innerLogger;

        public StringBuilderLogger()    
        {
            _innerLogger = new CommonOutputLogger(outputCallback, errorCallback);
        }

        #region Callbacks

        protected void errorCallback( string obj )
        {
            StringBuilderError.AppendLine(obj);
        }

        protected void outputCallback( string obj )
        {
            StringBuilderOutput.AppendLine(obj);
        }

        #endregion

        #region Delegating Implementation

        public bool HadErrors => _innerLogger.HadErrors;

        public void Info( string msg, params object[] parameters )
        {
            _innerLogger.Info(msg, parameters);
        }

        public void Warning( string msg, params object[] parameters )
        {
            _innerLogger.Warning(msg, parameters);
        }

        public void Error( string msg, params object[] parameters )
        {
            _innerLogger.Error(msg, parameters);
        }

        public void Exception( string msg, Exception innerException )
        {
            _innerLogger.Exception(msg, innerException);
        }

        #endregion
    }
}
