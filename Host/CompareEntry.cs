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
        public string CommonName { get; private set; }
        public string PathA { get; private set; }
        public string PathB { get; private set; }

        public CompareEntry(string commonName, string pathA, string pathB)
        {
            CommonName = commonName;
            PathA = pathA;
            PathB = pathB;
        }
    }
}
