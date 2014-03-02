using System;
namespace OpinionSharing.Agt
{
    //今は使ってない@08/31
    public interface IAgent
    {
        double Belief { get; }
        double ImportanceLevel { get; }
        OpinionSharing.Subject.BlackWhiteSubject? Opinion { get; }
        void RoundInit();
        void ReceiveOpinion(BWMessage message);
    }
}
