using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Logger testLogger  = new Logger("test.log");//filename, isAppend = false

            Console.WriteLine("test log");

            testLogger.Write("no break;1");
            testLogger.WriteLine("hoihoi break1");

            testLogger.Flush();

            testLogger.Write("no break;2");
            testLogger.WriteLine("hoihoi break2");

            testLogger.Close();
            */

            L.g("meter").WriteLine("meter -------------*------------");
            L.g("meter,accuracy").WriteLine("accuracy is 0.6");

            L.Flush();
            L.Close();
        }
    }
}
