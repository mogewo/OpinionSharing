using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpinionSharing.Subject;
using MyRandom;

namespace OpinionSharing.Agt
{
    public delegate void StateChangedHandler(Thought t);

    public class Thought : IFormattable, IThought
    {
    #region 定数 1つ。
        private const double sigma = 0.8;
    #endregion

    #region privateメンバ 3つ。

        //意見
        private BlackWhiteSubject? opinion = null;//nullable型 初めて使うぜ！nullならundeter

        //信念の初期値 (コンストラクタで初期化する)
        private readonly double priorBelief;

        //信念
        private double belief;


    #endregion

    #region コンストラクタ&初期化関数 


        ////////////引数なしコンストラクタ//////////

        //指定された初期値で初期化
        public Thought(double prior)
        {
            priorBelief = prior;//最初の信念は、このオブジェクトの寿命の間変わらない。


            priorBelief = Math.Min(sigma, Math.Max(1 - sigma, prior));

            Initialize();
        }


        //コンストラクタ呼び出し直後の状態に戻す。
        public void Initialize()
        {
            belief = priorBelief;

            opinion = null;
        }


    #endregion コンストラクタ&初期化関数 

    #region プロパティ

        public double SigmaRight
        {
            get
            {
                return sigma;
            }
        }

        public double SigmaLeft
        {
            get
            {
                return 1 - sigma;
            }
        }

        //連続値 -> 閾値によって、離散値を更新する。
        public double Belief
        {
            get
            {
                return belief;
            }
            set
            {
                belief = Math.Max(0, Math.Min(1, value)); //belief <- [0, 1]

                OnBeliefChanged(new BeliefEventArgs(belief));

                if (belief >= sigma)
                {
                    Opinion = BlackWhiteSubject.White;
                }
                else if (belief <= 1 - sigma)
                {
                    Opinion = BlackWhiteSubject.Black;
                }
            }
        }

        public double PriorBelief
        {
            get
            {
                return priorBelief;
            }
        }

        //離散値
        public BlackWhiteSubject? Opinion
        {
            get
            {
                return opinion;
            }

            private set
            {
                if( value != this.Opinion ){
                    opinion = value;
                    OnOpinionChanged(new OpinionEventArgs(opinion));
                }
            }
        }
    #endregion プロパティ

    #region イベント

        public event EventHandler<BeliefEventArgs> BeliefChanged;
        public event EventHandler<OpinionEventArgs> OpinionChanged;

        protected virtual void OnBeliefChanged(BeliefEventArgs e){
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<BeliefEventArgs> handler = BeliefChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnOpinionChanged(OpinionEventArgs e){

            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<OpinionEventArgs> handler = OpinionChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    #endregion イベント

        #region オーバーライド 便利メソッド
        public override string ToString()
        {
            return string.Format(
                "[Opinion prior:{2:0.000}, belief:{0:0.000}, value:{1}]",
                belief, opinion == null ? "unknown" : opinion.ToString(),PriorBelief);
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (format == "VAL")
            {
                return opinion == null ? 
                        "undet" : opinion.ToString();
            }
            else if (format == "BEL")
            {
                return belief.ToString();
            }
            else
            {
                return this.ToString();
            }
        }
        //called from ToString
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
    #endregion オーバーライド 便利メソッド
    }
}
