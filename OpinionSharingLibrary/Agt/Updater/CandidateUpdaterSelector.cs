using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OpinionSharing.Subject;
using OpinionSharing.Agt.Algorithm;
using GraphTheory.Net;
using OpinionSharing.Agt;

namespace OpinionSharing.Agt
{
    //

    public class CandidateUpdaterSelector
    {

    #region privateメンバ


        protected Candidate[] candidates;

        private Candidate currentCandidate;

        //1007重みは各候補に対応したものと紐づけたい，ならばここでやるのが吉か？
        private AgentIO agt;    //重みのためのエージェント
        private BWMessage mes;  //重みのためのメッセージ
        private INode node;

        public readonly object CandidateLock = new Object();

    #endregion privateメンバ

    #region プロパティ


        public IEnumerable<Candidate> Candidates
        {
            get
            {
                return candidates;//candidateManager.Candidates
            }
        }

        public Candidate CurrentCandidate
        {
            get
            {
                return currentCandidate;//candidateManager.SelectedCandidate
            }
            set
            {
                currentCandidate = value;

                if (currentCandidate != null)
                {
                    OnCandidateChanged(new CandidateEventArgs(currentCandidate));
                }
            }
        }


        public BeliefUpdater BeliefUpdater //これをしたいがためにごちゃごちゃやっている。
        {
            get
            {
                if (CurrentCandidate == null)
                {
                    return new BeliefUpdater();
                }

                return CurrentCandidate.BeliefUpdater;
            }
        }


        public double ImportanceLevel{
            get
            {
                if (this.BeliefUpdater == null)
                {
                    return 0.5;
                }
                else{
                    return BeliefUpdater.ImportanceLevel;
                }
            }
        }

        public AgentIO Agt
            {
                get
                {
                    return agt;
                }
                set
                {
                    agt = value;
                }
            }

        public BWMessage Mes
            {
                get
                {
                    return mes;
                }
                set
                {
                    mes = value;
                }
            }

        public INode Node
            {
                get
                {
                    return node;
                }
                set
                {
                    node = value;
                }
            }

    #endregion プロパティ

    #region イベント

        public event EventHandler<CandidateEventArgs> CandidateChanged = (sender,e) => { };
        public event EventHandler<EstimationEventArgs> EstimationChanged = (sender,e) => { };

