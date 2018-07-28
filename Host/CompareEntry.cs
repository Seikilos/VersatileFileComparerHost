using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersatileFileComparerHost
{
    /// <summary>
    /// Container containing the common name of an entry and their full paths at location A and B
    /// </summary>
    public class CompareEntry
    {
        public string CommonName { get; }
        public string PathA { get; }
        public string PathB { get; }

        public CompareEntry(string commonName, string pathA, string pathB)
        {
            CommonName = commonName;
            PathA = pathA;
            PathB = pathB;
        }
    }
}
