using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class Distance
    {

        /// <summary>
        /// ワーシャルフロイド法の実装
        /// 最短経路を求めます
        /// </summary>
        public static void warshall_Floyd(Network net)
        {
            int n = net.Nodes.Count();
            int[,] d = new int[n,n];

            //dの初期化
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    d[i, j] = Int32.MaxValue;//
                }
            }

            //接続されているリンクには距離を入れておく
            foreach (var link in net.Links)
            {
                d[link.Node1.ID, link.Node2.ID] = 1;
                d[link.Node2.ID, link.Node1.ID] = 1;
            }            

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        d[i, j] = System.Math.Min(d[i, j], d[i, k] + d[k, j]);                        
                        //Console.WriteLine(d + "あああ");
                    }
                }
            }            
        }

        //
        //public static void warshall_Floyd2(Network net, int from, int to, int middle)
        //{
        //    int n = net.Nodes.Count();
        //    int[,] d = new int[n, n];

        //    //dの初期化
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            d[i, j] = Int32.MaxValue;//
        //        }
        //    }

        //    //接続されているリンクには距離を入れておく
        //    foreach (var link in net.Links)
        //    {
        //        d[link.Node1.ID, link.Node2.ID] = 1;
        //        d[link.Node2.ID, link.Node1.ID] = 1;
        //    }

        //    for (int k = 0; k < n; k++)
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < n; j++)
        //            {
        //                d[i, j] = System.Math.Min(d[i, j], d[i, k] + d[k, j]);
        //                //Console.WriteLine(d + "あああ");
        //            }
        //        }
        // }
        //}


        //最短路の更新
        //結局使ってないです
        private static void updateDistance(Network net, int[,] distance,int from, int to, int middle)
        {
            var node_start = net.Nodes.ElementAt(from);//接続元
            var node_end = net.Nodes.ElementAt(to);//接続先
            var node_middle = net.Nodes.ElementAt(middle);//中間地点
                        

            if (node_start.Neighbours.Contains(node_end))
            {
                            
            } 
            if (node_start.Neighbours.Contains(node_middle))
            {
                if (node_middle.Neighbours.Contains(node_end))
                {
                                
                }
            }
        }

        //距離平均
        private static void averageDistance(Network net,int [,] distance, int from, int to, int middle)
        {

            //for (int i = 0; i < length; i++)
            //{
            //    for (int i = 0; i < length; i++)
            //    {
                    
            //    }
            //}
        }
    }
}
