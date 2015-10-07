using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyRandom;//20150525：重みをつけるために乱数を使いたい

namespace GraphTheory.Net
{
    public class EdgeWeight
    {
        /// <summary>
        /// 重みエッジの実装
        /// リンクに重みを実装します
        /// </summary>

        public static Dictionary<int, Dictionary<int,double>> edgeWeight_up(Network net)
        {

            
            Dictionary<int, Dictionary<int, double>> ew = new Dictionary<int, Dictionary<int, double>>();//重み保存配列
            

            //初期化
            foreach (var nodeLeft in net.Nodes)
            {
                ew[nodeLeft.ID] = new Dictionary<int, double>();
                       
                foreach (var nodeRight in net.Nodes)
                {
                    ew[nodeLeft.ID][nodeRight.ID] = Int32.MaxValue;
                                      
                   
                }
            }           
            //foreach (var nodeLeft in node.)
            //{
            //    //int x = nodeLeft.ID;
            //    //net.GetNeighbour(nodeLeft);
            //    //ew[nodeLeft] = new Dictionary<INode, double>();
                
                            
            //    foreach (var nodeRight in net.Nodes)
            //    {
            //        ew[nodeLeft][nodeRight] = Int32.MaxValue;                                      
            //    }
            //}

            //接続されているリンクに重みを配置する 0~10までの整数を代入
            foreach (var link in net.Links)
            {
                ew[link.Node1.ID][link.Node2.ID] = RandomPool.Get("envset").NextDouble();
                ew[link.Node2.ID][link.Node1.ID] = RandomPool.Get("envset").NextDouble();


                ////お試し版重み分布　08/06
                ////次数が10以上のagtは重みを重視される 0.8<=ew<=1.0
                //if (link.Node1.Neighbours.Count <= 10)
                //{
                //    ew[link.Node2.ID][link.Node1.ID] = RandomPool.Get("envset").NextDouble(0.8, 1.0);
                //}
                //if (link.Node2.Neighbours.Count <= 10)
                //{
                //    ew[link.Node1.ID][link.Node2.ID] = RandomPool.Get("envset").NextDouble(0.8, 1.0);
                //}

            }

            foreach (var link in net.Links)
            {
                if (link.Node1.Neighbours.Count <= 10)
                {

                }
                ew[link.Node1.ID][link.Node2.ID] = RandomPool.Get("envset").NextDouble();
                ew[link.Node1.ID][link.Node2.ID] = RandomPool.Get("envset").NextDouble();

            }
            return ew;

        }
    }
}
