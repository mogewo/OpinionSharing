using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt.Algorithm
{
    public class AgentSpec
    {
        public readonly double PriorBelief;
        public readonly CandidateUpdaterSelector CandidateSelector;

        public AgentSpec(double prior, CandidateUpdaterSelector selector)
        {
            PriorBelief = prior;
            CandidateSelector = selector;
        }
    }
}
