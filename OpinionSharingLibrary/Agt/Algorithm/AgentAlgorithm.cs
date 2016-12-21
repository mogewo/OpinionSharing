using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing.Subject;
using GraphTheory.Net;
using MyRandom;


namespace OpinionSharing.Agt
{

    public abstract class AgentAlgorithm : IAgent
    {

        public void Listen()
        {
        }
        public void ProcessMessages()
        {
        }


        public virtual void connected(INode neighbour)
        {
            Console.WriteLine("connected to " + neighbour);
            return ;
        }

        public virtual void disconnected(INode neighbour)
        {
            Console.WriteLine("disconnected from" + neighbour);

            return;
        }


    #region private&protectedメンバ
        private AgentIO body;

    #endregion

        //*** constructor and properties ***//
        public AgentAlgorithm()
        {
            Initialize();
        }


        //コンストラクタで初期化された直後の状態に戻す
        public virtual void Initialize()
        {
            //状態の変更を通知
            //OnOpinionChanged(new OpinionEventArgs(Opinion));
        }



    #region イベント 状態が変わったら通知する

        public event EventHandler<OpinionEventArgs> OpinionChanged; 

        protected virtual void OnOpinionChanged(OpinionEventArgs e){
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

    #region 各種プロパティ

        public double? Accuracy
        {
            get
            {
                return null;
            }
        }
        
        //意見が変わったかどうか。
        public bool ChangedOpinion { get; set; }

        public virtual AgentIO Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }

        public int ID
        {
            get
            {
                if (body != null) return body.ID;
                else return 0;
            }
        }


        //これこそが大事なもの。アドレス帳
        public IEnumerable<IAgent> Neighbours
        {
            get {
                foreach (IAgent a in body.Neighbours)
                {
                    yield return a;
                }
            }
        }

    #endregion //各種プロパティ

    #region IAgentを実装

        //*** IOpinionSender ***


        //*** IAgent *** 


        public virtual void ReceiveOpinion(BWMessage message)
        {
        }


        //fromを自分にして、意見を送信する。
        public virtual void SendOpinion(BlackWhiteSubject opinion, IAgent to){
            to.ReceiveOpinion(new BWMessage(opinion, this.body));
        }


    #endregion

    #region 抽象メソッド
            //とりあえず意見があることはわかる
            public abstract BlackWhiteSubject? Opinion{get;} 

            //ここだけ派生クラスで考えてね。
            public abstract void ProcessMessage(BWMessage message);

            public abstract void InheritFrom(AgentAlgorithm otherAlgo);

    #endregion 抽象メソッド

    #region public method
        //みんなに知らせる。自分の意見はこうですよと。
        protected virtual void NotifyOthers(BlackWhiteSubject? myOpinion = null)
        {
            BlackWhiteSubject opinion = checkOpinion(myOpinion);
            //ご近所さん全員に伝える
            foreach (IAgent neighbour in this.Neighbours)
            {
                SendOpinion(opinion, neighbour);
            }
        }

        protected virtual BlackWhiteSubject checkOpinion(BlackWhiteSubject? opinion)
        {
            if (opinion == null) //引数が省略されたら
            {
                if(this.Opinion == null){ //引数が省略されたのに自分の意見もなかったら、バグ
                    throw new Exception("this.Opinionがnullの状態で呼び出さないで。バグです。");
                }
                else //引数が省略され、自分の意見がある場合は
                {
                    opinion = this.Opinion;
                }
            }
            return opinion.Value;
        }

        public abstract void PrepareAlgorithm();

        public abstract void RoundInit();

        public abstract void RoundFinished(BlackWhiteSubject? thefact);

    #endregion public method

    #region override public method

        public override string ToString()
        {
            return string.Format("[Agent ID:{0} opinion:{1}]", ID, Opinion);
        }

    #endregion override public method


    }
}


