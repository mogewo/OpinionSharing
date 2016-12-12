using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OpinionSharing.Subject;
using GraphTheory.Net;
using OpinionSharing.Agt;

namespace OpinionSharing.Agt
{
    //値オブジェクトということにしてみるか。immutable.
    public class BeliefUpdater
    {
        public readonly double ImportanceLevel = 0.55;//!!!!!だめ！！

        //8-6追記　重みのために追記
        public BeliefUpdater(double v = 0.55)
        {
            ImportanceLevel = v;
            //ew = 1.0;
            
        }

        public virtual double updateBelief(BlackWhiteSubject sbj, double belief)
        {
            double cupd = decideCupd(sbj, ImportanceLevel);

            return updateFunc(belief, cupd);
        }

        //受け取った意見とimportancelevelからCupdを決める
        //AAT ImportanceLevelというclassを使ってもいいかもね。
        public static double decideCupd(BlackWhiteSubject sbj, double importanceLevel)
        {
            return sbj == BlackWhiteSubject.White ? importanceLevel : 1 - importanceLevel;

            /*
             * 
            if(BlackWhiteSubject.White == true)
              retur  importanceLevel;
            else
              return 1- i;
             */
        }


        //更新式
        public static double updateFunc(double currentBelief, double Cupd)
        {
   
            if (!(0 <= currentBelief && currentBelief <= 1)) //belief <- [0,1]
            {
                throw new Exception("異常な信念値" + currentBelief);
            }

            if (!(0 < Cupd && Cupd < 1)) //Cupd <- [0.5,1)
            {
                throw new Exception("異常な重要視度" + Cupd);
            }

            double numerator = (currentBelief * Cupd);
            double denominator = ((1 - Cupd) * (1 - currentBelief) + Cupd * currentBelief);

            //denominator = 0になる条件
            //(Cupd == 1 || currentBelief == 1) && (Cupd == 0 || currentBelief == 0)
            //= {Cupd == 1 && currentBelief == 0 } || {Cupd == 0 && currentBelief == 1 }
            //つまり、端っこからどかーんと行こうとすると落ちる
            //ImportanceLevel== 1はやっぱ禁止やな。

            //重み更新　8-6
            //if (BeliefUpdater.agt != null)
            //{
            //    INode neighbor = BeliefUpdater.mes.From as INode;//意見受け取り元
            //    //null対策
            //    if (neighbor != null)
            //    {
            //        ew = BeliefUpdater.agt.Edgeweights[neighbor.ID];//重み
            //        //ew = BeliefUpdater.agt.getEdgeWeight(BeliefUpdater.mes);
            //    }
            //}

            

           

            double ret = (numerator / denominator);//この更新式に重みをかける?

            if (ret == 1.0)
            {
                ret = 1;
            }

            return ret;
        }

        //n回更新
        //ImportanceLevel
        public static double updateFunc(double currentBelief, double Cupd, int n)
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

        //double型に/から変換するやつを実装
    }
}
