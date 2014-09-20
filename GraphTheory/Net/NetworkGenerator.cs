using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using MyRandom;

namespace GraphTheory.Net
{

    public delegate INode NodeCreater();

    public delegate NodeCreater NodeCreaterCreater(double h_trg);

    public abstract class NetworkGenerator
    {

        public NetworkGenerator()
        {
        }
        public abstract Network create();
        public NodeCreater NodeCreate{ get;set; }

    }




    /*
    public class RandomNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;

        public RandomNetworkGenerator(int n)
        {
            NodeNum = n;
        }

        public override Network create()
        {
            Console.WriteLine("make random network!");
            //ネットワーク
            Network net = new Network();

            //ノードであるAgent
            for (int i = 0; i < NodeNum; i++)
            {
                net.AddNode(NodeCreate());//エージェントを選択可能にしたいよね
            }

            foreach (var Node_i in net.Nodes)
            {
                foreach (var Node_j in net.Nodes)
                {
                    if (Node_i == Node_j)
                    {
                        break;
                    }

                    double r = RandomPool.Get("envset").NextDouble();

                    if (r < 0.3)
                    { //0.3の確率でコネクション作成
                        net.ConnectNodes(Node_i, Node_j);
                    }
                }

                var nodes = net.Nodes;
                if (Node_i.Neighbours.Count() == 0)
                {
                    int r;
                    do
                    {
                        r = RandomPool.Get("envset") .Next(NodeNum);
                    } while (nodes.ElementAt(r) == Node_i);

                    net.ConnectNodes(Node_i, nodes.ElementAt(r));
                }
            }
            //まだ！！
            return net;
        }
    }
    */


}
