using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyRandom;
using GraphTheory.Net;

using Log;

namespace OpinionSharing.Agt
{
    using Subject;


    public class NewDontReply : DontReply
    { 
        //最後の推定時
        public override void EstimateAwarenessRate()
        {
            //Don'tReplyなので、もらえない可能性がある。もらえなかったところを補完するために、もらえなかった奴は、自分と同じだと思ってしまう。

            //まず、送信者のリストをエージェントのリストに変換
            List<AgentIO> senderAgents = new List<AgentIO>();
            foreach (AgentIO senderAgent in messageBox.Senders)
            {
                senderAgents.Add(senderAgent);
            }

            //意見をくれてない近隣は、DontReplyしてるだけだと思い込む。

            var notSender = Neighbours.Except(senderAgents);

            int countNotSender = notSender.Count();

            if(this.Opinion == BlackWhiteSubject.White){
                for (int i = 0; i < countNotSender; i++)
                {
                    counter.CountUp();
                }
            }

            else if(this.Opinion == BlackWhiteSubject.Black){
                for (int i = 0; i < countNotSender; i++)
                {
                    counter.CountDown();
                }
            }

            
            base.EstimateAwarenessRate();
        }

    }
}
