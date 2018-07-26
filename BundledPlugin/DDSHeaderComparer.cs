using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace BundledPlugin
{
    /// <summary>
    /// Reads DDS headers and compares them. This plugin does not check for sizes of data, only for matching headers
    /// </summary>
    public class DDSHeaderComparer : IVFComparer
    {
        public string Explanation => "Compares DDS files by comparing their header";

        public void Init(IIO io)
        {
        }

        public bool WantsToHandle(string file)
        {
            return Path.GetExtension(file).ToLower() == ".dds";
        }

        public void Handle(string fileA, string fileB)
        {
            throw new NotImplementedException();
        }
    }
}
