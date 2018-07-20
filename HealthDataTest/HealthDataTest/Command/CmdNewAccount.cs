using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Nethereum.Signer;

namespace HealthDataTest.Command {
    internal class CmdNewAccount : CmdBase {


        public override void Run(string[] args) {
            var count = 1;

            var cmdargs = ParseArgs(args);

            foreach (var cmdarg in cmdargs)
                switch (cmdarg.Item1) {
                    case "-n":
                        count = int.Parse(cmdarg.Item2);
                        break;
                }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"acc.txt");

            List<string> keys = new List<string>();

            Parallel.For(0,
                count,new ParallelOptions() {
                    MaxDegreeOfParallelism = 100
                },
                (o) => {
                    var ecKey = EthECKey.GenerateKey();

                    keys.Add($"{ecKey.GetPublicAddress()},{ecKey.GetPrivateKey()}");
                    
                });
            
            File.WriteAllLines(path,keys);


        }
    }
}