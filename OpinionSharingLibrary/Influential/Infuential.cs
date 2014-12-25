using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyRandom;
using GraphTheory.Net;
using OpinionSharing.Agt.Algorithm;
using OpinionSharing.Subject;

namespace OpinionSharing.Agt
{
    //とりあえずpublicにしてしまったけどだいじょうぶなんか？
    public class Influential
    {
        private IAgent agent;
        private Purpose Purpose;

        //インフルエンシャルは目的を持つ
        //前提じゃなくね？
        public Influential(Purpose  p)　
        {
            Purpose = p;
        }

        public IAgent Agent
        {
            get
            {
                return agent;
            }
            set
            {
                SetAgent(value);
            }
        }

        //agentのセット
         public void SetAgent(IAgent a)
        {
             //agentが目的をもっていたらセットする→これをEnvironmentで具体的に割り当て
            agent = a;
            
        }

        //private int NodeNum;
        //private int degree;
        //private double p_rewire;
        //private IAgent agent;
        ////private int linkNum;

        //public Influential(int aNum, int expd, double p_rewire)//エージェント数は100で固定（7/27）　
        //{
        //    if (aNum < expd)
        //    {
        //        throw new Exception("エージェントの数より完全グラフの数の方が大きいです");
        //    }

        //    if (p_rewire < 0 || p_rewire > 1)
        //    {
        //        throw new Exception(p_rewire + "は確率ではありません");
        //    }

        //    this.NodeNum = aNum;
        //    this.degree = expd;
        //    this.p_rewire = p_rewire;
            
        //    //this.linkNum = LinkNum;
        //}

        //public void infuentialList(INode node, Network net)
        //{
        //    //Network net = new Network();
        //    INode[] Nodes = new INode[NodeNum];
        //    List<AgentIO> sensorAgents = new List<AgentIO>();
        //    List<AgentIO> sensorNeighbours = new List<AgentIO>();

        //    //センサーエージェントの一覧を求める
        //    foreach (var node in net.Nodes)
        //    {
        //        if ((node as AgentIO).HasSensor)
        //        {
        //            sensorAgents.Add(node as AgentIO);
        //        }
        //    }

        //    //センサーエージェントの友達を調べ，リストに加える
        //    foreach (var neighbour in )
        //    {
        //        if ((neighbour as AgentIO).HasSensor)
        //        {
        //            sensorNeighbours.Add(neighbour as AgentSpec);
        //        }
        //    }

        //    //友人の中で

        //    //if(showSensorNetworkCB.Checked && showOnlySensorNetworkCB.Checked){
        //    //        センサーネットワークに入っているか否か
        //    //        bool inNetwork = false;

        //    //        センサーエージェントならば入っている
        //    //        if (agentIO.HasSensor)
        //    //        {
        //    //            inNetwork = true;

        //    //        }
        //    //        それ以外でも，友達がセンサーネットワークならばOK
        //    //        else
        //    //        {
        //    //            友達を調べる
        //    //            foreach (var neighbour in agentIO.Neighbours)
        //    //            {
        //    //                if ((neighbour as AgentIO).HasSensor)
        //    //                {
        //    //                    inNetwork = true;
        //    //                }
        //    //            }
        //    //        }
        //    Console.WriteLine(sensorAgents.Count());
        //    Console.WriteLine("0000000000");    
        //}

    }
}
