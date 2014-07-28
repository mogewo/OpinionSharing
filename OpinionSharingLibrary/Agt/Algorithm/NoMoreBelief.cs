using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;
using MyRandom;

namespace OpinionSharing.Agt
{
    public class LimitedBelief : AAT
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void UpdateOpinion(BlackWhiteSubject sub, BeliefUpdater updater)
        {
            //自分の意見と同じ意見が来たら
            if (sub == BlackWhiteSubject.White && Belief >= Sigma ||
                sub == BlackWhiteSubject.Black && Belief <= 1 - Sigma)
            {
                base.UpdateOpinion(sub, new BeliefUpdater( 0.5)); //重要視度0で更新する。
            }
            else
            {
                base.UpdateOpinion(sub, updater);
            }

        }
    }
}
