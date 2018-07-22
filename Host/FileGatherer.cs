using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost
{
    /// <summary>
    /// Performs a lookup for directories and check whether there are missing ones
    /// </summary>
    public class FileGatherer
    {
        private readonly IIO _io;
        private readonly string _dirA;
        private readonly string _dirB;

        public FileGatherer(IIO io, string dirA, string dirB)
        {
            if (io.DirectoryExist(dirA) == false)
            {
                throw new DirectoryNotFoundException($"Missing directory '{dirA}'");
            }

            if (io.DirectoryExist(dirB) == false)
            {
                throw new DirectoryNotFoundException($"Missing directory '{dirB}'");
            }

            _io = io;
            _dirA = dirA;
            _dirB = dirB;
        }

        /// <summary>
        /// Creates a joined list for all files. Each tuple having one item being null indicates that there where no files at either location A or B
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompareEntry> CreateFilelist()
        {
            var filesA = _io.GetFiles(_dirA, "*", true);
            var filesB = _io.GetFiles(_dirB, "*", true);


            // Create mapped dictionary for files mapping to their normalized value
            var normalizedBMap = filesB.ToDictionary(s => s.ReplaceFront(_dirB, ""), s => s);

            // First loop all A and look for matches
            foreach (var fileA in filesA)
            {
                var normalized = fileA.ReplaceFront(_dirA, "");
                if (normalizedBMap.ContainsKey(normalized))
                {
                    // found explicit match
                    yield return new CompareEntry(normalized, fileA, normalizedBMap[normalized]);
                    normalizedBMap.Remove(normalized);
                }
                else
                {
                    // This is missing in b, return null for second item
                    yield return new CompareEntry(normalized, fileA, null);
                }
            }

            // From here normalizedB now contains only entries left which do not exist in filesA, so simply retrieve them
            foreach (var fileBNormalized in normalizedBMap)
            {
                yield return new CompareEntry(fileBNormalized.Key, null, fileBNormalized.Value);
            }

        }
    }
}
