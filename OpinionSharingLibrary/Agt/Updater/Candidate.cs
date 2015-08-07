﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;



namespace OpinionSharing.Agt
{
    public class Candidate
    {
        #region privateメンバ
        private int jumpNum;
        private int otherJumpNum;
        private BeliefUpdater beliefUpdater;// ここからImportanceLevelなどが派生する
        private double awarenessRate;
        private List<bool> determinedRound;
        //private AgentIO agt;    //重みのためのエージェント
        //private BWMessage mes;  //重みのためのメッセージ
        //private INode node;
        #endregion privateメンバ

        #region プロパティ
        public Candidate(int JN, double IL, double AR = 0)
        {
            jumpNum = JN;
            beliefUpdater = new BeliefUpdater(IL);
            awarenessRate = AR;
            determinedRound = new List<bool>();
        }

        public int JumpNum { get { return jumpNum; } }          // ...-3 -2 -1  +1 +2 +3...
        public int OtherJumpNum { get { return otherJumpNum; } set { otherJumpNum = value; } }// ... 2  3  4  -4 -3 -2...

        
        public int JumpNumRight
        {
            get
            {
                if (jumpNum > 0)
                {
                    return jumpNum;
                }
                else
                {
                    return otherJumpNum;
                }
            }
        }

        public int JumpNumLeft
        {
            get
            {
                if (jumpNum < 0)
                {
                    return -jumpNum;
                }
                else
                {
                    return otherJumpNum;
                }
            }
        }

        public int JumpNumMin
        {
            get
            {
                return Math.Min(JumpNumLeft, JumpNumRight);
            }
        }

        //public AgentIO Agt
        //{
        //    get
        //    {
        //        return agt;
        //    }
        //    set
        //    {
        //        agt = value;
        //    }
        //}

        //public BWMessage Mes
        //{
        //    get
        //    {
        //        return mes;
        //    }
        //    set
        //    {
        //        mes = value;
        //    }
        //}

        //public INode Node
        //{
        //    get
        //    {
        //        return node;
        //    }
        //    set
        //    {
        //        node = value;
        //    }
        //}
        #endregion プロパティ

        public int RequiredUpdateNum(int direction) //Right: 1 , Left: -1
        {
            Debug.Assert(direction == 1 || direction == -1, "direction should be 1 or -1");

            if (direction == 1)     //右にいく
            {
                return JumpNumRight;
            }
            else    //左にいく
            {
                return JumpNumLeft;
            }
        }

        public BeliefUpdater BeliefUpdater { get { return beliefUpdater; } }
        public double ImportanceLevel { get { return beliefUpdater. ImportanceLevel; }}
        public double AwarenessRate { get { return awarenessRate; } }


        //新しい意見形成を得て、推定し直す
        public void EstimateAwarenessRate(bool determined)
        {
//            Console.Write("estimate awareness rate to {0}",ImportanceLevel);
            //double ew = 1.0;
            determinedRound.Add(determined);

            //意見を決められた数を求める
            int determinedNum = 0;
            foreach (var round in determinedRound)
            {
                if (round == true)
                {
                    determinedNum++;
                }
            }

            //if (BeliefUpdater.Agt != null)
            //{
            //    INode neighbor = BeliefUpdater.Mes.From as INode;//意見受け取り元
            //    //null対策
            //    if (neighbor != null)
            //    {
            //        ew = beliefUpdater.Node.Edgeweights[neighbor.ID];//重み
            //        //ew = BeliefUpdater.agt.getEdgeWeight(BeliefUpdater.mes);
            //    }
            //}

            awarenessRate = (double)determinedNum / determinedRound.Count;

        }

        public override string ToString()
        {
            return string.Format("[Candidate JmpLeft:{0,6:d}, JmpRight:{3,6:d}, IptLvl:{1,6:f2}, AwrsRt:{2,6:f2}]"
                ,JumpNumLeft, ImportanceLevel, AwarenessRate, JumpNumRight);

        }
    }
}
