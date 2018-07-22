using System;
using System.Collections.Generic;
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
    }
}
