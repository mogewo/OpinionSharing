using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing.Subject;
using OpinionSharing.Net;
using OpinionSharing.Util;

namespace OpinionSharing.Agt
{
    public delegate void AgentChangedHandler(Agent a);

    public class Agent : IAgent, INode, IOpinionSender, IFormattable
    {
        private const double priorImportanceLevel = 0.5;

    #region private&protectedメンバ
        //エージェントの内部状態
        protected Thought thought;

        // 意見の重要度　AATで調整
        private double importanceLevel = priorImportanceLevel;

        //ノードとしてのエージェントの実装を委譲
        private Node node = new Node();

        //メッセージキュー 一本化したい

        protected MessageQueue messageQueue = new MessageQueue();

        //なるかもしれないBeliefのリスト 表示のため。
        private List<double> beliefList;


    #endregion

        //*** constructor and properties ***//
        public Agent()
        {
            thought = new Thought(RandomPool.Get("setenv").NextNormal(0.5, 0.1));//ランダム要素はここだけ

            Initialize();
        }

        //コンストラクタで初期化された直後の状態に戻す
        public virtual void Initialize()
        {
            messageQueue.Initialize();
            
            thought.Initialize();
            beliefList = new List<double>();
            ImportanceLevel = priorImportanceLevel;

            //状態の変更を通知
            if(agentChanged != null){
                agentChanged(this);
            }
        }

    #region イベント 状態が変わったら通知する

        protected AgentChangedHandler agentChanged;

        public event AgentChangedHandler AgentChanged
        {
            add
            {
                agentChanged += value;
            }
            remove
            {
                agentChanged -= value;
            }
        }
    #endregion

    #region 各種プロパティ

        public double ImportanceLevel
        {
            get
            {
                return importanceLevel;
            }

            protected set
            {
                
                if (!(0.5<=  importanceLevel && importanceLevel < 1))
                {
                    throw new Exception("無効な重要視度" + importanceLevel);
                }

                //重要視度を計算
                importanceLevel = value;

                //重要視度から、将来なりうる信念のリストを再計算
                calcBeliefList(importanceLevel);

                //状態が変わったので通知
                if(agentChanged != null)
                    agentChanged (this);
            }
        }

        public BlackWhiteSubject? Opinion
        {
            get
            {
                return thought.Opinion;
            }
        }

        public double Belief
        {
            get
            {
                return thought.Belief;
            }
        }

        public double PriorBelief
        {
            get
            {
                return thought.PriorBelief;
            }
        }

        public double Sigma
        {
            get
            {
                return thought.Sigma;
            }
        }

        public IEnumerable<double> BeliefList//将来なる可能性があるbeliefたち
        {
            get
            {
                return beliefList;
            }
        }

        public int MessageNum
        {
            get
            {
                return messageQueue.Count;
            }
        }

        public bool ChangedOpinion { get; set; }

    #endregion //各種プロパティ

    #region Interfaceを実装
            public int ID
            {
                get
                {
                    return node.ID;
                }
            }
            public Network Network
            {
                get { return node.Network; }
                set { node.Network = value; }
            }
            public ISet<INode> Neighbours
            {
                get {
                    if (node.Network == null)
                        return new HashSet<INode>();

                    return node.Network.GetNeighbour(this);
                }
            }

            public double? Accuracy
            {
                get { return null; }
            }
        

    #endregion



    #region public method

        public virtual void RoundInit()
        {
            messageQueue.Clear();
            thought.Initialize();

            //状態の変更を通知
            if(agentChanged != null){
                agentChanged(this);
            }
        }

        //他のエージェントから意見をもらう OR センサーから値もらう
        public virtual void ReceiveOpinion(BWMessage message)
        {
            //自分のimportance levelを使う

            
            messageQueue.Enqueue(message);

        }

        //聞こえる　メッセージキューに貯める。
        

        //解釈する　メッセージキューを処理対象に入れる。あたらしいメッセージキューをつくる
        public virtual void Listen()
        {
            messageQueue.Flip();
        }

        //受け取った意見を解釈する。
        public virtual void Think()
        {
            BlackWhiteSubject? oldSubj = thought.Opinion;

            //考える対象がなければ、何もしない。
            if (messageQueue.ProcessQueue.Count() == 0)
            {
                ChangedOpinion = false;
                return;
            }


            ProcessMessages();
        
            //意見が変わっていたら
            if (oldSubj != Opinion)
            {
                ChangedOpinion = true;
                //みんなに伝える
                NotifyOthers();
            }
            else
            {
                ChangedOpinion = false;
            }

        }

