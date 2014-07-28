using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Util;
using OpinionSharing.Net;

using Log;

namespace OpinionSharing.Agt
{
    using Subject;

    //意見を決めるまでに、意見をもらった人を覚えておいて、そいつには意見を投げない
    //AwarenessRateの推定がおかしくなる。
    //渡さないのではなくて、解釈しない　＝＞NoMoreBeliefへ。

    public class DRMessageQueue{
        Queue<BWMessage> messages;

        public DRMessageQueue()
        {
            messages = new Queue<BWMessage>();
        }

        public IEnumerable<IOpinionSender> Senders
        {
            get
            {
                HashSet<IOpinionSender> senders = new HashSet<IOpinionSender>();

                foreach (var message in messages)
                {
                    senders.Add(message.From);
                }

                return senders;
            }
        }

        public IEnumerable<BWMessage> Messages
        {
            get
            {
                foreach (var item in messages)
                {
                    yield return item;
                }
            }
        }


        public bool ReceivedFrom(IOpinionSender sender)
        {
            foreach (var message in messages)
            {
                if (message.From == sender)
                {
                    return true;
                }
            }
            return false;
        }


        public BlackWhiteSubject OpinionOf(IOpinionSender sender)
        {
            //最も最近のものを持ってこなくてはならない。
            var reverseMessages = messages.Reverse();

            foreach (var message in reverseMessages)
            {
                if (message.From == sender)
                {
                    return message.Subject;
                }
            }
            throw new Exception("no such Sender");
        }

        public void Enqueue(BWMessage message)
        {
            messages.Enqueue(message);
        }
    }


    public class DontReply : AAT
    {
        //意見をくれたご近所
        DRMessageQueue messageBox;
        

        public DontReply(double h_trg)
            : base(h_trg)
        { }

        public DontReply()
        { }

        public override void Initialize()
        {
            base.Initialize();

            messageBox = new DRMessageQueue();
        }

        public override void ReceiveOpinion(BWMessage message)
        {
            /*  */
            if (message.From != null && message.From is Agent)//送信者がエージェントの場合のみ貯める
            {
                messageBox.Enqueue(message);
            }
            //*/

            base.ReceiveOpinion(message);
        }

        protected override void NotifyOthers()
        {
            //ご近所さん全員に伝える
            foreach (Agent neighbour in this.Neighbours)
                
            if (!messageBox.ReceivedFrom(neighbour) || //ただし、意見をくれたやつにはあげる必要ない。
                messageBox.OpinionOf(neighbour) != this.Opinion )//くれてるやつでも、違う意見もってるやつには投げる。
            {//センサーが入ってるとバグるよ。なぜならば、receivedFromが成立しないから？

                
                if (Opinion == null)
                {
                    throw new Exception("こんなことありえない。バグだ");
                }

                BlackWhiteSubject myopinion = thought.Opinion.Value;//nullableを通常に変換

                this.SendOpinion(myopinion, neighbour);//送る
            }

            messageBox = new DRMessageQueue();
        }


        //最後の推定時
        public override void EstimateAwarenessRate()
        {
            //Don'tReplyなので、もらえない可能性がある。もらえなかったところを補完するために、もらえなかった奴は、自分と同じだと思ってしまう。

            //まず、送信者のリストをエージェントのリストに変換
            List<Agent> senderAgents = new List<Agent>();
            foreach (Agent senderAgent in messageBox.Senders)
            {
                senderAgents.Add(senderAgent);
            }

            //意見をくれてない近隣は、DontReplyしてるだけだと思い込む。

            var notSender = Neighbours.Except(senderAgents);

            int countNotSender = notSender.Count();

            if(this.Opinion == BlackWhiteSubject.White){
                updatedNum += countNotSender;
            }
            else if(this.Opinion == BlackWhiteSubject.Black){
                updatedNum -= countNotSender;
            }

            
            base.EstimateAwarenessRate();
        }



    }
}
