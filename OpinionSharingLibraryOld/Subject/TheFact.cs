using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Util;

namespace OpinionSharing.Subject
{
    public class TheFact
    {
        public BlackWhiteSubject Value{get;set;}

        public TheFact(BlackWhiteSubject value){
            Value = value;
        }

        public void randomNext()
        {
            double r = RandomPool.Get("fact").NextDouble();
            if(r < 0.5){
                Value = BlackWhiteSubject.White;
            }
            else{
                Value = BlackWhiteSubject.Black;
            }
        }

        public void setNext(BlackWhiteSubject value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return "[TheFact " + (Value == BlackWhiteSubject.White ? "White":"Black") + "]";
        }

    }
}
