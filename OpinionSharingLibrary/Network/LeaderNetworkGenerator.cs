using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyRandom;

using GraphTheory.Net;

using OpinionSharing.Subject;

namespace OpinionSharing.Agt
{
    public class LeaderNetworkGenerator: NetworkGenerator
    {
        private int NodeNum;
        private int degree;
        private double p_rewire;
        private int linkNum;//leaderとつなぐ数

        public LeaderNetworkGenerator(int aNum, int expd, double p_rewire, int LinkNum)//エージェント数は100で固定（7/27）　
        {
            if (aNum < expd)
            {
                throw new Exception("エージェントの数より完全グラフの数の方が大きいです");
            }

            if (p_rewire < 0 || p_rewire > 1)
            {
                throw new Exception(p_rewire + "は確率ではありません");
            }

            this.NodeNum = aNum;
            this.degree = expd;
            this.p_rewire = p_rewire;
            this.linkNum = LinkNum;
        }

        public override Network create()
        {
            //RandomPool.Get("envset").Init();
            Network net = new Network();

            //計算量軽減のため，再計算を無効化する
            net.EnableCalculateDistance = false;

            INode[] Nodes = new INode[NodeNum];

            //エージェントを登録
            for (int i = 0; i < NodeNum; i++)
            {
                Nodes[i] = NodeCreate();
                net.AddNode(Nodes[i]);
            }

            //まずは輪っかをつくる
            for (int i = 0; i < NodeNum-1; i++)
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


            List<AgentIO> sensorAgents = new List<AgentIO>();

            //センサーエージェントの一覧を求める
            foreach (var node in net.Nodes)
            {
                if ((node as AgentIO).HasSensor)
                {
                    sensorAgents.Add(node as AgentIO);
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

            //Nodes[NodeNum -1]に，全員をつながせたい 
            //2014_0707：ｎ人と繋がるように(NodeNum - n)に変更
            //2014_0708：全員と繋げるためにリンクの張替え後に移動しました
            //2014_0901leaderNodeをランダムに繋げるように設定        
            //for (int i = 0; i < 30; i++)
            //{
            //    Node[0]と，LeaderNodesをつなぐ
            //    int o = RandomPool.Get("envset").Next(linkNum);//leaderと繋がるノード                
            //    if (o != l)
            //    {
            //        net.ConnectNodes(Nodes[o], Nodes[l]);
            //    }
            int l = RandomPool.Get("envset").Next(linkNum);//leaderノード
            int k = 0;
            int o = 0;
                while (o >= 0&&k != 30)
                {
                    o = RandomPool.Get("envset").Next(linkNum);//leaderと繋がるノード                
                    if (o != l)
                    {
                        net.ConnectNodes(Nodes[o], Nodes[l]);//leaderとNodeをランダムに繋ぐ
                        k++;
                    }
                }
            //
            RandomPool.Get("envset").Init();///ここでノード指定？環境のノードっぽい

            //距離計算
            net.EnableCalculateDistance = true;
            net.updateDistance();
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

