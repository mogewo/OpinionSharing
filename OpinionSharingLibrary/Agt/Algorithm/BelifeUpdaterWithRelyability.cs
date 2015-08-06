using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyRandom;
using GraphTheory.Net;

using Log;

namespace OpinionSharing.Agt.Algorithm
{
   //  using Subject;

   // //意見を決めるまでに、意見をもらった人を覚えておいて、そいつには意見を投げない
   // //AwarenessRateの推定がおかしくなる。
   // //渡さないのではなくて、解釈しない　＝＞NoMoreBeliefへ。

   //  public class DRMessageBox{
   //     Queue<BWMessage> messages;

   //     public DRMessageBox()
   //     {
   //         messages = new Queue<BWMessage>();
   //     }

   //     public IEnumerable<IOpinionSender> Senders
   //     {
   //         get
   //         {
   //             HashSet<IOpinionSender> senders = new HashSet<IOpinionSender>();

   //             foreach (var message in messages)
   //             {
   //                 senders.Add(message.From);
   //             }

   //             return senders;
   //         }
   //     }

   //     public IEnumerable<BWMessage> Messages
   //     {
   //         get
   //         {
   //             foreach (var item in messages)
   //             {
   //                 yield return item;
   //             }
   //         }
   //     }

   //     public bool ReceivedFrom(IOpinionSender sender)
   //     {
   //         foreach (var message in messages)
   //         {
   //             if (message.From == sender)
   //             {
   //                 return true;
   //             }
   //         }
   //         return false;
   //     }

   //     public BlackWhiteSubject OpinionOf(IOpinionSender sender)
   //     {
   //         //最も最近のものを持ってこなくてはならない。
   //         var reverseMessages = messages.Reverse();

   //         foreach (var message in reverseMessages)
   //         {
   //             if (message.From == sender)
   //             {
   //                 return message.Subject;
   //             }
   //         }
   //         throw new Exception("no such Sender");
   //     }

   //     public void Enqueue(BWMessage message)
   //     {
   //         messages.Enqueue(message);
   //     }
   // }

    //public class BelifeUpdaterWithRelyability
    //{
    //    public override void Initialize()
    //    {
    //        base.Initialize();

    //        messageBox = new DRMessageBox();
    //    }

    //    public override void RoundInit()
    //    {
    //        base.RoundInit();

    //        messageBox = new DRMessageBox();
    //    }

    //    public override void ProcessMessage(BWMessage message)
    //    {
    //        //ここじゃだめかも。Receiveのところでもいっこ穴開けなきゃ行けないかもしれない。！！！！！！！
    //        if (message.From != null && message.From is AgentIO)//送信者がエージェントの場合のみ貯める
    //        {
    //            messageBox.Enqueue(message);
    //        }

    //        base.ProcessMessage(message);

    //    }

    //    //みんなに知らせる。自分の意見はこうですよと。
    //    protected override void NotifyOthers(BlackWhiteSubject? myOpinion = null)
    //    {
    //        BlackWhiteSubject opinion = checkOpinion(myOpinion);

    //        //ご近所さん全員に伝える
    //        foreach (IAgent neighbour in this.Neighbours)
    //            if (!messageBox.ReceivedFrom(neighbour) || //ただし、意見をくれたやつにはあげる必要ない。
    //                messageBox.OpinionOf(neighbour) != this.Opinion)//くれてるやつでも、違う意見もってるやつには投げる。
    //            {
    //                SendOpinion(opinion, neighbour);
    //            }

    //        messageBox = new DRMessageBox();
    //    }


    //}
   }

