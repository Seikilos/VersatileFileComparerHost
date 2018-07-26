using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost.Implementation
{
    /// <summary>
    /// Simple console logger relying on common output logger
    /// </summary>
    class ConsoleLogger : CommonOutputLogger
    {
        public ConsoleLogger( ) : base(Console.WriteLine, Console.Error.WriteLine)
        {
        }
    }
}
