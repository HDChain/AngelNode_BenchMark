using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HealthDataTest.DataTask;
using Nethereum.Signer;

namespace HealthDataTest.Command {
    /// <summary>
    /// newaccount
    /// -n=[number of account to create]
    /// -t=[thread]
    /// </summary>
    internal class CmdNewAccount : CmdBase {
        private DataTask<Tuple<int, int>> _dataTask;


        public override void Run(string[] args) {
            var countToCreate = 1;
            var threadCount = 1;

            var cmdargs = ParseArgs(args);

            foreach (var cmdarg in cmdargs)
                switch (cmdarg.Item1) {
                    case "-n":
                        countToCreate = int.Parse(cmdarg.Item2);
                        break;
                    case "-t":
                        threadCount = int.Parse(cmdarg.Item2);
                        break;
                }
            
            _dataTask = new DataTask<Tuple<int, int>>();

            for (var i = 0; i < threadCount; i++) {
                _dataTask.AddTask(new Tuple<int, int>(i,countToCreate / threadCount));
            }
            
            _dataTask.Start(DoTask,threadCount);
            _dataTask.Wait();

        }

        private void DoTask(Tuple<int, int> arg) {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,$"acc{arg.Item1}.txt");
            

            using (var sw = new StreamWriter(path,false)) {
                for (int i = 0; i < arg.Item2; i++) {
                    var ecKey = EthECKey.GenerateKey();
                    
                    sw.WriteLine($"{ecKey.GetPublicAddress()},{ecKey.GetPrivateKey()}");

                    if (i%100 == 0) {
                        sw.Flush();
                        
                        Console.WriteLine($"thread {arg.Item1} created [{i}]");
                    }
                }
            }
            
        }

        public override void Stop() {
            _dataTask.Stop();
        }
    }
}