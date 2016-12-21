using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Log;

namespace OpinionSharing.Agt
{
    using Subject;

    //最終的に決めた意見と、もらった意見を比べて、間違ってるやつの信頼度を減らす
    public class Trast : AAT
    {
        Queue<BWMessage> receivedMessages = new Queue<BWMessage>();

        Dictionary<AgentAlgorithm, int> trastList = new Dictionary<AgentAlgorithm,int>();
        


        public override void Initialize()
        {
            base.Initialize();

            foreach (AgentAlgorithm n in Neighbours)
            {
                trastList[n] = 0;
            }
        }

        public override void ReceiveOpinion(BWMessage message)
        {

            base.ReceiveOpinion(message);

            receivedMessages.Enqueue(message);
        }


        public override void RoundFinished(BlackWhiteSubject? thefact)
        {

            if (this.Opinion != null)
            {
                foreach (var message in receivedMessages)
                {
                    if (message == null)
                    {
                        continue;
                    }

                    if(message.Subject == this.Opinion && message.From is AgentAlgorithm){
                        trastList[message.From as AgentAlgorithm] ++;
                    }
                }


                L.g("trast").WriteLine("I'm" + this);

                foreach (AgentAlgorithm a in trastList.Keys)
                {
                    L.g("trast").WriteLine(a + "'s trust = " + trastList[a]);
                }
                
            }
            

            base.RoundFinished(thefact);
        }
    }
}
