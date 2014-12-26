using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class NetworkIndexes
    {

        /// <summary>
        /// クラスタ係数の実装
        /// ネットワーク分析に用います
        /// </summary>
        public static double cluster(INode node, Network net)
        {
            int nodeID = node.ID;//nodeの固定
            int k = node.Neighbours.Count;//次数
            int triangleNum = 0;//三角形の総数
            double c = 0.0;//クラスタ係数   

            //

            //近隣エージェントは別になるように
            for (int i = 0; i < node.Neighbours.Count; i++)//node.Neighbours.Countで近隣エージェントの数、つまり次数
            {
                INode aNeighbour = node.Neighbours.ElementAt(i);

                for (int j = 0; j < i; j++)
                {
                    INode bNeighbour = node.Neighbours.ElementAt(j);
                    if (aNeighbour.Neighbours.Contains(bNeighbour))//友達同士かどうか
                    {
                        triangleNum++;
                    }
                }
            }

            c = (double)triangleNum / (k * (k - 1) / 2); //doubleに変換しないとだめ！！！            
            return c;
            //foreach (var aNeighbour in node.Neighbours)
            //{
            //    foreach (var bNeighbour in node.Neighbours)
            //    {

            //        //友達同士が友達がどうか
            //        if (aNeighbour.Neighbours.Contains(bNeighbour))
            //        {
            //            triangleNum++;
            //        }
            //    }

            //}

            //foreach(var aNeighbour in node.Neighbours){
            //    foreach(var bNeighbour in node.Neighbours){
            //        if(aNeighbour.Neighbours.Contains(bNeighbour)){
            //            //aとbはつながっている
            //            Console.Write("neighbour ");

            //        }

            //        if(net.Links.Contains(new Link(aNeighbour,bNeighbour))){
            //            //aとbはつながっている
            //            Console.Write("link ");
            //        }
            //        Console.WriteLine();
            //    }

            //}
        }

        /// <summary>
        /// 次数中心性の実装
        /// ネットワーク分析に用います
        /// </summary>
        public static double degreeCentrality(INode node, Network net)
        {
            int k = node.Neighbours.Count;//次数
            double cd = 0.0;//次数中心性

            cd = (double)k / net.Nodes.Count();
            return cd;
        }


        /// <summary>
        /// 媒介中心性の実装
        /// コミュニティの検出などのネットワーク分析に用います
        /// 最短経路求められないので途中で挫折．マーシャルフロイド法とか？
        /// </summary>
        public static double betweenessCentrality(INode node, Network net)
        {
            double cb;//媒介中心性
            int allNodes = net.Nodes.Count();//ノードの総数
            int g = ; //自分を中継路とした最短路総数
           
            //int d = node.Distances.Count;//自分を始点とした最短経路の数 こっちが正解？
            

            double n = ((allNodes - 1) * (allNodes - 2)) / 2;

            cb = g / n;

            return cb;
        }

        /// <summary>
        /// 近接中心性の実装（単純に次数，実装済み）
        /// リーダー，コミュニティの検出などネットワーク分析
        /// </summary>
        public static double closenessCentrality(INode node)
        {
            double cc;
            double a = averageDistanceNode(node);
            //0ならば異常値を返す
            if (a == 0)
            {
                return -1;
            }

            cc = 1 / a;

            return cc;
            
        }

        //距離平均
        public static double averageDistanceNode(INode node)
        {
           return node.Distances.Average(num => Convert.ToDouble(num.Value));
        }
        //最大距離
        public static int maxDistanceNode(INode node)
        {
            return node.Distances.Max(num => Convert.ToInt32(num.Value));
        }
        //最小距離
        public static double minDistanceNode(INode node)
        {
            return node.Distances.Min(num => Convert.ToInt32(num.Value));
        }


    }
}
