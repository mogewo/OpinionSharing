using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyRandom;

namespace GraphTheory.Net
{
    public class WSmodelNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;
        private int degree;
        private double p_rewire;

        public WSmodelNetworkGenerator(int aNum, int d, double p)
        {
            if (aNum < d)
            {
                throw new Exception("エージェントの数より完全グラフの数の方が大きいです");
            }

            if (p < 0 || p > 1)
            {
                throw new Exception(p + "は確率ではありません");
            }

            NodeNum = aNum;
            degree = d;
            p_rewire = p;
        }

        public override Network create()
        {
            //RandomPool.Get("envset").Init();
            Network net = new Network();

            INode[] Nodes = new INode[NodeNum];
            

            //エージェントを登録
            for (int i = 0; i < NodeNum; i++)
            {
                Nodes[i] = NodeCreate();
                net.AddNode(Nodes[i]);
            }

            //まずは輪っかをつくる
            for (int i = 0; i < NodeNum; i++)
            {
                //degreeが偶数じゃないとちゃんと働きません！！つか奇数ってどうやるの！？

                for (int j = 1; j <= degree/2; j++)
                {
                    int index = i + j;

                    if (index >= NodeNum)
                    {//indexが範囲を超えていたら、一周回らせて収める
                        index = index - NodeNum;
                    }

                    net.ConnectNodes(Nodes[i], Nodes[index]);
                }
            }



            /* pの割合のリンクをreconnectする。*/

            //rewireするべきlinkをみつける
            var linksToRewire = SelectLinksToRewire(net.Links);

            foreach (var link in linksToRewire)//それぞれのリンクを張り替える
            {
                //リンクをはずす。
                net.DisconnectNode(link);

                //軸はどっちか
                INode pivot = (RandomPool.Get("envset").NextDouble() <= 0.5 ? link.Node1 : link.Node2);

                //ご近所じゃないやつを探す
                var neighbourOfPivot = net.GetNeighbour(pivot);
                var allNodes = new List<INode>(net.Nodes);

                foreach (var node in neighbourOfPivot)
                {
                    allNodes.Remove(node);
                }
                allNodes.Remove(pivot);
                var notNeighbour = allNodes;

                //次のパートナーを選ぶ。
                INode nextPartner = notNeighbour.ElementAt(RandomPool.Get("envset").Next(notNeighbour.Count));

                net.ConnectNodes(pivot, nextPartner);
            }

            RandomPool.Get("envset").Init();

            return net;
        }

        public IEnumerable<Link> SelectLinksToRewire(IEnumerable<Link> links)
        {
            int linksNum = links.Count();
            int rewireNum = (int)(p_rewire * linksNum);

            IEnumerable<int> selectedIndex = RandomPool.Get("envset").getRandomIndexes(linksNum, rewireNum);

            List<Link> selectedLinks = new List<Link>();
            foreach (var index in selectedIndex)
            {
                selectedLinks.Add(links.ElementAt(index));
            }

            return selectedLinks;
        }



    }
}
