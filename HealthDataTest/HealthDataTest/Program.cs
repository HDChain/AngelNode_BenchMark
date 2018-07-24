using System;
using System.IO;
using HealthDataTest.Command;
using log4net;
using log4net.Config;

namespace HealthDataTest {
    internal class Program {
        private static readonly ILog Logger = LogManager.GetLogger(Log4NetCore.CoreRepository, typeof(Program));

        private static void Main(string[] args) {
            var fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, fi);

            if (args.Length == 0) {
                PrintHelp();
                return;
            }

            Logger.Info(args);
            //command list
            
            switch (args[0].ToLower()) {
                case "newaccount": {
                    new CmdNewAccount().Run(args);
                }
                    break;
                case "batchtranfer": {
                    new CmdBatchTransfer().Run(args);
                }
                    break;
                case "contract": {
                    new CmdContractTest().Run(args);
                }
                    break;
            }
        }
        

        private static void PrintHelp() {
            Console.WriteLine(
                @"Usage HealthDataTest.exe [cmd] [cmd options]

Commands:

newaccount
    -n=[number of account to create]
    -t=[thread]
batchtranfer
    -n=[account filename]
contract
    -n=[number of account]
    -t=[each file with thread count ]
11");
        }
    }
}