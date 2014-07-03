using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing.Subject;
using MyRandom;
using System.Diagnostics;
using System.Threading;


using OpinionSharing.Agt.Algorithm;

namespace OpinionSharing.Agt
{
    public class AAT : AgentAlgorithm, IAATBasedAgent, IFormattable
    {
    #region private メンバ

        public double TargetAwarenessRate { get; set; }

        //エージェントの内部状態
        protected IThought thought;

        //候補集合から重要視度を求めるやつ。
        protected CandidateUpdaterSelector candidateSelector;

        //whiteをもらったら++, Blackをもらったら--
        protected UpdateCounter counter = new UpdateCounter();

    #endregion private メンバ

        public Algorithm.AgentSpec publishSpec() //IAATBasedAgent
        {
            return new Algorithm.AgentSpec(thought.PriorBelief, candidateSelector);
        }

        public override void InheritFrom(AgentAlgorithm otherAlgo)//候補集合、初期値を引き継ぐ。
        {
            if (otherAlgo is IAATBasedAgent)
            {
                AgentSpec spec = (otherAlgo as IAATBasedAgent).publishSpec();

                Thought = new Thought(spec.PriorBelief);
                CandidateSelector = spec.CandidateSelector;

                Body = otherAlgo.Body;
                //候補集合と現在の重要視度だけは引き継ぐ
            }
            else
            {
                throw new Exception(otherAlgo.GetType().Name + "は互換性のない型です。");
            }
        }

        public void thought_OpinionChanged(object sender, OpinionEventArgs e)
        {
            NotifyOthers(e.Opinion);
            OnOpinionChanged(e);
        }

        //コンストラクト時に呼ばれる　が意味ないこともある。どうにかしたい。Factoryの導入か。
        public override void Initialize()
        {
            //親クラスを初期化
            base.Initialize();
            
            //考えを初期化
            if(thought != null)
                thought.Initialize();

            //候補集合を初期化
            if(candidateSelector != null)
                candidateSelector.Initialize();


            counter.Initialize();
        }

        public override void PrepareAlgorithm()
        {
            //候補集合を生成
            candidateSelector.BuildCandidate(Neighbours.Count(), thought);
        }

        public override void RoundInit()
        {
            base.Initialize();//Agentクラスの、メッセージが初期化
            thought.Initialize();
            counter.Initialize();
        }

    #region 各種プロパティ

        public CandidateUpdaterSelector CandidateSelector
        {
            get
            {
                return candidateSelector;
            }
            set
            {
                //今までのがあったならばイベントを削除
               
                /*古いのから新しいのに受け継ぎ，は，なくてもいいかも知れない．
                if (candidateSelector != null)
                {
                    candidateSelector.CandidateChanged -= candidateSelector_CandidateChanged;
                    
                }
                */

                candidateSelector = value;

                /*
                candidateSelector.CandidateChanged += candidateSelector_CandidateChanged;
                    
                */
            }
        }



        public virtual IThought Thought
        {
            get
            {
                return thought;
            }
            set
            {
                //今までのがあったならばイベントを削除
                if (thought != null)
                {
                    thought.OpinionChanged -= thought_OpinionChanged;

                }

                thought = value;
                thought.OpinionChanged += thought_OpinionChanged;
            }
        }

        public override BlackWhiteSubject? Opinion
        {
            get
            {
                return thought.Opinion;
            }
        }

        public virtual double Belief
        {
            get
            {
                return thought.Belief;
            }
        }

