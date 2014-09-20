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

        public RandomNetworkGenerator(int aNum,int maxK)
        {
            NodeNum = aNum;
            MaxK = maxK;
        }

        public override Network create()
        {
            Network net = new Network();

            List<INode> nodes = new List<INode>();

            INode n = NodeCreate();
            nodes.Add(n);
            net.AddNode(n);

            while (net.Nodes.Count() < NodeNum)
            {
                List<INode> newnodes = new List<INode>();
                foreach (var node in nodes)
                {

                    int k = SelectK() - node.Neighbours.Count;

                    for (int i = 0; i < k; i++)
                    {

                        var newnode = NodeCreate();
                        newnodes.Add(newnode);
                        net.AddNode(newnode);
                        net.ConnectNodes(node, newnode);

                        //満杯になったら抜ける
                        if (!(net.Nodes.Count() < NodeNum))
                            break ;
                    }

                    //満杯になったら抜ける
                    if (!(net.Nodes.Count() < NodeNum))
                        break ;
                }

                if (newnodes.Count == 0)
                    break;

                nodes = newnodes;
            }

            return net;
        }

        private int SelectK(){
            return MyRandom.RandomPool.Get("envset").Next(MaxK-1) + 1;
        }
    }
}
