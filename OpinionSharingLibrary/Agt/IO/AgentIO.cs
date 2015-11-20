using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing.Subject;

using MyRandom;
using GraphTheory.Net;
using OpinionSharing.Agt.Algorithm;


namespace OpinionSharing.Agt
{
    public class AgentIO : INode, IAgent 
    {
        #region private&protectedメンバ

        //class Human :DelegativeAgentをつくることを考えよう
        //ご近所をもつのはかなりFundamental. 送信用のアドレス帳のようなものか。
        //ノードとしてのエージェントの実装を委譲　いうほど委譲できてない気も？
        private Node node = new Node();

        //メッセージキュー
        //通信するという意味で、やはりFundamental.
        //受信用のポートみたいな。ひとつだけ。誰から来たかはmessage.fromの値で区別する。
        protected MessageQueue messageQueue = new MessageQueue();

        //実際に処理する実装
        //protected AgentAlgorithm algorithm;

        //解析のためとりあえずpublicにしました 11/20
        public AgentAlgorithm algorithm;



        //センサーエージェントの場合はセンサーをもっている
        protected Sensor sensor;

        ////インフルエンシャル（リーダーかも）の場合は目的をもっている
        ////　　11/13追記
        //protected Purpose purpose;

        //Network net;

        #endregion


        //*** constructor and properties ***//
        public AgentIO()
        {
            Initialize();

        }


        //コンストラクタで初期化された直後の状態に戻す
        public virtual void Initialize()
        {
            messageQueue.Initialize();



            //状態の変更を通知
            //OnOpinionChanged(new OpinionEventArgs(Opinion));
        }

        #region プロパティ

        public virtual AgentAlgorithm Algorithm
        {
            get
            {
                return algorithm;
                 
            }
            set
            {
                //相互参照関係を構築
                //あとでイベントなども。
                //変更されるときは、Candidateの引き継ぎなども。

                if (algorithm != null)
                {
                    //旧アルゴリズムからイベントの削除
                    RemoveEvents();
                    //引き継ぎ
                    value.InheritFrom(algorithm);
                }

                //初めて登録されるとき 引き継ぎがない。
                else
                {
                    value.Body = this;
                }

                //新アルゴリズムを登録
                algorithm = value;

                //近隣の追加 WeightedNeighbourアルゴリズム
                //if (algorithm is WeightedNeighbour)
                //{
                //    (algorithm as WeightedNeighbour).Neighbours = this.Neighbours;
                //}

                //重みの追加 WeightedNeighbourアルゴリズム
                //if (algorithm is WeightedNeighbour)
                //{
                //    (algorithm as WeightedNeighbour).Edgeweights = this.Edgeweights;
                //}


                //新アルゴリズムにイベントを追加
                AddEvents();
            }
        }

        public bool Drawable
        {
            get
            {
                return algorithm != null;
            }
        }

        protected virtual void AddEvents()
        {

            algorithm.OpinionChanged += algorithm_OpinionChanged;

        }

        protected virtual void RemoveEvents()
        {

            algorithm.OpinionChanged -= algorithm_OpinionChanged;

        }

        void algorithm_OpinionChanged(object sender, OpinionEventArgs e)
        {
            OnOpinionChanged(e);
        }

        public virtual BlackWhiteSubject? Opinion
        {
            get
            {
                if (algorithm == null)
                {
                    return null;
                }
                return algorithm.Opinion;
            }
        }


        #endregion プロパティ

        #region イベント 状態が変わったら通知する

        public event EventHandler<OpinionEventArgs> OpinionChanged;

