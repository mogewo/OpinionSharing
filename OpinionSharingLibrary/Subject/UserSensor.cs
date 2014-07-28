using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;

namespace OpinionSharing.Agt
{
    public class UserSensor : IOpinionSender
    {
        double? accuracy;
        
        public UserSensor() { }

        public double? Accuracy { get { return 0.9; } set { accuracy = value; } }//ほぼ正確だよ！信じて！

        public void SendOpinion(BlackWhiteSubject opinion, IAgent to)
        {
            to.ReceiveOpinion(new BWMessage(opinion, this));
        }

        public void SendWhite(AgentIO to)
        {
            SendOpinion(BlackWhiteSubject.White, to);
        }

        public void SendBlack(AgentIO to)
        {
            SendOpinion(BlackWhiteSubject.Black, to);
        }
    }
}
