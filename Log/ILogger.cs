using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    public interface ILogger
    {
        void WriteLine(string str);
        void Write(string str);
        void Close();
        void Flush();
    }
}
