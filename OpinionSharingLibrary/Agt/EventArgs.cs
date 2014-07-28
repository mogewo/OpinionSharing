using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;


namespace OpinionSharing.Agt
{

    public class OpinionEventArgs : EventArgs
    {
        private BlackWhiteSubject? opinion;

        public OpinionEventArgs(BlackWhiteSubject? o)
        {
            opinion = o;
        }

        public BlackWhiteSubject? Opinion
        {
            get
            {
                return opinion;
            }
        }
    }


    public class BeliefEventArgs : EventArgs
    {
        private double belief;

        public BeliefEventArgs(double b)
        {
            belief = b;
        }

        public double Belief
        {
            get
            {
                return belief;
            }
        }
    }

    public class EstimationEventArgs : EventArgs
    {
        IEnumerable<Candidate> candidates;

        public EstimationEventArgs( IEnumerable<Candidate> cands)
        {
            candidates = cands;
        }

        public IEnumerable<Candidate> Candidates
        {
            get
            {
                return candidates; 
            }
        }
    }

    public class CandidateEventArgs : EventArgs
    {
        Candidate cand;

        public CandidateEventArgs(Candidate u)
        {
            cand = u;
        }

        public Candidate Candidate
        {
            get
            {
                return cand; 
            }
        }
    }


}
