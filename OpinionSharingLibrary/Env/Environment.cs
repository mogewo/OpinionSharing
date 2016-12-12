using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Agt;
using OpinionSharing.Subject;

using MyRandom;
using GraphTheory.Net;

using GraphTheory;

namespace OpinionSharing.Env
{
    using Subject;


    public class OSEnvironment
    {
        //***************3つの組．***************
        //事実
        public TheFact TheFact { get; private set;}

        //エージェントのネットワーク
        public Network Network { get; private set; }

        //センサー
        public List<Sensor> Sensors { get; private set;}

        //信用度　2014/11/13
        public List<double> ImportanceLevel { get; private set;}


        public void disconnectSomething()
        {

            //消す奴をきめる
            //List<int> indexes = new InitableRandom.getRandomIndexes(Network.Links.Count,5);
            //var selectedDisconecctedIndexes = RandomPool.Get("disconnectedset").getRandomIndexes(this.Network.Links.Count(), 5);
            if (this.Network.Links == null)
            {
                Console.WriteLine("disconnectSomethingのNetworkはnullです");
            }
            else
            {
                var count = this.Network.Links.Count();
                if (count < 10)
                {
                    return;
                }
                List<int> dindexes = RandomPool.Get("WeightSet").getRandomIndexes(count, 1);

                List<Link> selectedDisLinks = new List<Link>();

                foreach (var i in dindexes)
                {
                    Link l = this.Network.Links.ElementAt(i);
                    selectedDisLinks.Add(l);
                }
                foreach (Link link in selectedDisLinks)
                {
                    Network.DisconnectNode(link);
                }
            }
            

        }

        //ごり押しの極み 重みを任意のタイミングで変更するための関数
        public void updateWeight(IAgent agent)
        {

            //foreach (var item in Network.Nodes.Count())
            //{
            //    AgentIO agent = this.Network.Nodes.ElementAt(i) as AgentIO;
            //}
            
            if (this.Network.Links == null)
            {
                Console.WriteLine("updataWeightのagentはnullです");
            }
            else
            {
                AgentIO a = agent as AgentIO;
                foreach (var i in a.Neighbours)
                {
                    var n = i as IAgent;
                    //a.updateEdgeWeight(n);

                    if (a.Algorithm is WeightedNeighbour)
                    {
                        (a.Algorithm as WeightedNeighbour).updateEdgeWeight(n);
                    }
                }
                //var opinion = a.Opinion as BlackWhiteSubject;
                //var mes = new BWMessage(a.Opinion, a);
                //foreach (var n in a.Neighbours)
                //{
                //    algo.updateEdgeWeight(a, mes);
                //    algo.ProcessMessage();
                //}
               
                //foreach (var n in agent.Neighbours)
                //{
                //    algo.updateEdgeWeight(n, message);
                //}                            
            }


            //var selectedIndexes = RandomPool.Get("envset").getRandomIndexes(this.Network.Nodes.Count(), sensorNum);

            //foreach (var i in selectedIndexes)
            //{
            //    //センサーを生成
            //    Sensor s = new Sensor(TheFact);

            //    //センサーにエージェントをセット(内部でAgentのhavesensorをtrueにしてる．微妙に密結合)
            //    AgentIO agent = this.Network.Nodes.ElementAt(i) as AgentIO;
            //    s.SetAgent(agent);
            //    agent.SetSensor(s);
            //    this.Sensors.Add(s);
            //}


        }



        internal void Initialize()
        {
            //エージェントをコンストラクト直後の状態に戻す
            foreach(IAgent a in Network.Nodes){
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

            //インフルエンシャルの設定
            //PreparePurpose();
        }

        private Network GenerateNetwork(NetworkGenerator generator)
        {
            //中身:ネットワークとセンサー
            return generator.create();
        }



        protected virtual void PrepareSensor(int sensorNum)
        {
           //センサー
            Sensors = new List<Sensor>();

            //センサーをもつエージェントを決める
            var selectedIndexes = RandomPool.Get("envset").getRandomIndexes(this.Network.Nodes.Count(), sensorNum);

            foreach (var i in selectedIndexes)
            {
                //センサーを生成
                Sensor s = new Sensor(TheFact);

                //センサーにエージェントをセット(内部でAgentのhavesensorをtrueにしてる．微妙に密結合)
                AgentIO agent = this.Network.Nodes.ElementAt(i) as AgentIO;
                s.SetAgent(agent);
                agent.SetSensor(s);
                this.Sensors.Add(s);
        
            }
            //20161208 sensorの精度をバラバラにする
            //this.Sensors.First().Accuracy = 0.1;
        }

        //インフルエンシャルの割り当て
        //private void PreparePurpose(double ImportanceLevel)//Network net INode node, BeliefUpdater importanceLevel)
        //{
        //    AgentIO sensorAgent = this.Network.Nodes
        //    for (int i = 0; i < length; i++)
        //    {
			 
        //    }
            

        //    //List<double> ImportanceLevelList = new List<double>{};
        //    //for (int i = 0; i < this.Network.Nodes.Count(); i++)
        //    //{
        //    // AgentIO agent = this.Network.Nodes.ElementAt(i) as AgentIO;
        //    //    AgentIO n
        //    //    List<AgentIO> neighbours = new List<AgentIO>{};
        //    //    if (agent.HasSensor)
        //    //    {
        //    //        agent = agent.Neighbours;
        //    //    }
		 	                                
            
        //    ////センサーの近隣エージェントかどうか
        //    ////信用度の高いエージェントをセットする


        //    //if ()
        //    //{
                
        //    //}
        //    //}
        //}

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


                foreach (IAgent a in this.Network.Nodes)
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