        public virtual double PriorBelief
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
                return thought.SigmaRight;
            }
        }

        //そのラウンドで受け取った回数　右なら＋　左なら-
        /*
        public virtual int MaxUpdateNum
        {
            get
            {
                return counter.MaxUpdateNum;
            }
        }
        */

        public IEnumerable<Candidate> Candidates 
        {
            get
            {
                return candidateSelector.Candidates;//candidateManager.Candidates
            }
        }

        public virtual Candidate CurrentCandidate
        {
            get
            {
                return candidateSelector.CurrentCandidate;//candidateManager.SelectedCandidate
            }
        }

        public virtual BeliefUpdater BeliefUpdater
        {
            get
            {
                return candidateSelector.BeliefUpdater;
            }
        }

        public double ImportanceLevel
        {
            get
            {
                return candidateSelector.BeliefUpdater.ImportanceLevel;
            }

        }

        public object CandidateLock { get { return candidateSelector.CandidateLock; } }



    #endregion 各種プロパティ

    #region イベント

        //イベントハンドラ
        //Thoughtに委譲
        public event EventHandler<BeliefEventArgs> BeliefChanged
        {
            add
            {
                thought.BeliefChanged += value;
            }
            remove
            {
                thought.BeliefChanged -= value;
            }
        }

        //UpdaterSelectorに委譲
        public event EventHandler<EstimationEventArgs> EstimationChanged
        {
            add
            {
                candidateSelector.EstimationChanged += value; 
            }

            remove
            {
                candidateSelector.EstimationChanged -= value; 
            }
        }

        public event EventHandler<CandidateEventArgs> CandidateChanged
        {
            add
            {
                candidateSelector.CandidateChanged += value; 
            }
            remove
            {
                candidateSelector.CandidateChanged -= value; 
            }
        }

    #endregion イベント

        public override void ProcessMessage(BWMessage message)
        {
            //センサーからのメッセージならば、センサーの精度分信じる
            if (message.From != null && message.From.Accuracy != null)
            {
                UpdateOpinion(message.Subject, new BeliefUpdater( message.From.Accuracy.Value));//センサーの精度をもとに新たなUpdaterを生成。Accuracy をdouble じゃなくてupdaterにしてもいいかもな
            }
            //それ以外は、自分のImportanceLevelを使う
            else
            {
                UpdateOpinion(message.Subject, candidateSelector.BeliefUpdater);
            }
        }

        public virtual void UpdateOpinion(BlackWhiteSubject sub, BeliefUpdater updater)
        {
            thought.Belief = updater.updateBelief(sub, thought.Belief);

            ////Console.WriteLine("AAT: UpdateOpinion()");
            //意見を変える際に、カウント
            if (sub == BlackWhiteSubject.White)
            {
                counter.CountUp();
            }
            else //if(sub == BlackWhiteSubject.Black)
            {
                counter.CountDown();
            }
        }

        //ラウンドが終わったら、step2,3を実行
        public override void RoundFinished()
        {
            //AATを実行

            //AATアルゴリズム
            if (counter.UpdateMax != 0)//意見を受け取っていない(に等しい)場合は何もしないこいつにとってのラウンドは少ないことになる．
            {
                
                //第２ステップ
                EstimateAwarenessRate();


                //第３ステップ
                SelectTheBest();
            }

            //状態の変更を通知 ここはOpinionChangedじゃないはず。
            //OnOpinionChanged(new OpinionEventArgs(this.Opinion));//おそらくいらない。SelectTheBestでできてるはず。

            //candidate以外を初期化
            RoundInit();
        }

        public virtual void EstimateAwarenessRate()
        {
            candidateSelector.EstimateAwarenessRate(Opinion, counter);
        }


        public virtual void SelectTheBest()
        {
            candidateSelector.SelectTheNext(TargetAwarenessRate);
        }

        public virtual string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (format == "MIN")
            {
                return string.Format("[Agent ID:{0:000}]", ID);
            }

        //もはやいらないが。AAT行き。
            else if (format == "MAX")
            {
                return string.Format("[Agent ID:{0:000}, opinion:{1}\n {2}]", ID, thought, thought.MeterStr);
            }
            else if (format == "METER")
            {
                return string.Format("[Agent ID:{0:000}] {1} {2} {3}",
                    ID, thought.MeterStr, thought.ToString(), candidateSelector.CurrentCandidate);
            }
            else
            {
                return string.Format("[Agent ID:{0:000}, opinion:{1}]", ID, thought);
            }
        }
    }
}
