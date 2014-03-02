using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt
{
    public interface IAATBasedAgent : IBeliefBasedAgent 
    {
        event EventHandler<CandidateEventArgs> CandidateChanged;
        event EventHandler<EstimationEventArgs> EstimationChanged;

        System.Collections.Generic.IEnumerable<Candidate> Candidates { get; }

        Candidate CurrentCandidate { get; }
        Object CandidateLock { get; }

        CandidateUpdaterSelector CandidateSelector { get; set; }
        IThought Thought { get; set; }


        double TargetAwarenessRate { get; set; }



        Algorithm.AgentSpec publishSpec();
    }
}
