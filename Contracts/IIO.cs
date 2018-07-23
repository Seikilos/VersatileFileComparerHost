using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    /// <summary>
    /// Abstracts common IO operations
    /// </summary>
    public interface IIO
    {

        bool DirectoryExist(string path);

        List<string> GetFiles(string path, string wildcard, bool recurse);

        /// <summary>
        /// Returns a stream for a given file. Note caller is responsible for disposing the stream.
        /// It is possible that this file is cached on specific implementations.
        /// Assume that ReadFile is also thread safe when called in a threaded environment.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Stream ReadFile( string file );
    }
}
