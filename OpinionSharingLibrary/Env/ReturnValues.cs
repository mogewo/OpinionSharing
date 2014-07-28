using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Env
{
    public class ExpResult
    {
        public readonly ExpAccuracy Accuracy;
        public readonly ExpAgentsParam AgentsParam;

        public ExpResult(ExpAccuracy acc, ExpAgentsParam a){
            Accuracy = acc;
            AgentsParam = a;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, ",Accuracy, AgentsParam);
        }
    }

    public class ExpAgentsParam
    {
        public readonly double ImportanceLevel;
        public readonly double AwarenessRate;

        public ExpAgentsParam(double t, double h)
        {
            ImportanceLevel = t;
            AwarenessRate = h;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, ",ImportanceLevel,AwarenessRate);
        }
    }
    public class ExpAccuracy
    {
        public readonly double Correct;
        public readonly double Incorrect;
        public readonly double Undeter;

        public ExpAccuracy(double c, double i, double u)
        {
            Correct = c;
            Incorrect = i;
            Undeter = u;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, ", Correct, Incorrect, Undeter);
        }

    }
}
