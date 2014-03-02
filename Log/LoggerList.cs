using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    public class LoggerList :  List<ILogger>, ILogger
    {
        public void WriteLine(string str)
        {
            foreach (var logger in this)
            {
                logger.WriteLine(str);
            }
        }

        public void Write(string str)
        {
            foreach (var logger in this)
            {
                logger.Write(str);
            }
        }

        public void Close()
        {
            foreach (var logger in this)
            {
                logger.Close();
            }
        }

        public void Flush() 
        { 
            foreach (var logger in this)
            {
                logger.Flush();
            }
        }
    }
}
