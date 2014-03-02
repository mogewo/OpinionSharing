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

        public void SendOpinion(BlackWhiteSubject opinion, Agent to)
        {
            to.ReceiveOpinion(new BWMessage(opinion, this));

        }

        public void SendWhite(Agent to)
        {
            SendOpinion(BlackWhiteSubject.White, to);
        }

        public void SendBlack(Agent to)
        {
            SendOpinion(BlackWhiteSubject.Black, to);
        }
    }
}
