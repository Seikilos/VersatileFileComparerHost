using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost.Implementation
{
    /// <summary>
    /// Implements a thread-safe and as lock free as possible way to handle logs in threads
    /// Note: It is not entirely lock free because periodic progress is reported forcing all threads to yield.
    /// Until then each thread has a non-blocking
    /// </summary>
    public class MultiThreadedCommonLogger : ILogger
    {
        /// <summary>
        /// Reads are lock-free
        /// </summary>
        private readonly ConcurrentDictionary<int, StringBuilderLogger> _loggerMapping = new ConcurrentDictionary< int, StringBuilderLogger >();

        /// <summary>
        /// Holds thread R/W locks.
        /// </summary>
        private readonly ConcurrentDictionary<int, ReaderWriterLock> _threadLockObjects = new ConcurrentDictionary< int, ReaderWriterLock >();

        /// <summary>
        /// Should query underlying loggers to report whether errors were detected
        /// </summary>
        public bool HadErrors
        {
            get {
                foreach ( var logger in _loggerMapping )
                {
                    if (logger.Value.HadErrors)
                    {
                        return true;
                    }
                }

                return false;
            }
        } 

        /// <summary>
        /// Yields all threads and dumps their data to passed arguments
        /// </summary>
        public void DumpAllLogs(Action<string> outputDump, Action<string> errorDump)
        {
           
            // Loop over every underlying string builder and get their data, replace it afterwards
            foreach ( var logger in _loggerMapping )
            {
                // Obtain the write lock because the next operations must never be interrupted by writing to them
                try
                {
                    // Draining the logger from their messages and flushing the streams must be an atomic operation

                    _threadLockObjects[logger.Key].AcquireWriterLock( Int32.MaxValue );

                    outputDump(logger.Value.StringBuilderOutput.ToString());
                    errorDump(logger.Value.StringBuilderError.ToString());
                    logger.Value.StringBuilderOutput.Clear();
                    logger.Value.StringBuilderError.Clear();

                }
                finally
                {
                    _threadLockObjects[logger.Key].ReleaseWriterLock();
                }
            }

        }


        public void Info( string msg, params object[] parameters )
        {
            try
            {
                _getLockObjectForThread().AcquireReaderLock( Int32.MaxValue );
                _getLoggerForThread().Info( msg, parameters );
            }
            finally
            {
                _getLockObjectForThread().ReleaseReaderLock();
            }

        }

        public void Warning( string msg, params object[] parameters )
        {
            try
            {
                _getLockObjectForThread().AcquireReaderLock( Int32.MaxValue );
                _getLoggerForThread().Warning( msg, parameters );
            }
            finally
            {
                _getLockObjectForThread().ReleaseReaderLock();
            }

        }

        public void Error( string msg, params object[] parameters )
        {
            try
            {
                _getLockObjectForThread().AcquireReaderLock( Int32.MaxValue );
                _getLoggerForThread().Error( msg, parameters );
            }
            finally
            {
                _getLockObjectForThread().ReleaseReaderLock();
            }

        }

        public void Exception( string msg, Exception innerException )
        {
            try
            {
                _getLockObjectForThread().AcquireReaderLock( Int32.MaxValue );
                _getLoggerForThread().Exception( msg, innerException );
            }
            finally
            {
                _getLockObjectForThread().ReleaseReaderLock();
            }

        }

        /// <summary>
        /// Locks the write access to the dictionary
        /// </summary>
        /// <returns></returns>
        private StringBuilderLogger _getLoggerForThread()
        {
            var id = _getThreadId();

            if ( _loggerMapping.ContainsKey(id) == false)
            {
                _loggerMapping[id] = new StringBuilderLogger();
            }

            return _loggerMapping[id];

        }

        private ReaderWriterLock _getLockObjectForThread()
        {
            var id = _getThreadId();

            if ( _loggerMapping.ContainsKey(id) == false)
            {
              
                _threadLockObjects[id] = new ReaderWriterLock();
            }

            return _threadLockObjects[id];

        }

        private int _getThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }
    }
}
