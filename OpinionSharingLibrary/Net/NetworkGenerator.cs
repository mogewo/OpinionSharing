using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing.Util;

using System.Diagnostics;



namespace OpinionSharing.Net
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
                    net.ConnectNode(Node_i, Node_j);

                }

            }

            return net;
        }


    }

    public class RandomNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;

        public RandomNetworkGenerator(int aNum)
        {
            NodeNum = aNum;
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

                        net.ConnectNode(Node_i, Node_j);
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

                    net.ConnectNode(Node_i, nodes.ElementAt(r));


                }
            }
            //まだ！！
            return net;

        }

    }
    /*
    class SmallworldNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;
        private int completeNum;

        public SmallworldNetworkGenerator(int aNum, int Km)
        {
            if (aNum < Km)
            {
                throw new Exception("エージェントの数より完全グラフの数の方が大きいです");
            }

            NodeNum = aNum;
            completeNum = Km;


        }
        public Network create()
        {

            NetworkGenerator cpltNetGen = new CompleteNetworkGenerator(completeNum);
            Network net = cpltNetGen.create();//完全グラフを生成

            int currentNum = completeNum;//現在のノードの数

            while(currentNum != NodeNum
            net
        }

    }*/

    public class SmallworldRingNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;
        private int degree;
        private double p_rewire;


        public SmallworldRingNetworkGenerator(int aNum, int d, double p)
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
            RandomPool.Get("envset").Init();

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

                    net.ConnectNode(Nodes[i], Nodes[index]);
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

                net.ConnectNode(pivot, nextPartner);
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
