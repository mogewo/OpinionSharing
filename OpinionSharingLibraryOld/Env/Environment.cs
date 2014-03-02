using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Util;
using OpinionSharing.Agt;
using OpinionSharing.Subject;

namespace OpinionSharing.Env
{
    using Net;
    using Subject;


    public class OSEnvironment
    {
        public Network Network { get; private set; }
        public List<Sensor> Sensors { get; private set;}
        public TheFact TheFact { get; private set;}


        internal void Initialize()
        {
            //エージェントをコンストラクト直後の状態に戻す
            foreach(Agent a in Network.Nodes){
                a.Initialize();
            }
        }

        public OSEnvironment (NetworkGenerator generator, int sensorNum)
        {
            //事実
            TheFact = new TheFact(BlackWhiteSubject.White);

            //ネットワークをつくる
            this.Network = GenerateNetwork(generator);

            //センサーをもつエージェントをつくる
            PrepareSensor(sensorNum);
        }

        private Network GenerateNetwork(NetworkGenerator generator)
        {
            //中身:ネットワークとセンサー
            return generator.create();
        }


        private void PrepareSensor(int sensorNum)
        {
            //センサー
            Sensors = new List<Sensor>();

            //センサーをもつエージェントを決める
            var selectedIndexes = RandomPool.Get("setenv").getRandomIndexes(this.Network.Nodes.Count(), sensorNum);

            foreach (var i in selectedIndexes)
            {
                //センサーを生成
                Sensor s = new Sensor(TheFact);

                //センサーにエージェントをセット(内部でAgentのhavesensorをtrueにしてる．微妙に密結合)
                s.SetAgent((Agent)this.Network.Nodes.ElementAt(i));
                this.Sensors.Add(s);
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Network: \n" + Network + "\n");
            
            sb.Append("\nSensors: \n");
            foreach (var item in Sensors)
            {
                sb.Append("\t" + item + "\n");
            }

            sb.Append("\nTheFact: \n\t" + TheFact + "\n");

            return sb.ToString();
        }

        public ExpAccuracy EnvAccuracy
        {
            get
            {
                ///////////評価.正しかったエージェントの割合を計算///////////
                double CorrectNum = 0;//正解数
                double IncorrectNum = 0;


                foreach (Agent a in this.Network.Nodes)
                {
                    if (a.Opinion != null)
                    {
                        //エージェントの意見が事実と一致していたら
                        if (a.Opinion.Value == this.TheFact.Value)
                        {
                            //true
                            CorrectNum++;//正解数をインクリメント
                        }
                        else
                        {
                            //false
                            IncorrectNum++;
                        }
                    }
                    else
                    {
                        //undetermind
                    }
                }

                //正解率
                double CorrectRate = CorrectNum / Network.Nodes.Count();
                double IncorrectRate = IncorrectNum / Network.Nodes.Count();
                double UndeterRate = 1 - CorrectRate - IncorrectRate;//(env.Network.Nodes.Count() - CorrectNum - IncorrectNum) / env.Network.Nodes.Count();


                return new ExpAccuracy(CorrectRate, IncorrectRate, UndeterRate);
            }
        }

    }
}
