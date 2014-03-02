using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    class ConsoleLogger : ILogger
    {
        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }

        public void Write(string str)
        {
            Console.Write(str);
        }

        public void Close() { }
        public void Flush() { }

    }
}
