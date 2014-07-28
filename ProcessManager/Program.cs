using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProcessManager
{
    class Program
    {
        static void Main(string[] args)
        {

            string processName = "";
            int seed = 0;

            try
            {
                processName = args[0];
                seed = int.Parse(args[1]);
            }
            catch(IndexOutOfRangeException e){
                
                Console.WriteLine("引数が足りません．プログラム名を入力してください．");

                Environment.Exit(0);
            }

            List<Process> processes = new List<Process>();
            //var algoStrList = new string[] { "AAT", "DontReply", "NewDontReply", "LimitedBelief", "PartialLimitedBelief"};
            var algoStrList = new string[] { "AAT","LimitedBelief", "PartialLimitedBelief"};
            var netStrList = new string[] { "WS", "BA", "Random"};

            Console.WriteLine("exp about");


            foreach (string netStr in netStrList)
            {
                foreach (string algoStr in algoStrList)
                {
                    Console.WriteLine("net: " + netStr + "algo: " + algoStr + ", seed: " + seed);
                    processes.Add(
                        Process.Start(processName, algoStr + " " + seed + " " + netStr));
                }
            }

            foreach (var process in processes)
            {
                process.WaitForExit();
            }


            Console.WriteLine("Finished");
        }
    }
}
