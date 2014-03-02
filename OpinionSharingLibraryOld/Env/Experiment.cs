using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

using Log;


namespace OpinionSharing.Env
{
    using Net;
    using Agt;
    using Util;
    using Subject;


    public class ExpResult
    {
        public readonly ExpAccuracy Accuracy;
        public readonly ExpAgentsParam AgentsParam;

        public ExpResult(ExpAccuracy acc, ExpAgentsParam a){
            Accuracy = acc;
            AgentsParam = a;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ",Accuracy, AgentsParam);
        }
    }

    public class ExpAgentsParam
    {
        public readonly double ImportanceLevel;
        public readonly double AwarenessRate;

        public ExpAgentsParam(double t, double h)
        {
            ImportanceLevel = t;
            AwarenessRate = h;
        }

        public override string ToString()
        {
            return string.Format("ImLv: {0} AwRt: {1} ",ImportanceLevel,AwarenessRate);
        }
    }
    public class ExpAccuracy
    {
        public readonly double Correct;
        public readonly double Incorrect;
        public readonly double Undeter;

        public ExpAccuracy(double c, double i, double u)
        {
            Correct = c;
            Incorrect = i;
            Undeter = u;
        }

        public override string ToString()
        {
            return string.Format("correct: {0} incorrect: {1} undeter: {2} ", Correct, Incorrect, Undeter);
        }

    }

    public class Experiment
    {
        public OSEnvironment Environment    { get; set; }
        public int RoundNum         { get; set; }
        public int StepNum          { get; set; }
        public double SensorRate    { get; set; }

        public int Round    { get; private set; }
        public int Step     { get; private set; }
        

        public BlackWhiteSubject? Fact{get;set;}


        public Experiment(OSEnvironment e, int rn = 150, int sn = 3000, double sr = 0.1)
        {
            RoundNum = rn;
            StepNum = sn;
            SensorRate = sr;

            Environment = e;

            Initialize();
        }

        public void Initialize()
        {
            Environment.Initialize();

            Round = 0;
            Step = 0;

            //BuildCandidates() 必要か
        }

        public void BuildCandidates()
        {
            //AAT 1st stepを実行
            foreach (AAT aat in Environment.Network.Nodes)
            {
                aat.BuildCandidates();
            }
        }
        
        public ExpAccuracy Run()//お手本
        {
            var roundAccLog = L.g("roundacc"); 


            Initialize();

            //1st step
            BuildCandidates();

            ////////////評価開始:正確性を求める////////////

            double correctRound = 0;
            double incorrectRound = 0;
            double undeterRound = 0;

            roundAccLog.WriteLine("<LEARNING> RoundNum: " + RoundNum);

            //LEARNING
            while (!this.LearningIsFinished)
            {
                ///////////////////////////////ラウンドを実行///////////////////////////////

                // このラウンドでの事実bを決める 
                Environment.TheFact.randomNext();

                //ラウンドを実行
                var roundResult = ExecRound();

                ////////////////////////////////////////////////////////////////////// 

                roundAccLog.WriteLine( 
                    string.Format("Round: {0} fact: {1} {2}",
                        Round, 
                        Environment.TheFact.Value, 
                        roundResult ));

            }

            roundAccLog.WriteLine("***EVALUATION***");

            //EVALUATION
            while (!this.EvaluationIsFinished)
            {
                ///////////////////////////////ラウンドを実行///////////////////////////////
                // このラウンドでの事実bを決める 
                Environment.TheFact.randomNext();

                //ラウンドを実行
                var roundResult = ExecRound(false);//学習しないラウンドを実行

                ////////////////////////////////////////////////////////////////////// 

                correctRound   += roundResult.Accuracy.Correct;
                incorrectRound += roundResult.Accuracy.Incorrect;
                undeterRound   += roundResult.Accuracy.Undeter;
            

                //ラウンドを実行して，そのラウンドでの正確性を求める
                roundAccLog.WriteLine( 
                    string.Format("Round: {0} fact: {1} {2}",
                        Round, 
                        Environment.TheFact.Value, 
                        roundResult ));

            }

            //すべてのラウンドの平均
            double correctRoundRate   =   correctRound / RoundNum; // = accuracy
            double incorrectRoundRate = incorrectRound / RoundNum;
            double undeterRoundRate   =   undeterRound / RoundNum;


            var res = new ExpAccuracy(correctRoundRate, incorrectRoundRate, undeterRoundRate);

            L.g("roundacc,console").WriteLine(res.ToString());

            return res;
        }

        public Boolean LearningIsFinished
        {
            get
            {
                return Round >= RoundNum;
            }
        }

        public Boolean EvaluationIsFinished
        {
            get
            {
                return Round >= RoundNum * 2;
            }
        }

        public double ProgressRate
        {
            get
            {
                return (double)Round / RoundNum;
            }
        }

