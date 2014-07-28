using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;
using OpinionSharing.Util;

namespace OpinionSharing.Agt
{
    public class NoMoreBelief : AAT
    {
        public NoMoreBelief(double h_trg)
            : base(h_trg)
        {
        }

        public NoMoreBelief()
            : base()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void UpdateOpinion(BlackWhiteSubject sub, double importanceLevel)
        {
            //自分の意見と同じ意見が来たら
            if (sub == BlackWhiteSubject.White && Belief >= Sigma ||
                sub == BlackWhiteSubject.Black && Belief <= 1 - Sigma)
            {
                base.UpdateOpinion(sub, 0.5); //重要視度0で更新する。
            }
            else
            {
                base.UpdateOpinion(sub, importanceLevel);
            }

        }
    }
}
