using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace BundledPlugin
{
    /// <summary>
    /// Very simple comparer that performs a check on file sizes
    /// </summary>
    public class FileSizeComparer : IVFComparer
    {
        private IIO _io;

        public string Explanation => "Compares files by checking their file sizes.";

        public void Init( IIO io )
        {
            _io = io;
        }

        /// <summary>
        /// Handles every file and checks only for sizes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool WantsToHandle( string file )
        {
            return true;
        }


        public void Handle( string fileA, string fileB )
        {
            using ( var sA = _io.ReadFile( fileA ) )
            using ( var sB = _io.ReadFile( fileB ) )
            {
                if (sA.Length != sB.Length)
                {
                    throw new ArgumentException($"File sizes for '{fileA}' and '{fileB}' are different: {sA.Length} != {sB.Length}");
                }
            }
        }
    }
}
