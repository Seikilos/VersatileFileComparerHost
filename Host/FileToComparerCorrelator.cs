using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost
{
    /// <summary>
    /// Matches a given set of files against a given set of comparers and returns
    /// a correlated list.
    /// Reports errors if no comparer found for a file
    /// </summary>
    public class FileToComparerCorrelator
    {
        private ILogger _logger;

        public FileToComparerCorrelator( ILogger logger )
        {
            _logger = logger;
        }


        /// <summary>
        /// Returns a correlated list of comparers suitable for a given compare entry
        /// </summary>
        /// <param name="comparers">List of comparers</param>
        /// <param name="files">List of CompareEntry files</param>
        /// <returns></returns>
        public IEnumerable<Tuple<CompareEntry, List<IVFComparer>>> MatchFilesToComparers( List<IVFComparer> comparers, IEnumerable<CompareEntry> files )
        {
            _logger.Info( "Matching comparers to files" );


            foreach ( var file in files )
            {
                var list = _correlateComparersForFile(file, comparers);
                
                // Design decision: Do not return an object if the list of matching comparers is empty
                // This could also return a file with no comparer but could not be processed anyway without them
                if( list.Count == 0)
                {
                    continue;
                }

                yield return Tuple.Create( file, list );
            }

            _logger.Info( "... done matching comparers to files" );
        }

        private List<IVFComparer> _correlateComparersForFile( CompareEntry file, List<IVFComparer> comparers )
        {
            var list = new List< IVFComparer >();
            foreach ( var comparer in comparers )
            {
                // Exit early if one of the files is null
                if (file.PathA == null || file.PathB == null)
                {
                    if (file.PathA == null)
                    {
                        _logger.Error("Inconsistent result for {0}: No file in first location found, only at second location '{1}'. Skipping", file.CommonName, file.PathB);
                    }
                    else
                    {
                        _logger.Error("Inconsistent result for {0}: No file in second location found, only at first location '{1}'. Skipping", file.CommonName, file.PathA);
                    }

                    continue;
                }

                var matchesA = comparer.WantsToHandle(file.PathA);
                var matchesB = comparer.WantsToHandle(file.PathB);

                if( matchesA && matchesB )
                {
                    // Both matched successfully
                    list.Add( comparer );
                }
                else
                {
                    // Additional check for
                    // Possible: One is false => wrong gathering of files in a previous step
                    if(matchesA || matchesB)
                    {
                        // Will be true if one of them is true, so wrong gathering
                        _logger.Error("Inconsistent result: Comparer {0} will either match file '{1}' or '{2}' but not both", comparer.GetType().Name, file.PathA, file.PathB);
                    }

                }

                
            }

            // If list is empty, no comparer is suitable for this file, report
            if (list.Any() == false)
            {
                _logger.Error("No comparer was found for files '{1}' and not '{2}'", file.PathA, file.PathB);
            }

            return list;
        }
    }
}
