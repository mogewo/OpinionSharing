using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing.Subject;
using OpinionSharing.Util;
using System.Diagnostics;
using System.Threading;


namespace OpinionSharing.Agt
{
    public class AAT : Agent
    {

    #region private メンバ

        private double  h_trg = 0.9;

        public double TargetAwarenessRate { get { return h_trg; } set { h_trg = value; } }

        protected Candidate[] candidates;

        private Candidate currentCandidate;

        protected int updatedNum = 0;//whiteをもらったら++, Blackをもらったら--

        private object candidateLock = new Object();

    #endregion private メンバ

        public AAT(double h): base()
        {
            TargetAwarenessRate = h;
        }

        public AAT() : base() { }

        //コンストラクト時に呼ばれる
        public override void Initialize()
        {
            base.Initialize();

            //h_trgは多分変わってないだろう

            candidates = null;

            currentCandidate = null;

            updatedNum = 0;

        }

        //各ラウンド毎にエージェントの状態を初期化
        public override void RoundInit()
        {
            base.RoundInit();

            updatedNum = 0;
            //candidate は初期化しない

        }

    #region 各種プロパティ

        public int UpdatedNum
        {
            get
            {
                return updatedNum;
            }
        }

        public IEnumerable<Candidate> Candidates 
        {
            get
            {
                return candidates;
            }
        }

        public Candidate CurrentCandidate
        {
            get
            {
                return currentCandidate;
            }
            set
            {
                if (value == null)
                {
                    currentCandidate = null;
                    ImportanceLevel = 0.5;
                }
                else
                {
                    currentCandidate = value;
                    ImportanceLevel = currentCandidate.ImportanceLevel;
                }
            }
        }

        public object CandidateLock { get { return candidateLock; } }


    #endregion 各種プロパティ

        public override void UpdateOpinion(BlackWhiteSubject sub, double importanceLevel)
        {
            base.UpdateOpinion(sub, importanceLevel);
            ////Console.WriteLine("AAT: UpdateOpinion()");
            //意見を変える際に、カウント
            if (sub == BlackWhiteSubject.White)
            {
                updatedNum++;
            }
            else //if(sub == BlackWhiteSubject.Black)
            {
                updatedNum--;
            }
        }

        // step 1 of AAT　プログラムの最初に呼び出す 馬鹿正直．まだまだ最適化できる．
        public virtual IEnumerable<Candidate> BuildCandidates()
        {
            lock (CandidateLock)
            {
                //ご近所の数
                int dn = base.Neighbours.Count();

                //ご近所の数の２倍だけ作成
                candidates = new Candidate[2 * dn];

                double sigmaRight = Sigma;
                double sigmaLeft = 1 - Sigma;

                double impLvl = 0.5;
                // Jumpnum = 3, 2, 1と調べていく．
                for (int l = dn; l >= 0; l--)
                {
                    //importance levelを上げていって，beliefがしきい値を超えたときその値を使う．
                    for (; impLvl <= 1; impLvl += 0.001)
                    {
                        if (updateFunc(l, PriorBelief, impLvl) > sigmaRight)
                        {
                            Candidate newcan = new Candidate(l, impLvl);

                            //この重要視度で、逆側に行くときは、何ステップでいけるのか？
                            double tmpBelief = PriorBelief;
                            int count = 0;
                            do
                            {
                                tmpBelief = updateFunc(tmpBelief, 1 - impLvl);//逆では1-impLvl
                                count++;

                            } while (!(tmpBelief <= 1 - Sigma));

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
                        if (updateFunc(l, PriorBelief, 1 - impLvl) < sigmaLeft)
                        {
                            //重要視度を決定
                            Candidate newcan = new Candidate(-l, impLvl);

                            //この重要視度で、逆側に行くときは、何ステップでいけるのか？
                            double tmpBelief = PriorBelief;
                            int count = 0;
                            do
                            {
                                tmpBelief = updateFunc(tmpBelief, impLvl);//1-impLvlの逆なのでimpLvl
                                count++;

                            } while (!(tmpBelief >= Sigma));//0.8を超えたら

                            newcan.OtherJumpNum = count;


                            candidates[l - 1 + dn] = newcan;

                            break;
                        }
                    }
                }


            }
            return candidates;
        }

       


        //ラウンドが終わったら、step2,3を実行
        virtual public void RoundFinished()
        {
            //AATを実行

            //AATアルゴリズム
            if (updatedNum != 0)//意見を受け取っていない(に等しい)場合は何もしない
                //本当にそれでいいのか！？！！！！！！！！！！！
            {
                //第２ステップ
                this.EstimateAwarenessRate();


                //第３ステップ
                Candidate theBest = this.SelectTheBest();

                CurrentCandidate = theBest;

            }

            //状態の変更を通知
            if(agentChanged != null){
                agentChanged(this);
            }

            //candidate以外を初期化
            RoundInit();
        }


        // step 2 各候補に対して、awarenessRateを推定
        public virtual void EstimateAwarenessRate()
        {
            foreach (var can in candidates)
            {
                //この候補は意見形成できるか？
                bool formed = OpinionFormed(can);

                //推定値を更新
                can.EstimateAwarenessRate(formed);

            }
        }


            // step2で使われる。このimportance levelは意見形成できるかどうか
            public virtual bool OpinionFormed(Candidate can)
            {
                //3つの条件の論理式

                //意見を決められたか
                bool determined = Opinion != null;

                //現在のより大きいか？
                bool bigger = can.ImportanceLevel >= ImportanceLevel;

                //この候補にとって十分な意見が来たか？
                bool enoughUpd = updatedNum > 0 ?
                         updatedNum >= can.JumpNumRight: //正だったらrightと比べる
                        -updatedNum >= can.JumpNumLeft; //負だったらleftと比べる

                //意見を決められていて、候補の方が大きいか、　もしくは、十分な意見が来たか。
                return (determined && bigger) || enoughUpd;
            }

        // step 3
        public Candidate SelectTheBest()
        {
            Candidate[] sortedCandidates = new Candidate[candidates.Count()];

            //昇順に並び替える
            IEnumerable<Candidate> sorted = candidates.OrderBy((el) => el.ImportanceLevel);
            for (int i = 0; i < candidates.Length; i++)
            {
                sortedCandidates[i] = sorted.ElementAt(i);
            }

            {//選ばれないときがあるから注意！！！

                //最初（currentCandidateが一度も選ばれてないとき）は適当に選択
                if (currentCandidate == null)
                {//最初のみ。
                    return sortedCandidates.Last();//おっきいやつを選ぶ;
                }

                //現在使っている候補
                int i = Array.IndexOf(sortedCandidates, currentCandidate);

                //Debug.Assert(sortedCandidates[i] == currentCandidate);

                if (i == -1)
                {
                    i = 0;
                }


                //小さければ一個すすめる
                if (sortedCandidates[i].AwarenessRate < h_trg)
                {
                    i++;
                }
                //一個下がh_trg以上ならば戻る．
                else if ((i > 0) && sortedCandidates[i - 1].AwarenessRate > h_trg)
                {
                    i--;
                }
                else
                {
                    ;//現状維持 
                }

                if (i < 0)
                    i = 0;
                if (i >= sortedCandidates.Length)
                    i = sortedCandidates.Length - 1;

                return sortedCandidates[i];
            } // */

        }

        //文字列化
        public override string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (format == "METER")
            {
                return string.Format("[Agent ID:{0:000}] {1} {2} {3}",
                    ID, thought.MeterStr, thought.ToString("VAL"), currentCandidate);
            }
            else
            {
                return base.ToString(format, formatProvider);
            }
        }
    }
}
