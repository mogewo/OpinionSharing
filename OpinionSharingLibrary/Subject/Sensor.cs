using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MyRandom;
using OpinionSharing.Agt;

namespace OpinionSharing.Subject
{

    public class Sensor : IOpinionSender
    {
        protected double? accuracy = 0.55;

        private IAgent agent; //センサーの値を受け取るエージェント

        private TheFact theFact;

        public Sensor(TheFact fact)
        {
            theFact = fact;
        }

        public Sensor(TheFact fact, double acc)
        {
            theFact = fact;
            accuracy = acc;
        }

        public double? Accuracy
        {
            get
            {
                return accuracy;
            }
            set {
                
                accuracy = value; 
            }
        }
        public IAgent Agent
        {
            get
            {
                return agent;
            }
            set
            {
                SetAgent(value);
            }
        }

        public void SetAgent(IAgent a)
        {
            agent = a;

        }

        public BlackWhiteSubject newData()
        {
            BlackWhiteSubject currentData;
            double r = RandomPool.Get("sensor").NextDouble();


            if (r < accuracy)//正しいデータ
            {
                currentData = theFact.Value;

            }

            else //間違ったデータ
            {
                if (theFact.Value == BlackWhiteSubject.Black)//天邪鬼if
                {
                    currentData = BlackWhiteSubject.White;
                }
                else
                {
                    currentData = BlackWhiteSubject.Black;
                }
            }
            
            //知らせる
            SendOpinion(currentData, agent);

            return currentData;
        }

        public void SendOpinion(BlackWhiteSubject opinion, IAgent to)
        {
            to.ReceiveOpinion(new BWMessage(opinion,this));
        }
        
        public override string ToString()
        {
            return string.Format("[Sensor -> {0}]",agent.ToString()) ;
        }
    }
}
