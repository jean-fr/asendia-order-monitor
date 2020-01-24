using Autofac;
using CommandLine;
using NLog;
using System;

namespace Asendia.Order.Monitor
{
    public class Program
    {
      public  static void Main(string[] args)
        {
            var container = SetupIocContainer();
            var parser = new Parser(config => config.HelpWriter = Console.Out);

            parser.ParseArguments<OrderOptions>(args)
                    .MapResult((OrderOptions opts) => container.Resolve<IFileGenerator>().Execute(opts), errs => -1);
        }

        //Register dependencies
        private static IContainer SetupIocContainer()
        {
            var container = new ContainerBuilder();
            container.RegisterInstance(LogManager.GetCurrentClassLogger()).As<ILogger>().SingleInstance();
            container.RegisterType<FileGenerator>().As<IFileGenerator>().SingleInstance();           
            return container.Build();
        }
    }
}