        protected virtual void OnCandidateChanged(CandidateEventArgs e){

            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CandidateEventArgs> handler = CandidateChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnEstimationChanged(EstimationEventArgs e){

            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EstimationEventArgs> handler = EstimationChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

    #endregion イベント

        //コンストラクタ
        public CandidateUpdaterSelector()
        {
            Initialize();

        }//method

        public void Initialize()
        {
            candidates = null;
            CurrentCandidate = null;

        }

        //候補集合を初期化
        public void BuildCandidate(int neighbourNum, IThought thought)//ご近所の数と初期重要視度を渡されることで候補を生成
        {
            if (neighbourNum <= 0)
            {
                return;
            }

            //Sigma=Thought   PriorBelief=Thought   updateFunc=BeliefUpdater
            lock (CandidateLock)
            {
                //ご近所の数
                int dn = neighbourNum;

                //ご近所の数の２倍だけ作成
                candidates = new Candidate[2 * dn];

                double sigmaRight = thought.SigmaRight;
                double sigmaLeft = thought.SigmaLeft;

                double impLvl = 0.5;
                // Jumpnum = 3, 2, 1と調べていく．
                for (int l = dn; l >= 0; l--)
                {
                    //importance levelを上げていって，beliefがしきい値を超えたときその値を使う．
                    for (; impLvl <= 1; impLvl += 0.001)
                    {
                        if (BeliefUpdater.updateFunc(thought.PriorBelief, impLvl, l) > sigmaRight)
                        {
                            Candidate newcan = new Candidate(l, impLvl);

                            //この重要視度で、逆側に行くときは、何ステップでいけるのか？
                            double tmpBelief = thought.PriorBelief;
                            int count = 0;
                            do
                            {
                                tmpBelief = BeliefUpdater.updateFunc(tmpBelief, 1 - impLvl);//逆では1-impLvl
                                count++;

                            } while (!(tmpBelief <= sigmaLeft));

                            newcan.OtherJumpNum = count;

                            candidates[l - 1] = newcan;
                            break;
                        }
                    }
                }

                impLvl = 0.5;
                for (int l = dn; l >= 0; l--)
                {
                    for (; impLvl <= 1; impLvl += 0.001) //impLvl ∈ (0.5, 1.0)
                    {
                        if (BeliefUpdater.updateFunc(thought.PriorBelief, 1 - impLvl,l) < sigmaLeft)
                        {
                            //重要視度を決定
                            Candidate newcan = new Candidate(-l, impLvl);

                            //この重要視度で、逆側に行くときは、何ステップでいけるのか？
                            double tmpBelief = thought.PriorBelief;
                            int count = 0;
                            do
                            {
                                tmpBelief = BeliefUpdater.updateFunc(tmpBelief, impLvl);//1-impLvlの逆なのでimpLvl
                                count++;

                            } while (!(tmpBelief >= sigmaRight));//0.8を超えたら

                            newcan.OtherJumpNum = count;


                            candidates[l - 1 + dn] = newcan;

                            break;
                        }
                    }
                }
            }//lock
            CurrentCandidate = candidates.Last();

            OnEstimationChanged(new EstimationEventArgs(Candidates));

            OnCandidateChanged(new CandidateEventArgs(CurrentCandidate));
        }

        public virtual void EstimateAwarenessRate(BlackWhiteSubject? Opinion, UpdateCounter counter){

            foreach (var can in Candidates)
            {
                //この候補は意見形成できるか？
                bool formed = OpinionFormed(can,Opinion,counter);
                //↑can.EstimateAwarenessRateに入れてしまってもいいが入れられない．結構依存が大きい OpinionFormedはImportaceLevelを使う
                //推定値を更新 //意見が更新できたか
                //1006ここで各候補の意見形成率を更新
                //つまりcandidateクラスで反映すべき内容か？ほしい情報は候補のkeyと近隣エージェントをひもづけすること．
                //そうすれば各重みに反映した重みを生かすことが出来る
                can.EstimateAwarenessRate(formed);

            }

            OnEstimationChanged(new EstimationEventArgs(Candidates));
        }

        public virtual bool OpinionFormed(Candidate can, BlackWhiteSubject? Opinion, UpdateCounter counter)
        {
            //ImportanceLevelに依存

                //3つの条件の論理式

                //意見を決められたか
                bool determined = Opinion != null;

                //現在のより大きいか？
                bool bigger = can.ImportanceLevel >= ImportanceLevel;

                //この候補にとって十分な意見が来たか？
            /*  bool enoughUpd = counter.UpdateNum  > 0 ? //この場合わけをすべきか？
                         counter.UpdateRight >= can.JumpNumRight : //正だったらrightと比べる
                         counter.UpdateLeft  >= can.JumpNumLeft; //負だったらleftと比べる */

                bool enoughUpd =    counter.UpdateRight >= can.JumpNumRight 
                                 || counter.UpdateLeft  >= can.JumpNumLeft;//一番近いσを，超えられるか

                //意見を決められていて、候補の方が大きいか、　もしくは、十分な意見が来たか。
                return (determined && bigger) || enoughUpd;
        }

        public void SelectTheNext(double TargetAwarenessRate)
        {
            //昇順に並び替える
            IEnumerable<Candidate> sorted = Candidates.OrderBy((el) => el.ImportanceLevel);
            Candidate[] sortedCandidates = sorted.ToArray();


            {
                //最初（currentCandidateが一度も選ばれてないとき）は適当に選択
                if (CurrentCandidate == null)
                {
                    CurrentCandidate = sortedCandidates.Last();//おっきいやつを選ぶ;
                    return;
                }

                //現在使っている候補
                int i = Array.IndexOf(sortedCandidates, CurrentCandidate);

                if (i == -1)
                {
                    throw new Exception("おかしいんでない？");
                }

                //小さければ一個すすめる
                if (sortedCandidates[i].AwarenessRate < TargetAwarenessRate)
                {
                    i++;
                }
                //一個下がh_trg以上ならば戻る．
                else if ((i > 0) && (sortedCandidates[i - 1].AwarenessRate > TargetAwarenessRate))//ここに=が入るかどうかは大問題。=入れて実験してみたい。=入れれば0.94ではなく0.9になったりして。
                {
                    i--;
                }
                else
                {
                    ;//現状維持 
                }

                i = Math.Max(0,Math.Min(sortedCandidates.Length - 1, i));

                CurrentCandidate = sortedCandidates[i];
                
                //イベントを発火
                OnCandidateChanged(new CandidateEventArgs(CurrentCandidate));

            } // */
        }
    }
}
