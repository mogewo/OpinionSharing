using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;


namespace OpinionSharing.Agt
{
    public class ThoughtStack:IThought
    {
    #region private変数

        //考えを複数持っている
        Stack<Thought> thoughts = new Stack<Thought>();


        //現在スタックの一番上にある考え
        public Thought CurrentThought
        {
            get
            {
                return thoughts.Peek();
            }
        }

        //初期信念
        public double PriorBelief
        {
            get
            {
                return CurrentThought.PriorBelief;
            }
        }

        private object ThoughtsLock = new Object();

    #endregion private変数

        //コンストラクタ
        public ThoughtStack(double prior)
        {
            thoughts.Push(new Thought(prior));

            Initialize();
        }

        public void Initialize()
        {
            double prior = PriorBelief;
            thoughts.Clear();
            thoughts.Push(new Thought(prior));

            //現在の考えが、意見を形成したら

            AddEvents();
        }

        //現在の考えが意見を形成したら。
        private void CurrentThought_OpinionChanged(object sender, OpinionEventArgs e)
        {
            if (thoughts.Count < 2 || ( thoughts.Count >= 2 && Opinion.Value != e.Opinion) )
            {
                //自らのイベントを発火
                OnOpinionChanged(e);
            }
            

            //順番大切！
            AddThought();
        }

        private void CurrentThought_BeliefChanged(object sender, BeliefEventArgs e)
        {
            OnBeliefChanged(e);
        }

        //考えを積み上げる
        //これをやると、新しい考えが積み重なってしまうので注意
        public void AddThought()
        {
            //現在の考えは現在の考えでなくなるのでイベントを削除
            RemoveEvents();

            lock (ThoughtsLock)
            {
                //スタックに新しい考えをプッシュ
                thoughts.Push(new Thought(CurrentThought.PriorBelief));
            }

            //新しいのが現在の考えになる。
            AddEvents();
        }

        private void AddEvents()
        {
            CurrentThought.OpinionChanged += CurrentThought_OpinionChanged;
            CurrentThought.BeliefChanged += CurrentThought_BeliefChanged;
        }

        private void RemoveEvents()
        {
            CurrentThought.OpinionChanged -= CurrentThought_OpinionChanged;
            CurrentThought.BeliefChanged -= CurrentThought_BeliefChanged;
        }

        public BlackWhiteSubject? Opinion
        {
            get
            {
                lock (ThoughtsLock)
                {
                    int white = 0;
                    int black = 0;

                    foreach (var thought in thoughts)
                    {
                        if (thought.Opinion == BlackWhiteSubject.White)
                        {
                            white++;
                        }
                        else if (thought.Opinion == BlackWhiteSubject.Black)
                        {
                            black++;
                        }
                    }

                    if (white > black)
                    {
                        return BlackWhiteSubject.White;
                    }
                    else if (white < black)
                    {
                        return BlackWhiteSubject.Black;
                    }
                    else
                    {
                        //最新の意見を、自分の意見とする。
                        foreach (var thought in thoughts)
                        {
                            if (thought.Opinion != null)
                            {
                                return thought.Opinion;
                            }
                        }

                        return null;
                    }
                }
            }
        }

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

        public double SigmaRight
        {
            get
            {
                return CurrentThought.SigmaRight;
            }
        }

        public double SigmaLeft
        {
            get
            {
                return CurrentThought.SigmaLeft;
            }
        }


    #region イベント

        public event EventHandler<BeliefEventArgs> BeliefChanged;

        public event EventHandler<OpinionEventArgs> OpinionChanged;

        protected virtual void OnBeliefChanged(BeliefEventArgs e){
            EventHandler<BeliefEventArgs> handler = BeliefChanged;

            if (handler != null)
            {
                handler(this, e);
            }

        }

        protected virtual void OnOpinionChanged(OpinionEventArgs e){
            EventHandler<OpinionEventArgs> handler = OpinionChanged;

            if (handler != null)
            {
                handler(this, e);
            }

        }

    #endregion イベント

        public string MeterStr
        {
            get
            {
                string buf = "";
                int length = 20;
                for (int j = 0; j < length; j++)
                {
                    if (Math.Floor(Belief * length) == j)
                    {
                        buf += "*";
                    }
                    else if (Math.Floor(SigmaRight * length) == j || Math.Floor((1 - SigmaRight) * length) == j)
                    {
                        buf += "|";
                    }
                    else if (Math.Floor(PriorBelief * length) == j)
                    {
                        buf += "+";
                    }
                    else
                    {
                        buf += "-";
                    }
                }
                return buf;
            }
        }
    }
}
