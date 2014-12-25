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
        public static Dictionary<INode, Dictionary<INode, int>> warshall_Floyd(Network net)
        {
            int n = net.Nodes.Count();
            
            //二次元連想配列
            Dictionary<INode, Dictionary<INode, int>> d = new Dictionary<INode, Dictionary<INode, int>>();            
            foreach (var nodeLeft in net.Nodes)
            {
                 d[nodeLeft] = new Dictionary<INode,int>();
                foreach (var nodeRight in net.Nodes)
                {
		           d[nodeLeft][nodeRight] = Int32.MaxValue;
                }		   
            }           

            //接続されているリンクには距離を入れておく
            foreach (var link in net.Links)
            {
                d[link.Node1][link.Node2] = 1;
                d[link.Node2][link.Node1] = 1;
            }


            foreach (var k in net.Nodes)
            {
                foreach (var i in net.Nodes)
                {
                    foreach (var j in net.Nodes)
                    {
                        //iとjが繋がっている
                        bool directConnected = d[i][j] != Int32.MaxValue;
                        //iとjがk経由で繋がっている
                        bool indirectConnected = (d[i][k] != Int32.MaxValue) && (d[k][j] != Int32.MaxValue);

                        if (directConnected && indirectConnected)
                        {
                            d[i][j] = System.Math.Min(d[i][j], d[i][k] + d[k][j]);
                        }
                        else if (directConnected && !indirectConnected)
                        {
                            d[i][j] = d[i][j];
                        }
                        else if (!directConnected && indirectConnected)
                        {
                            d[i][j] = d[i][k] + d[k][j];                            
                        }
                    }
                }
            }
            return d;
        }

        /// <summary>
        /// 自身を中継点としたの最短路測定
        /// </summary>
        public static int betweenessDistance(Network net)
        {
            warshall_Floyd(net);
            return 0;
        }
    }
}
