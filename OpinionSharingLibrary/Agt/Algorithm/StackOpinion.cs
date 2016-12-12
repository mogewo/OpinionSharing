using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using MyRandom;
using GraphTheory.Net;


using OpinionSharing.Agt.Algorithm;
using Log;

using OpinionSharing.Agt.Updater;

namespace OpinionSharing.Agt
{
    using Subject;

    //意見を決めるまでに、意見をもらった人を覚えておいて、そいつには意見を投げない
    //AwarenessRateの推定がおかしくなる。
    //渡さないのではなくて、解釈しない　＝＞NoMoreBeliefへ。


    public class StackOpinion : AgentAlgorithm, IAATBasedAgent // 抜本的な改革かもしれないので、IAgentを実装するだけになるかも。
    {

    #region private メンバ
        //
        ThoughtStack thought;

        //候補集合から重要視度を求めるやつ。
        //COMMON
        CandidateUpdaterSelector candidateSelector = new CandidateUpdaterSelector();

        //これの更新方法を改善する案もあったな。right とleftで
        //これはどうカプセル化するべきか。ぶっちゃけ系列を保存しておけばそこから計算できるしそうしようかな。

        protected UpdateCounter counter = new UpdateCounter();



    #endregion private メンバ

        public StackOpinion(double h = 0.9): base()
        {
        }

        void thought_OpinionChanged(object sender, OpinionEventArgs e)
        {
            //この段階では、前の段階のOpinionがある
            //thoughtStack.CurrentThought.Opinion

            NotifyOthers( e.Opinion );

            //自信のイベントも連鎖して発火
            OnOpinionChanged(e);


            //コノ後、新しい意見が積み重なってしまうので注意。
            //気を配らなきゃいけないのめんどくさいので設計ヨクない
        }


        //コンストラクト時に呼ばれる
        //COMMON
        public override void Initialize()
        {
            base.Initialize();
            if (thought != null) //なにか初期化のいい方法はないものか・・・
            {
                thought.Initialize();
            }
            
            candidateSelector.Initialize();


            counter.Initialize();
        }

        //COMMON
        public override void RoundInit()
        {
            base.Initialize();//Agentクラスの、メッセージが初期化

            thought.Initialize();

            counter.Initialize();

        }

        public Algorithm.AgentSpec publishSpec() //IAATBasedAgent
        {
            return new Algorithm.AgentSpec(thought.PriorBelief, candidateSelector);
        }

        public override void InheritFrom(AgentAlgorithm otherAlgo)
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
                throw new Exception("互換性のない型です。");
            }
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
                candidateSelector = value;
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
                //型をチェック
                if (!(value is ThoughtStack))
                {
                    throw new Exception("互換性がありません");
                }

                //今までのがあったならばイベントを削除
                if (thought != null)
                {
                    thought.OpinionChanged -= thought_OpinionChanged;

                }

                thought = value as ThoughtStack;
                thought.OpinionChanged += thought_OpinionChanged;
            }
        }


        //COMMON
        public override BlackWhiteSubject? Opinion
        {
            get
            {
                if (thought != null)
                {
                    return thought.Opinion;
                }
                else
                {
                    return null;
                }
            }
        }

        //COMMON
        public double Belief
        {
            get
            {
                //あえて代表して言うなら
                return thought.Belief;
            }
        }

        //COMMON
        public double PriorBelief
        {
            get
            {
                return thought.PriorBelief;
            }
        }

        //COMMON
        public double Sigma
        {
            get
            {
                return thought.SigmaRight;
            }
        }

        //そのラウンドで受け取った回数　右なら＋　左なら-
        //COMMON
        /*
        public int MaxUpdatedNum
        {
            get
            {
                return counter.MaxUpdateNum;
            }
        }
        */

        //COMMON
        public IEnumerable<Candidate> Candidates 
        {
            get
            {
                return candidateSelector.Candidates;//candidateManager.Candidates
            }
        }

        //COMMON
        public Candidate CurrentCandidate
        {
            get
            {
                return candidateSelector.CurrentCandidate;//candidateManager.SelectedCandidate
            }
        }

        //COMMON
        public BeliefUpdater BeliefUpdater
        {
            get
            {
                return candidateSelector.BeliefUpdater;
            }
        }

        //COMMON
        public double ImportanceLevel
        {
            get
            {
                return candidateSelector.BeliefUpdater.ImportanceLevel;
            }
            /*

            protected set{
                //考えどころ。外部から設定していいのか？たぶんよくない。イベントハンドラを使うべき。
                calcBeliefList(value);
            }
            */

        }

        //COMMON
        public object CandidateLock { get { return candidateSelector.CandidateLock; } }


        //COMMON

        public double TargetAwarenessRate { get; set; }


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
        //COMMON
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

        //COMMON
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


        //COMMON
        public override void PrepareAlgorithm()
        {
            candidateSelector.BuildCandidate(Neighbours.Count(), thought.CurrentThought);
        }


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
        public override void RoundFinished(BlackWhiteSubject thefact)
        {
            //AATを実行

            //AATアルゴリズム
            if (counter.UpdateMax != 0)//意見を受け取っていない(に等しい)場合は何もしない
                //本当にそれでいいのか！？！！！！！！！！！！！
            {
                //第２ステップ
                EstimateAwarenessRate();

                //第３ステップ
                SelectTheBest();
            }

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
                return string.Format("[Agent ID:{0:000}, opinion:{1}\n {2}]", ID, thought.CurrentThought, thought.CurrentThought.MeterStr);
            }
            else if (format == "METER")
            {
                return string.Format("[Agent ID:{0:000}] {1} {2} {3}",
                    ID, thought.CurrentThought.MeterStr, thought.CurrentThought.ToString("VAL"), candidateSelector.CurrentCandidate);

            }
            else
            {
                return string.Format("[Agent ID:{0:000}, opinion:{1}]", ID, thought.CurrentThought);
            }
        }
    }
}
