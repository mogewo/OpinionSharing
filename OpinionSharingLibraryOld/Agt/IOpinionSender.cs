using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;

namespace OpinionSharing.Agt
{
    public interface IOpinionSender
    {
        double? Accuracy { get; }

        void SendOpinion(BlackWhiteSubject opinion, Agent to);
    }
}
