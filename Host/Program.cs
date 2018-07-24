using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Contracts;
using VersatileFileComparerHost.Implementation;

namespace VersatileFileComparerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var parserResult = parser.ParseArguments<Options>(args);
            parserResult.WithParsed<Options>(options => Execute(options));
            parserResult.WithNotParsed(errs =>
                {
                    var helpText = HelpText.AutoBuild(parserResult, h =>
                    {
                        // Configure HelpText here or create your own and return it 
                        h.AdditionalNewLineAfterOption = false;
                        return HelpText.DefaultParsingErrorsHandler(parserResult, h)
                            .AddPostOptionsLine("Exit code 0: finished without differences")
                            .AddPostOptionsLine("Exit code 1: found differences")
                            .AddPostOptionsLine("Exit code 2: Serious error or invalid arguments")
                            .AddPostOptionsLine("")
                            ;
                    }, e =>
                    {
                        Environment.ExitCode = 2;
                        return e;
                    });
                    Console.Error.Write(helpText);
                }
            );
        }

        private static void Execute(Options options)
        {
            var log = new ConsoleLogger();

            try
            {
                var io = new IO();
             
                log.Info("Comparing directories");
                log.Info("\t[A] {0}", options.DirectoryA);
                log.Info("\t[B] {0}", options.DirectoryB);


                var foundComparers = _getComparers(io);

                log.Info("Found {0} comparers: {1}", foundComparers.Count, string.Join(", ",foundComparers.Select(c => c.GetType().Name)) );

                var foundFiles = new FileGatherer(io, options.DirectoryA, options.DirectoryB).CreateFilelist();

                var correlator = new FileToComparerCorrelator(log);
                var matchedFiles = correlator.MatchFilesToComparers(foundComparers, foundFiles);
              
                log.Info("Processing files");

                _processFiles(matchedFiles);


                log.Info("Processing files finished");


            }
            catch (Exception ex) when (!System.Diagnostics.Debugger.IsAttached)
            {
                Console.Error.WriteLine(ex);
                Environment.ExitCode = 2;
            }

            // compare errors should not indicate serious (exit code 2)
            if( log.HadErrors )
            {
                Environment.ExitCode = 1;
            }
        }

        private static void _processFiles(IEnumerable<Tuple<CompareEntry, List<IVFComparer>>> matchedFiles)
        {
            var tasks = new List<Task>();

            foreach (var matchedFile in matchedFiles)
            {
               // tasks.Add(Task.Run(_executeTask));
            }
        }


        /// <summary>
        /// Constructs a plugin loader and discovers all available comparer
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        private static List<IVFComparer> _getComparers( IO io )
        {

            var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var pluginDir = Path.Combine(assemblyDir, "plugins");

            var pluginLoader = new PluginLoader(io);
            pluginLoader.LoadFromPluginDirectory(pluginDir);

            return pluginLoader.DiscoverAndInitializePlugins();
        }
    }
}
