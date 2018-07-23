using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    /// <summary>
    /// Central interface to implement when a new comparer should be required.
    /// </summary>
    public interface IVFComparer
    {
        /// <summary>
        /// Gets called once to initialize all dependencies available to the comparer
        /// </summary>
        /// <param name="io"></param>
        void Init( IIO io );

        /// <summary>
        /// Method should decide whether it wants to handle this sort of file.
        /// Decision may be based by inspecting extension, contents, paths etc.
        /// Recommendation: Utilize IIO and other dependencies to access files
        /// to benefit from caching and locking mechanisms.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool WantsToHandle( string file );


        /// <summary>
        /// Called for both files to perform handling.
        /// Assume that WantsToHandle has already ensured consistency, there is no need
        /// to recheck whether it is valid to handle both files
        /// </summary>
        /// <param name="fileA">Path to first file</param>
        /// <param name="fileB">Path to second file</param>
        void Handle( string fileA, string fileB );
    }
}