        protected virtual void OnOpinionChanged(OpinionEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<OpinionEventArgs> handler = OpinionChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion


        #region INodeを実装 ホントはNodeNetworkはジェネリクスで
        //*** INode ***
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

        //これこそが大事なもの。アドレス帳
        public ISet<INode> Neighbours
        {
            get
            {
                if (node.Network == null)
                    return new HashSet<INode>();

                return node.Network.GetNeighbour(this);
            }
        }

        #endregion INodeを実装

        #region IOuterAgent を実装 詳細は委譲

        //fromを自分にして、意見を送信する。
        public virtual void SendOpinion(BlackWhiteSubject opinion, IAgent to)
        {
            to.ReceiveOpinion(new BWMessage(opinion, this));
        }

        //自分はこれぐらい信じていいですよ がない=null
        public virtual double? Accuracy
        {
            get { return null; }
        }

        //エージェント本体の初期化を行う
        public virtual void PrepareAlgorithm()
        {
            algorithm.PrepareAlgorithm();
        }

        //他のエージェント(意見送信できるなにか)から意見をもらう OR センサーから値もらう
        //ここで重みの確率で意見をもらうかもらわないかを行えばいい？
        public virtual void ReceiveOpinion(BWMessage message)
        {
    
            //確率更新用
            //初期乱数生成
            //double rReceive = RandomPool.Get("opinionupdateset").NextDouble();
            //double aveR = NetworkIndexes.AveEdgeweight(this);
            //
            //初期化
            //double e = this.getEdgeWeight(message);

            ////null対策
            ////neighborがnullでならばneighbor.IDでeに重みを代入
            //if (neighbor != null)
            //{
            //    e = this.Edgeweights[neighbor.ID];//意見を受け取った人と自分の重み：最初はnullなので初期化する必要あり
            //}
            
            //Console.WriteLine(e);

            /*if (HasSensor)
            {
                algorithm.ReceiveOpinion(message);
                //Queueに送信 
                messageQueue.Enqueue(message);
            }
            else
            {
                
                if (rReceive <= e)
                {
                    algorithm.ReceiveOpinion(message);
                    
                    //Queueに送信 
                    messageQueue.Enqueue(message);
                }                                 
            }*/

            algorithm.ReceiveOpinion(message);
            //Queueに送信 
            messageQueue.Enqueue(message);
        }

        //解釈する　メッセージキューを処理対象に入れる。あたらしいメッセージキューをつくる
        public virtual void Listen()
        {
            messageQueue.Flip();
        }

        //ラウンドの最初に実行する
        public virtual void RoundInit()
        {
            messageQueue.Clear();
            algorithm.RoundInit();
        }

        //
        public virtual void RoundFinished()
        {
            algorithm.RoundFinished();
        }

        #endregion IAgentを実装

        public virtual void ProcessMessages()
        {
            //ProcessQueueの中身を読む。中身はalgorithmに委譲
            messageQueue.ProcessMessage(algorithm.ProcessMessage);

            messageQueue.ClearProcessQueue();
        }

        public virtual void ProcessMessage(BWMessage mes)
        {
        }

        #region override public method
        public override string ToString()
        {
            return string.Format("[DelegativeAgent ID:{0} opinion:{1}]", ID, Opinion);
        }

        #endregion override public method

        //意見受け取り重み関数
        //public double getEdgeWeight(BWMessage message)
        //{
        //    INode neighbor = message.From as INode;
        //    double w = this.Edgeweights[neighbor.ID];

        //    return w;
        //}

        internal void SetSensor(Sensor s)
        {
            sensor = s;
        }

        public bool HasSensor
        {
            get
            {
                return sensor != null;
            }
        }

        public IDictionary<INode, int> Distances
        {
            get
            {
                return node.Distances;
            }
        }

        //重みエッジ
        //public IDictionary<int, double> Edgeweights
        //{
        //    get
        //    {

        //        return node.Edgeweights;
        //    }
        //}

        
       
        public IList<Dictionary<INode, int>> Betweens
        {
            get
            {
                return node.Betweens;
            }
        }


        //表示したい情報を返す
        public string Status
        {
            get
            {
                return "nodeID," + node.ID + "\r\n" +
                        "degree," + this.Neighbours.Count + "\r\n" +
                        "cluster," + NetworkIndexes.cluster(this) + "\r\n" +
                        "degreeCentrality," + NetworkIndexes.degreeCentrality(this, Network) + "\r\n" +
                        "closenessCentrality," + NetworkIndexes.closenessCentrality(node) + "\r\n" +
                        "AverageDistance," + NetworkIndexes.averageDistanceNode(node) + "\r\n" +
                        "maxDistance," + NetworkIndexes.maxDistanceNode(node) + "\r\n" +
                        "minDistance," + NetworkIndexes.minDistanceNode(node) + "\r\n";
                        //"maxEdgeWeight," + NetworkIndexes.maxEdgeweight(node) + "\r\n" +
                        //"minEdgeWeight," + NetworkIndexes.minEdgeweight(node) + "\r\n" +
                        //"aveEdgeWeight," + NetworkIndexes.AveEdgeweight(node);                         
            }
        }

        public string CsvStatus
        {
            get
            {
                return 
                        node.ID + "," +
                        this.Neighbours.Count + "," +
                        NetworkIndexes.cluster(this) + "," +
                        NetworkIndexes.degreeCentrality(this, Network) + "," +
                        NetworkIndexes.closenessCentrality(node) + "," +
                        NetworkIndexes.averageDistanceNode(node) + "," +
                        NetworkIndexes.maxDistanceNode(node) + "," +
                        NetworkIndexes.minDistanceNode(node);

            }
        }

        //public string CsvAwarenessRate(IAATBasedAgent thisAgent)
        //{
        //    this.Network.
        //    foreach (var can in thisAgent.Candidates)
        //    {
        //        if (can == thisAgent.CandidateSelector.CurrentCandidate)
        //        {
        //            return
        //                "nodeID," + node.ID + "\r\n" +
        //                "JumpNumLeft," + can.JumpNumLeft + "\r\n" +
        //                "JumpNumRight," + can.JumpNumRight + "\r\n" +
        //                "ImportanceLevel" + can.ImportanceLevel + "\r\n" +
        //                "AwarenessRate" + can.AwarenessRate;
        //        }
        //    }
        //    return "値が代入できていません";

        //}

        //目的をセットする
        //もしかしたらいらないかも
        //internal void SetPurpose(Purpose p)
        //{
        //    purpose = p;
        //}

        //目的を持っているかどうか
        //public bool HasPurpose
        //{
        //    get
        //    {
        //        return purpose != null;
        //    }
        //{
        //    get
        //    {
        //        return sensor != null;

        //    }
        //}
    }
}


