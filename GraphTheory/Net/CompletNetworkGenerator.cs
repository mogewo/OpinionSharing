using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class CompleteNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;
        public CompleteNetworkGenerator(int num)
        {
            NodeNum = num;
        }

        public override Network create()
        {
            //完全グラフ

            //ネットワーク
            Network net = new Network();


            //エージェントを登録
            for (int i = 0; i < NodeNum; i++)
            {
                net.AddNode(NodeCreate());
            }

            //完全ネットを作成
            foreach (var Node_i in net.Nodes)
            {
                foreach (var Node_j in net.Nodes)
                {
                    if (Node_i == Node_j)
                        break;
                    net.ConnectNodes(Node_i, Node_j);
                }
            }
            return net;
        }
    }
}
