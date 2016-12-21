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
    using OpinionSharing.Agt.Updater;

    //意見を決めるまでに、意見をもらった人を覚えておいて、正解と比較する
    public class WNMessageBox
    {
        Queue<BWMessage> messages;
        Dictionary<IOpinionSender, BWMessage> latestMessages;

        public WNMessageBox()
        {
            messages = new Queue<BWMessage>();
            latestMessages = new Dictionary<IOpinionSender, BWMessage>();
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
        public IEnumerable<BWMessage> LatestMessages
        {
            get
            {
                foreach (var item in latestMessages)
                {
                    yield return item.Value;
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

        public void Enqueue(BWMessage message)
        {
            messages.Enqueue(message);
            latestMessages[message.From] = message;
           
        }
    }

    public class WeightedNeighbour : LimitedBelief
    {
        //意見をくれたご近所
        //protected DRMessageBox messageBox;
        //public ISet<IAgent> Neighbours { get; set; }
        //public ISet<INode> Neighbours { get; set; }
        //意見をくれたご近所
        protected WNMessageBox messageBox;


        #region 各種プロパティ
        public IDictionary<IAgent, double> EdgeWeights{ get; set; }

        public override void RoundInit()
        {
            base.RoundInit();

            messageBox = new WNMessageBox();
        }

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
                return this.EdgeWeights[a];
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
                //this.EdgeWeights[a] = RandomPool.Get("WeightSet").NextDouble(0.9, 1.0); //とりあえず0.9~1.0
                this.EdgeWeights[a] = 0.5;
            }
            else
            {
                throw new Exception(this.ID + " : 友達じゃない"+a+"の重みのことをきかないでください！");
            }            
        }

        public void updateEdgeWeight(IAgent neighbour)
        {
            //20161122
            //if (neighbour != null)
            //{

            //    if (neighbour.Opinion != null)
            //    {
            //        var o = neighbour.Opinion as BlackWhiteSubject;
            //    }

            //    var mes = new BWMessage(neighbour.Opinion, neighbour);
            //    IAgent neighbor = mes.From as IAgent;

            //    if (this.Neighbours.Contains(neighbor))
            //    {
            //        this.EdgeWeights[neighbour] = RandomPool.Get("envset").NextDouble(0.9, 1.0); //とりあえず0.9~1.0
            //        double w = this.EdgeWeights[neighbour];

            //        if (mes.From != null && mes.From.Accuracy != null)
            //        {
            //            UpdateOpinionWithWeight(mes.Subject, new BeliefUpdater(mes.From.Accuracy.Value), w);//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
            //        }
            //        //それ以外は、自分のImportanceLevelを使う
            //        else
            //        {
            //            UpdateOpinionWithWeight(mes.Subject, candidateSelector.BeliefUpdater, w);
            //        }
            //    }
            //}
            
            ////if (this.Neighbours != null)
            ////{
            ////    IAgent n = neighbour as IAgent;
            ////    if (this.Neighbours.Contains(n))
            ////    {
            ////        this.EdgeWeights[n] = RandomPool.Get("envset").NextDouble(0.9, 1.0); //とりあえず0.9~1.0
            ////        double weight = this.EdgeWeights[n];
            ////        return weight;
            ////        //this.ProcessMessage(message);    
            ////    }
            ////    //this.EdgeWeights[a] = 0.9;//
            ////}
            ////else
            ////{
            ////    throw new Exception(this.ID + " : 友達じゃない" + neighbour+ "のことをきかないでください！");
            ////}
        }

        public override void Initialize()
        {
            base.Initialize();
            EdgeWeights = new Dictionary<IAgent, double>();
            messageBox = new WNMessageBox();
        }

        //重みの平均を計算する関数
        public double ave_getEdgeWeight(IAgent a)
        {
            double sum = 0;

            if (this.Neighbours != null)
                foreach (var node in this.Neighbours)
                {
                    sum += this.EdgeWeights[node];
                }

            else
            {
                throw new Exception(this.ID + " : の友達はいないです！");
            }

            double ave = sum / EdgeWeights.Count;

            return ave;
        }


        //public override void ProcessMessage(BWMessage message)
        //{
        //    base.ProcessMessage(message);
        //    double 
        //    ProcessMessageWithEdgeWeight(message, w);
        //}

        public override void ProcessMessage(BWMessage message)
        {
            if (message.From != null && message.From is AgentIO)//送信者がエージェントの場合のみ貯める
            {
                messageBox.Enqueue(message);
            }

            double w;

            if (message.From is Sensor)
            {
                w = 0.5;
            }
            else if(message.From is IAgent)
            {
                IAgent neighbor = message.From as IAgent;
                w = getEdgeWeight(neighbor);
                //w = ave_getEdgeWeight(neighbor);
                //w = 0.5;
            }
            else
            {
                w = 0.5;
            }
           
            //tの更新関数

            //base.ProcessMessage();


            //センサーからのメッセージならば、センサーの精度分信じる
            if (message.From is Sensor)
            {
                //UpdateOpinion(message.Subject, new BeliefUpdater( message.From.Accuracy.Value));//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
                UpdateOpinion(message.Subject, new BeliefUpdater( 0.55 ));//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
                //注意！！センサーの精度を決め打ちで0.55と信じちゃってるエージェント．
                //だからこそ，今は精度の低いセンサーに騙されちゃってるから好都合だからこうしてる．            
            }
            //それ以外は、自分のImportanceLevelを使う
            else
            {
                UpdateOpinion(
                    message.Subject, 
                    new WeightedBeliefUpdater(candidateSelector.BeliefUpdater.ImportanceLevel,w)
                );
            }

            

            //if (message.From != null && message.From.Accuracy != null)
            //{
            //    UpdateOpinionWithWeight(message.Subject, new BeliefUpdater(message.From.Accuracy.Value), w);//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
            //}
            ////それ以外は、自分のImportanceLevelを使う
            //else
            //{
            //    UpdateOpinionWithWeight(message.Subject, candidateSelector.BeliefUpdater, w);
            //}
        }

        public void checkFact(BlackWhiteSubject fact)
        {
           /*自分が所持するメッセージリストと比較し，答え合わせをする*/
            foreach (var mes in messageBox.LatestMessages)
            {
                double updateWidth = 0;

                double widthDiff = 0.01;
                if (mes.Subject == fact)
                {
                    //wを挙げる
                    updateWidth =0.5 + widthDiff;
                }
                else
                {
                    //wを下げる
                    updateWidth =0.5 - widthDiff;
                }

                var newWeight = BeliefUpdater.updateFunc(this.EdgeWeights[mes.From as IAgent], updateWidth);
                
                //w>=0.5にならないようにする．精度の低いセンサーの意見のみを弾くため，if文いらなくね？                
                double min = System.Math.Min(newWeight, 0.5);
                newWeight = min;


                this.EdgeWeights[mes.From as IAgent] = newWeight;
            }

        }

        //public void checkFactLastmes(BlackWhiteSubject fact)
        //{
        //    //opinionsenderの意見をリスト（）で保存
        //    //dictionaryは添え字が同じなら上書きされるので，そちらを参照したほうがいいかも
        //    //List<BlackWhiteSubject> indiviual_messages = new List<BlackWhiteSubject>();
 
        //    /*自分が所持するメッセージリストの内，最後の意見をを答え合わせをする*/
        //    foreach (var mes in messageBox.Messages)
        //    {
                
        //        double updateWidth = 0;

        //        if (mes.Subject == fact)
        //        {
        //            //wを挙げる
        //            updateWidth = 0.51;
        //        }
        //        else
        //        {
        //            //wを下げる
        //            updateWidth = 0.49;
        //        }

        //        var newWeight = BeliefUpdater.updateFunc(this.EdgeWeights[mes.From as IAgent], updateWidth);
        //        if (newWeight >= 0.5)
        //        {
        //            double max = System.Math.Min(newWeight, 0.5);
        //            newWeight = max;
        //        }

        //        this.EdgeWeights[mes.From as IAgent] = newWeight;
        //    }

        //}

      

        public void ProcessMessageWithEdgeWeight(BWMessage message, double w)
        {
            
            //IAgent neighbor = message.From as IAgent;
            //INode n = neighbor as INode;

            //if (neighbor == null)
            //{
            //    w = updateEdgeWeight();
            //    w = ave_getEdgeWeight(neighbor);//平均でやるときはこっち
            //}

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

        public override void RoundFinished(BlackWhiteSubject? thefact)
        {
            if (thefact.HasValue)
            {
                this.checkFact(thefact.Value);
            }
            base.RoundFinished(thefact);
        }

        //ラウンドの最後に「近隣の重み」を「選択された信用度」を一括で表示する
        public void weightlog()
        {
            //this.ID
            foreach (var mes in messageBox.Messages)
            {

            }
        }
    }
}
