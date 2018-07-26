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
            var directLog = new ConsoleLogger();
            var threadedLog = new MultiThreadedCommonLogger();

            try
            {
                var io = new IO();
             
                directLog.Info("Comparing directories");
                directLog.Info("\t[A] {0}", options.DirectoryA);
                directLog.Info("\t[B] {0}", options.DirectoryB);


                var foundComparers = _getComparers(io);

                directLog.Info("Found {0} comparers: {1}", foundComparers.Count, string.Join(", ",foundComparers.Select(c => c.GetType().Name)) );

                var foundFiles = new FileGatherer(io, options.DirectoryA, options.DirectoryB).CreateFilelist();

                var correlator = new FileToComparerCorrelator(threadedLog);
                var matchedFiles = correlator.MatchFilesToComparers(foundComparers, foundFiles);
              
                directLog.Info("Processing files");

                _processFiles(threadedLog, matchedFiles);
                
                // Finally always dump remaining logs
                threadedLog.DumpAllLogs(_writeToOutput, _writeToError);

                directLog.Info("Processing files finished");

              

            }
            catch (Exception ex) when (!System.Diagnostics.Debugger.IsAttached)
            {
                Console.Error.WriteLine(ex);
                Environment.ExitCode = 2;
            }

            // compare errors should not indicate serious (exit code 2)
            if( threadedLog.HadErrors )
            {
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// Processes files in an async manner, avoids locking of IO or any other operations when executed in parallel
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="matchedFiles">An lazy evaluated list that performs IO to disk on demand</param>
        private static void _processFiles(MultiThreadedCommonLogger logger, IEnumerable<Tuple<CompareEntry, List<IVFComparer>>> matchedFiles)
        {
            var tasks = new List<Task>();

            const int heartBeat = 100; // Each 100 jobs a heart beat to log should be sent

            var jobsRun = 0;

            foreach ( var matchedFile in matchedFiles )
            {
                var mf = matchedFile;
                tasks.Add( Task.Run( () => _executeTask(logger, mf.Item1, mf.Item2 ) ).ContinueWith( (task) => _checkForErrors(logger, task)) );

                if (jobsRun + 1 == heartBeat)
                {
                    jobsRun = 0;
                    // Queue a forced heart beat into the task queue to synchronize and dump logged data
                    tasks.Add(Task.Run( () => _syncLogs(logger)));
                }

                ++jobsRun;
            }


            Task.WaitAll( tasks.ToArray() );
        }

        /// <summary>
        /// Force a locked flush of all threads to report some progress to the console IO.
        /// Otherwise it will be blank until the entire task queue has been completed
        /// </summary>
        /// <param name="logger"></param>
        private static void _syncLogs( MultiThreadedCommonLogger logger )
        {
            try
            {
                logger.DumpAllLogs(_writeToOutput, _writeToError);
            }
            catch ( Exception e )
            {
                logger.Exception("Syncing heartbeat failed with", e);
            }
        }

        private static void _writeToError( string obj )
        {
            Console.Error.Write(obj);
        }

        private static void _writeToOutput( string obj )
        {
            Console.Write(obj);
        }

        private static void _checkForErrors( ILogger logger, Task task )
        {

            if( task.IsFaulted)
            {
                logger.Exception("Task failed", task.Exception);
            }
        }



        private static void _executeTask( ILogger logger, CompareEntry compareEntry, List< IVFComparer > comparers )
        {
            logger.Info("Checking '{0}'", compareEntry.CommonName);

            foreach ( var vfComparer in comparers )
            {
                vfComparer.Handle(compareEntry.PathA, compareEntry.PathB);
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
