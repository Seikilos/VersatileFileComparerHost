using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

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
            try
            {

            }
            catch (Exception ex) when (!System.Diagnostics.Debugger.IsAttached)
            {
                Console.Error.WriteLine(ex);
                Environment.ExitCode = 2;
            }
        }
    }
}
