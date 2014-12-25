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
        public static int[,] warshall_Floyd(Network net)
        {
            int n = net.Nodes.Count();
            Dictionary<INode, Dictionary<INode, int>> d = new Dictionary<INode, Dictionary<INode, int>>();

            //dの初期化
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    d[][] = Int32.MaxValue;//
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

            return d;
        }
    }
}
