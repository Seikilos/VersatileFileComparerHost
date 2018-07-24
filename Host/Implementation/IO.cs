using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost.Implementation
{
    /// <summary>
    /// Implements the IO operations on a simple layer.
    /// May cache read file streams if required
    /// </summary>
    class IO : IIO
    {
        public bool DirectoryExist( string path )
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Slim implementation around file IO
        /// </summary>
        /// <param name="path"></param>
        /// <param name="wildcard"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public List< string > GetFiles( string path, string wildcard, bool recurse )
        {
            return Directory.GetFiles(path, wildcard, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
        }

        /// <summary>
        /// Opens a stream in a non blocking way for reading. This is a shared lock
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Stream ReadFile( string file )
        {
            return new FileStream(file,FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
