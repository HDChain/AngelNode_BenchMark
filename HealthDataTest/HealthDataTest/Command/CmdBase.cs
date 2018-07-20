using System;
using System.Collections.Generic;
using System.Text;

namespace HealthDataTest.Command
{
    abstract class CmdBase {
        public abstract void Run(string[] args);

        public List<Tuple<string,string>> ParseArgs(string[] args) {

            var ret = new List<Tuple<string, string>>();
            
            for (var i = 1; i < args.Length; i++) {
                var s = args[i];
                var pos = s.IndexOf('=');
                if (pos == -1) {
                    continue;
                }

                var key = s.Substring(0, pos);
                var val = s.Substring(pos + 1);

                ret.Add(new Tuple<string, string>(key,val));
            }

            return ret;
        }
    }
}
