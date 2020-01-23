using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace Asendia.Order.Monitor
{
    [Verb("orders", HelpText = "Generate orders xml files")]
    public class OrderOptions
    {
        [Option('s', "source", HelpText = "Source Directory full path, where the CSV files are stored")]
        public string Source { get; set; }

        [Option('o', "output", HelpText = "Output Directory full path, where the XML files will be stored")]
        public string Output { get; set; }

        [Usage(ApplicationAlias = "Monitor")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Generate orders xml files", new OrderOptions
                {
                    Source = @"c:\source",
                    Output = @"c:\output"
                });
            }
        }
    }
}
