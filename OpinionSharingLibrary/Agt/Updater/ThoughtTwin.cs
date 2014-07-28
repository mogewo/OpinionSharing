using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;

namespace OpinionSharing.Agt.Updater
{
    class ThoughtTwin : IThought
    {
        const int N = 3;
        Thought[] thoughts = new Thought[N];

        int currentThoughtIndex = 0;

        double priorBelief;

        public ThoughtTwin(double prior)
        {
            priorBelief = prior;

            for (int i = 0; i < N; i++)
            {
                thoughts[i] = new Thought(priorBelief);
                thoughts[i].OpinionChanged += ThoughtTwin_OpinionChanged;
            }
        }

        public void Initialize(){
            foreach (var thought in thoughts)
            {
                thought.Initialize();
            }
        }

        void ThoughtTwin_OpinionChanged(object sender, OpinionEventArgs e)
        {
            ThoughtFlip();
            CurrentThought.Belief = CurrentThought.PriorBelief;

            if (e.Opinion != this.Opinion)
            {
                this.OpinionChanged(sender,e);
            }
        }

        public event EventHandler<global::OpinionSharing.Agt.BeliefEventArgs> BeliefChanged;
        
        public event EventHandler<global::OpinionSharing.Agt.OpinionEventArgs> OpinionChanged;

        public double Belief
        {
            get
            {
                return CurrentThought.Belief;
            }
            set
            {
                CurrentThought.Belief = value;
            }
        }

        public BlackWhiteSubject? Opinion
        {
            get
            {
                int b = 0;
                foreach (var thought in thoughts)
                {
                    if (thought.Opinion == BlackWhiteSubject.White)
                    {
                        b++;
                    }
                    else if (thought.Opinion == BlackWhiteSubject.Black)
                    {
                        b--;
                    }
                }


                if (b > 0)
                {
                    return BlackWhiteSubject.White;
                }

                else if (b < 0)
                {
                    return BlackWhiteSubject.Black;
                }

                else
                {
                    return CurrentThought.Opinion;
                }

            }
        }

        public double PriorBelief
        {
            get
            {
                return CurrentThought.PriorBelief;
            }
        }

        public double SigmaLeft
        {
            get
            {
                return CurrentThought.SigmaRight;
            }
        }
        public double SigmaRight
        {
            get
            {
                return CurrentThought.SigmaRight;
            }
        }



        public void ThoughtFlip()
        {
            currentThoughtIndex = (currentThoughtIndex + 1) % N;
        }

        public Thought CurrentThought
        {
            get
            {
                return thoughts[currentThoughtIndex];
            }
            private set
            {
                thoughts[currentThoughtIndex] = value;
            }
        }

        public string MeterStr
        {
            get
            {
                return CurrentThought.MeterStr;
            }
        }
    }
}
