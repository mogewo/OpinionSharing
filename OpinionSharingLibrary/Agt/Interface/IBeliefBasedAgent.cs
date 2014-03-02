using System;
namespace OpinionSharing.Agt
{
    public interface IBeliefBasedAgent : IAgent
    {

        event EventHandler<BeliefEventArgs> BeliefChanged;

        double Belief { get; }

        double ImportanceLevel { get; }
        double PriorBelief { get; }
        double Sigma { get; }

    }
}
