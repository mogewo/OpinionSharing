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

    //意見を決めるまでに、意見をもらった人を覚えておいて、そいつには意見を投げない
    //AwarenessRateの推定がおかしくなる。
    //渡さないのではなくて、解釈しない　＝＞NoMoreBeliefへ。


    public class WeightedNeighbour : AAT
    {
        //意見をくれたご近所
        //protected DRMessageBox messageBox;
        //public ISet<IAgent> Neighbours { get; set; }
        //public ISet<INode> Neighbours { get; set; }
        

        #region 各種プロパティ
        public IDictionary<IAgent, double> EdgeWeights{ get; set; }

        public override AgentIO Body
        {
            get
            {
                return base.Body;
            }
            set
            {
                base.Body = value;
                if(this.Neighbours != null)
                    foreach (var node in this.Neighbours)
                    {
                        setEdgeWeight(node);
	                }

                else
                {
                    throw new Exception(this.ID + " : の友達はいないです！");
                }
            }
        }

        #endregion

        public double getEdgeWeight(IAgent a)
        {
            if (this.Neighbours.Contains(a))
            {
                return this.EdgeWeights[a] == null ? 1.0 : this.EdgeWeights[a];
            }
            else
            {
                throw new Exception(this.ID + " : 友達じゃない"+a+"のことをきかないでください！");
            }
        }

        public void setEdgeWeight(IAgent a)
        {
            if (this.Neighbours.Contains(a))
            {
                this.EdgeWeights[a] = RandomPool.Get("WeightSet").NextDouble(0.9, 1.0); //とりあえず0.9~1.0
                //this.EdgeWeights[a] = 0.9;//
            }
            else
            {
                throw new Exception(this.ID + " : 友達じゃない"+a+"の重みのことをきかないでください！");
            }            
        }

        public override void Initialize()
        {
            base.Initialize();
            EdgeWeights = new Dictionary<IAgent, double>();
        }



        //public override void ProcessMessage(BWMessage message)
        //{
        //    base.ProcessMessage(message);
        //    double 
        //    ProcessMessageWithEdgeWeight(message, w);
        //}

        public override void ProcessMessage(BWMessage message)
        {
            double w;

            if (message.From is Sensor)
            {
                w = 1.0;
            }
            else if(message.From is IAgent)
            {
                IAgent neighbor = message.From as IAgent;
                w = getEdgeWeight(neighbor);
            }
            else
            {
                w = 1;
            }


            if (message.From != null && message.From.Accuracy != null)
            {
                UpdateOpinionWithWeight(message.Subject, new BeliefUpdater(message.From.Accuracy.Value), w);//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
            }
            //それ以外は、自分のImportanceLevelを使う
            else
            {
                UpdateOpinionWithWeight(message.Subject, candidateSelector.BeliefUpdater, w);
            }
        }
      

        public void ProcessMessageWithEdgeWeight(BWMessage message, double w)
        {

            IAgent neighbor = message.From as IAgent;
            if (neighbor == null)
            {
                w = getEdgeWeight(neighbor);
            }

            if (message.From != null && message.From.Accuracy != null)
            {
                UpdateOpinionWithWeight(message.Subject, new BeliefUpdater(message.From.Accuracy.Value), w);//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
            }
            //それ以外は、自分のImportanceLevelを使う
            else
            {
                UpdateOpinionWithWeight(message.Subject, candidateSelector.BeliefUpdater, w);
            }
        }

        public void UpdateOpinionWithWeight(BlackWhiteSubject sub, BeliefUpdater updater , double weight)
        {

            double Cupd = BeliefUpdater.decideCupd(sub, updater.ImportanceLevel * weight);
            thought.Belief = BeliefUpdater.updateFunc(thought.Belief, Cupd);

            if (sub == BlackWhiteSubject.White)
            {
                counter.CountUp();
            }
            else //if(sub == BlackWhiteSubject.Black)
            {
                counter.CountDown();
            }
            
        }

        public void EdgeWeightWrite()
        {
            if (this.Neighbours != null)
                foreach (var node in this.Neighbours)
                {
                    double w = this.EdgeWeights[node];
                    Console.WriteLine(w);
                }

            else
            {
                throw new Exception(this.ID + " : の友達はいないです！");
            }
            
        }

        public void neighborsAwarenessrate()
        {
            if (this.Neighbours != null)
            {
                
            }

            else
            {
                throw new Exception(this.ID + " : の友達はいないです！");
            }

        }

        //public override void ProcessMessage(BWMessage message)
        //{
        //    //ここじゃだめかも。Receiveのところでもいっこ穴開けなきゃ行けないかもしれない。！！！！！！！
        //    if (message.From != null && message.From is AgentIO)//送信者がエージェントの場合のみ貯める
        //    {
        //        //messageBox.Enqueue(message);
        //    }


        //    base.ProcessMessage(message);

        //}

        //みんなに知らせる。自分の意見はこうですよと。
        //protected override void NotifyOthers(BlackWhiteSubject? myOpinion = null) 
        //{
        //    BlackWhiteSubject opinion = checkOpinion(myOpinion);

        //    //ご近所さん全員に伝える
        //    //foreach (IAgent neighbour in this.Neighbours)
        //    //    if (!messageBox.ReceivedFrom(neighbour) || //ただし、意見をくれたやつにはあげる必要ない。
        //    //        messageBox.OpinionOf(neighbour) != this.Opinion)//くれてるやつでも、違う意見もってるやつには投げる。
        //    //    {
        //    //        SendOpinion(opinion, neighbour);
        //    //    }

        //    //messageBox = new DRMessageBox();
        //}


    }
}
