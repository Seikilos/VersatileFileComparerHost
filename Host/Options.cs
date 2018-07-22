using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace VersatileFileComparerHost
{
    class Options
    {
        [Option('a', "directoryA", Required = true, HelpText = "First directory for comparison")]
        public string DirectoryA { get; set; }

        [Option('b', "directoryB", Required = true, HelpText = "Second directory for comparison")]
        public string DirectoryB { get; set; }

    }
}