        //ラウンドを実行し，そのラウンドの正解率を求める
        public ExpResult ExecRound(bool learning = true)
        {
            // ラウンド開始 

            StartRound();

            ManySteps(SensorRate);

                // 実行結果:Accuracyを受け取る 
                ExpAccuracy res = this.EnvAccuracy;

                // 実行結果：ImportanceLevelを受け取る
                ExpAgentsParam aveIL = calcAverageImportanceLevel();


                if (learning)
                {
                    // ラウンド終了。エージェントは学習し、初期化される。
                    FinishRound();
                }
                else
                {
                    // ラウンド終了。エージェントは学習せず、初期化される。
                    CancelRound();
                }

            return new ExpResult( res, aveIL );
        }

        private ExpAgentsParam calcAverageImportanceLevel()
        {
            double ts = 0,hs = 0;

            foreach (AAT aat in Environment.Network.Nodes)
            {
                ts += aat.ImportanceLevel;

                if (aat.CurrentCandidate != null)
                {
                    hs += aat.CurrentCandidate.AwarenessRate;
                }
            }

            ts /= Environment.Network.Nodes.Count();
            hs /= Environment.Network.Nodes.Count();

            return new ExpAgentsParam(ts, hs);
        }

        private void ManySteps(double sensorPercent)
        {
            while(!this.IsRoundFinished)
            {
                ExecStep(sensorPercent);//センサーに値が行き、エージェントは聴き、考える
            }
        }

        public void StartRound(int? seed = null)
        {
            //ラウンドの数字を進める
            Round++;

            //ステップははじめにもどす
            Step = 0;

            //何も渡されなければ、ラウンドIDをシードにする。
            if (seed == null)
            {
                seed = Round;
            }

            RandomPool.Declare("sensor", seed.Value);
        }

        public bool IsRoundFinished
        {
            get
            {
                return Step >= StepNum;
            }
        }


        //ステップ センサーに値が行き、エージェントは聴き、考える。
        public void ExecStep(double sensorPercent) // Step
        {
            Step++;

            //センサーエージェントは観測
            SensorObservation();


            //意見を受け取ったエージェントは、一度耳を閉じて
            Listen();


            //聞いたことを考える
            Think();

            //現在の環境の状態を表示
        #region SHOW

            if (Round >= 150 && Round < 153)
            {
                double aveBelief = 0;
                int CorrectNum = 0;
                int UndeterNum = 0;
                foreach (AAT aat in Environment.Network.Nodes)
                {
                    aveBelief += aat.Belief;
                    CorrectNum += aat.Opinion == Environment.TheFact.Value ? 1 : 0 ;
                    UndeterNum += aat.Opinion == null ? 1 : 0 ;
                }
                int N = Environment.Network.Nodes.Count();

                double CorrectRate = (double)CorrectNum / N;
                double UndeterRate = (double)UndeterNum / N;
                double IncorrectRate = 1.0 - CorrectRate - UndeterRate;

                aveBelief /= N;

                L.g("stepbelief").WriteLine(
                    string.Format("step: {0} belief: {1} correct: {2} incorrect: {3} undeter: {4}",
                    Step,aveBelief,CorrectRate,IncorrectRate,UndeterRate));
            }

        #endregion SHOW

        }

        //センサーエージェントは、センサーの値を観測
        private void SensorObservation()
        {
            //*** 10%のセンサーが新しい値を取得する．***

            int sensorNum = Environment.Sensors.Count;

            int observationNum = (int)(Math.Ceiling(sensorNum * 0.1));

            var indexes = RandomPool.Get("sensor").getRandomIndexes(sensorNum, observationNum);

            //L.g("sensor").Write("");
            foreach (int index in indexes)
            {
                Sensor s = Environment.Sensors.ElementAt(index);
                var data = s.newData();

             //   L.g("sensor").Write("s->" + s.Agent.ID + " " +  ( data == BlackWhiteSubject.White ? 1 : -1 ) + ", ");
            }
            //L.g("sensor").WriteLine("");
        }

        //それぞれのエージェントは、聴く。
        private void Listen()
        {
            //各エージェントは受け取った値を解釈する。

            /**
            Parallel.ForEach(Environment.Network.Nodes, (node) =>
            {
                (node as Agent).Listen();
            });

            //*/

            /* */
            foreach (Agent agent in Environment.Network.Nodes)
            {
                agent.Listen();
            }
            //*/
        }

        //それぞれのエージェントが、考える。意見をいうかもしれない。
        private void Think()
        {
            /* *
            Parallel.ForEach(Environment.Network.Nodes, (node) =>
            {
                (node as Agent).Think();
            });
            //*/

            /* */
            //各エージェントは受け取った値を解釈する。
            foreach (Agent agent in Environment.Network.Nodes)
            {
                agent.Think();
            }
            //*/
        }

        //一連のステップの最後のエージェントの状況を見て、正解率を計算
        private ExpAccuracy EnvAccuracy
        {
            get
            {
                return Environment.EnvAccuracy;
            }
        }

        //
        public void FinishRound()
        {
            //ラウンドが終了 -> AATアルゴリズムを実行，エージェントを初期化
            foreach (AAT a in Environment.Network.Nodes)
            {
                a.RoundFinished();
            }
        }

        public void CancelRound()
        {
            //ラウンドが終了 -> AATアルゴリズムを実行，エージェントを初期化
            foreach (AAT a in Environment.Network.Nodes)
            {
                a.RoundInit();
            }
        }

    }
}
