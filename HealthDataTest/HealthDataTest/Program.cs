using System;
using HealthDataTest.Command;

namespace HealthDataTest {
    internal class Program {
        private static void Main(string[] args) {
            if (args.Length == 0) {
                PrintHelp();
                return;
            }


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
batchtranfer
    


11");
        }
    }
}