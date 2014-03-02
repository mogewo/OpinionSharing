using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Util;

using Log;

namespace OpinionSharing.Agt
{
    using Subject;

    //最終的に決めた意見と、もらった意見を比べて、間違ってるやつの信頼度を減らす
    public class Trast : AAT
    {
        Queue<BWMessage> receivedMessages = new Queue<BWMessage>();

        Dictionary<Agent, int> trastList = new Dictionary<Agent,int>();
        
        public Trast(double h):base(h)
        {

            
        }

        public override IEnumerable<Candidate> BuildCandidates()
        {

            //一番最初にやるこの関数のついでに、信頼リストを生成
            foreach (Agent n in Neighbours)
            {
                trastList[n] = 0;
            }

            return base.BuildCandidates();

        }
        public override void ReceiveOpinion(BWMessage message)
        {

            base.ReceiveOpinion(message);

            receivedMessages.Enqueue(message);
        }


        public override void RoundFinished()
        {

            if (this.Opinion != null)
            {
                foreach (var message in receivedMessages)
                {
                    if (message == null)
                    {
                        continue;
                    }

                    if(message.Subject == this.Opinion && message.From is Agent){
                        trastList[message.From as Agent] ++;
                    }
                }


                L.g("trast").WriteLine("I'm" + this);

                foreach (Agent a in trastList.Keys)
                {
                    L.g("trast").WriteLine(a + "'s trust = " + trastList[a]);
                }
                
            }
            

            base.RoundFinished();
        }
    }
}
