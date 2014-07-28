using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt
{
    /*
    public class AATBasedAgentIO :BeliefBasedAgentIO, IAATBasedAgent
    {
        public override AgentAlgorithm Algorithm
        {
            set
            {
                if (!(value is IAATBasedAgent))
                {
                    throw new Exception("互換性がありません");
                }

                base.Algorithm = value;
            }

        }


        /*
        protected override void AddEvents()
        {
            base.AddEvents();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();

        }
        * /
        
        public IAATBasedAgent AAT 
        {
            get
            {
                return algorithm as IAATBasedAgent;
            }
        }


        public event EventHandler<CandidateEventArgs> CandidateChanged;
        public event EventHandler<EstimationEventArgs> EstimationChanged;


        public CandidateUpdaterSelector CandidateSelector
        {
            get
            {
                return (algorithm as IAATBasedAgent).CandidateSelector;
            }
            set
            {
                (algorithm as IAATBasedAgent).CandidateSelector = value;
            }
        }

        public IThought Thought
        {
            get
            {
                return (algorithm as IAATBasedAgent).Thought;

            }
            set
            {
                (algorithm as IAATBasedAgent).Thought = value;
            }
        }


        public System.Collections.Generic.IEnumerable<Candidate> Candidates
        {
            get
            {
                return AAT.Candidates;
            }
        }

        public Candidate CurrentCandidate
        {
            get
            {
                return AAT.CurrentCandidate;
            }
        }

        public Object CandidateLock
        {
            get
            {
                return AAT.CandidateLock;
            }
        }

        public double TargetAwarenessRate
        {
            get
            {
                return AAT.TargetAwarenessRate;
            }
            set
            {
                AAT.TargetAwarenessRate = value;
            }
        }


        public override double Belief
        {
            get
            {
                return AAT.Belief;
            }
        }

        public override double ImportanceLevel
        {
            get
            {
                return AAT.ImportanceLevel;
            }
        }

        public override double PriorBelief
        {
            get
            {
                return AAT.PriorBelief;
            }
        }

        public override double Sigma
        {
            get
            {
                return AAT.Sigma;
            }
        }

        public override string ToString()
        {
            if (algorithm == null)
            {
                return "empty AgentIO";
            }
            
            return algorithm.ToString();
        }

        //ホントはこんなところに書きたくないんだけどん．
        public Algorithm.AgentSpec publishSpec()
        {
            return (Algorithm as IAATBasedAgent).publishSpec();
        }
    }
    */
}
