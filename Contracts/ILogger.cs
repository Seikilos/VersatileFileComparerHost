using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ILogger
    {
        bool HadErrors { get; }

        void Info( string msg, params object[] parameters );

        void Warning ( string msg, params object[] parameters );

        void Error( string msg, params object[] parameters );

        void Exception( string msg, Exception innerException );
    }
}