        public virtual void ProcessMessages()
        {
            //ProcessQueueの中身を読んで
            messageQueue.ProcessMessage((message) =>
            {
                //センサーからのメッセージならば、センサーの精度分信じる
                if (message.From != null && message.From.Accuracy != null)
                {
                    UpdateOpinion(message.Subject, message.From.Accuracy.Value);//BlackWhiteSubject,double
                }
                //それ以外は、自分のImportanceLevelを使う
                else
                {
                    UpdateOpinion(message.Subject, ImportanceLevel);
                }
            });

            messageQueue.ClearProcessQueue();
        }


        public virtual void UpdateOpinion(BlackWhiteSubject sub, double importanceLevel)
        {
            
            double Cupd = decideCupd(sub, importanceLevel);
            double t = importanceLevel;

            //信念を更新。この結果、意見が変わるかもしれない。
            thought.Belief = updateFunc(thought.Belief, Cupd);

           
            //状態の変更を通知
            if(agentChanged != null){
                agentChanged(this);
            }
        }

        protected virtual void NotifyOthers()
        {
            //ご近所さん全員に伝える
            foreach (Agent neighbour in this.Neighbours)
            {
                if (thought.Opinion == null)
                {
                    throw new Exception("こんなことありえない。バグだ");
                }

                BlackWhiteSubject myopinion = thought.Opinion.Value;//nullableを通常に変換

                SendOpinion(myopinion, neighbour);
            }
        }

        public virtual void SendOpinion(BlackWhiteSubject opinion, Agent to){
            
                to.ReceiveOpinion(new BWMessage(opinion, this));
        }

        //Viewのために計算
        private void calcBeliefList(double ImportanceLevel)
        {
            //ImportanceLevelが変わったら実行
            //beliefとimportanceLevelがあればできる

            beliefList.Clear();

            //更新しないようなパラメータでは、無限ループになってしまう
            if (ImportanceLevel == 0.5)
            {
                return;
            }

            double goright = PriorBelief;
            double goleft = PriorBelief;

            
            do{
                goright = updateFunc(goright,ImportanceLevel);
                beliefList.Add(goright);
            } while(goright < Sigma );//&& i < 5);


            do{
                goleft = updateFunc(goleft, 1-ImportanceLevel);
                beliefList.Add(goleft);
            } while(goleft > (1-Sigma)  );//&& i < 5);

        }

    #endregion public method


    #region override public method
        public override string ToString()
        {
            return this.ToString("MIN");
        }

        public virtual string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (format == "MIN")
            {
                return string.Format("[Agent ID:{0:000}]", ID);
            }
            else if (format == "MAX")
            {
                return string.Format("[Agent ID:{0:000}, opinion:{1}\n {2}]", ID, thought, thought.MeterStr);
            }
            else if (format == "METER")
            {
                return string.Format("[Agent ID:{0:000}] {1}{2}",
                    ID, thought.MeterStr, thought.ToString("VAL"));

            }
            else
            {
                return string.Format("[Agent ID:{0:000}, opinion:{1}]", ID, thought);
            }
        }
    #endregion override public method

    #region static method
        //受け取った意見とimportancelevelからCupdを決める
        protected static double decideCupd(BlackWhiteSubject sub, double importanceLevel)
        {
            return sub == BlackWhiteSubject.White ? importanceLevel : 1 - importanceLevel;
        }

        //更新式
        public static double updateFunc(double currentBelief, double Cupd)
        {
            if (!( 0 <= currentBelief && currentBelief <= 1 )) //belief <- [0,1]
            {
                throw new Exception("異常な信念値" + currentBelief);
            }

            if (!(0 <  Cupd && Cupd < 1)) //Cupd <- [0.5,1)
            {
                throw new Exception("異常な重要視度" + Cupd);
            }

            double numerator = (currentBelief * Cupd);
            double denominator = ((1 - Cupd) * (1 - currentBelief) + Cupd * currentBelief);

            //denominator 0になる条件
            //(Cupd == 1 || currentBelief == 1) && (Cupd == 0 || currentBelief == 0)
            //= {Cupd == 1 && currentBelief == 0 } ||
            //  {Cupd == 0 && currentBelief == 1 }
            //つまり、端っこからどかーんと行こうとすると落ちる
            //ImportanceLevel== 1はやっぱ禁止やな。


            double ret = numerator / denominator;

            if (ret == 1.0)
            {
                ret = 1;
            }

            return ret;
        }

        //n回更新
        public static double updateFunc(int n, double currentBelief, double Cupd)
        {
            /* old */
            double belief = currentBelief;

            //n 回適用
            for (int i = 0; i < n; i++)
            {
                belief = updateFunc(belief, Cupd);
            }

            return belief;
            //*/
        }

    #endregion

    }
}


