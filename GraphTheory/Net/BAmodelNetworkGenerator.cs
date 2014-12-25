using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyRandom;


namespace GraphTheory.Net
{

    public class BAModelNetworkGenerator : NetworkGenerator
    {
        private int NodeNum;
        private int m0;
        private int k;

        public BAModelNetworkGenerator(int n, int m0, int k)
        {
            if (n < m0)
            {
                throw new Exception("エージェントの数より完全グラフの数の方が大きいです");
            }
            if (m0 < k)
            {
                throw new Exception("完全グラフの数よりノードの数の方が大きいです");
            }

            this.NodeNum = n;
            this.m0 = m0;
            this.k = k;
        }

        public override Network create()
        {
            //まずは完全グラフ生成器を準備
            var cng = new CompleteNetworkGenerator(m0);
            cng.NodeCreate = this.NodeCreate;

            //完全グラフを生成
            var net = cng.create();

            //計算量軽減のため，再計算を無効化する
            net.EnableCalculateDistance = false;

            //順次成長させていく NodeNumになるまで
            while(net.Nodes.Count() < NodeNum)
            {
                //既存の全ノードを準備
                var candidateNodes =  net.Nodes.ToList();

                //新しいノードを追加
                var newNode = NodeCreate();
                net.AddNode(newNode);


                //k個を選ぶ
                for (int i = 0; i < k; i++)
                {
                    //既存ノードの中から選ぶ
                    var selectedNode = RuletSelect(candidateNodes);
                    candidateNodes.Remove(selectedNode);

                    net.ConnectNodes(selectedNode,newNode);
                }
            }

            //距離計算
            net.EnableCalculateDistance = true;
            net.updateDistance();

            return net;
        }

        private INode RuletSelect(IEnumerable<INode> nodes)
        {
            int sum = 0;
            int cum = 0;

            double r = RandomPool.Get("envset").NextDouble();

            foreach (var node in nodes)
            {
                sum += node.Neighbours.Count;
            }

            foreach (var node in nodes)
            {
                cum += node.Neighbours.Count;
                if (r < (double)cum / sum)
                {
                    return node;
                }
            }

            throw new Exception("何が起こった！？");
        }

    }
}
