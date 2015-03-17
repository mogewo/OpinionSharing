using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class RandomNetworkGenerator : NetworkGenerator
    {

        private int NodeNum;
        private int MaxK;
        private double p_rewire;

        public RandomNetworkGenerator(int aNum,int maxK, double p)
        {
            NodeNum = aNum;
            MaxK = maxK;
            p_rewire = p;
        }

        public override Network create()
        {
            Network net = new Network();
            //計算量軽減のため，再計算を無効化する
            net.EnableCalculateDistance = false;
            INode[] Nodes = new INode[NodeNum];
            List<INode> nodes = new List<INode>();

            INode n = NodeCreate();
            nodes.Add(n);
            net.AddNode(n);

            for (int i = 0; i < NodeNum; i++)
            {
                Nodes[i] = NodeCreate();
                net.AddNode(Nodes[i]);
            }

            for (int i = 0; i < NodeNum; i++)
            {
                for (int j = i+1; j < NodeNum ; j++)
                {
                    if (Linkrand() >= p_rewire)
                    {
                        net.ConnectNodes(Nodes[i], Nodes[j]);
                    }
                    
                }   
            }

           

            //距離計算
            net.EnableCalculateDistance = true;
            net.updateDistance();
            return net;
        }

        private double Linkrand()
        {
            return MyRandom.RandomPool.Get("envset").NextDouble();
        }
    }
}
