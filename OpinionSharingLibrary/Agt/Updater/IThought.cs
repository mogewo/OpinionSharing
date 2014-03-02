using System;
namespace OpinionSharing.Agt
{
    public interface IThought
    {
        void Initialize();

        event EventHandler<global::OpinionSharing.Agt.BeliefEventArgs> BeliefChanged;
        event EventHandler<global::OpinionSharing.Agt.OpinionEventArgs> OpinionChanged;

        double Belief { get; set; }
        global::OpinionSharing.Subject.BlackWhiteSubject? Opinion { get; }

        double PriorBelief { get; }
        double SigmaLeft { get; }
        double SigmaRight { get; }


        string MeterStr { get; }
    }
}
