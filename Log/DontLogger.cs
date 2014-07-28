using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    class DontLogger : ILogger
    {
        public void WriteLine(string str){
            //Do nothing.
        }

        public void Write(string str){
            //Do nothing.
        }

        public void Close(){
            //Do nothing.
        }

        public void Flush(){
            //Do nothing.
        }

    }
}
