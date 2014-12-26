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
        public static Dictionary<INode, Dictionary<INode, int>> warshall_Floyd(Network net, out Dictionary<INode, Dictionary<INode, Dictionary<INode, int>>> betweenessDistance, out List<int> candidatesBetweenessDistance)
        {
            int n = net.Nodes.Count();
            
            //二次元連想配列
            Dictionary<INode, Dictionary<INode, int>> d = new Dictionary<INode, Dictionary<INode, int>>();  //距離連想配列
            var b = new Dictionary<INode, Dictionary<INode, Dictionary<INode, int>>>();                     //媒介距離連想配列
            var c = new List<int>();                                                                        //媒介距離総数格納用の配列
            //初期化
            foreach (var nodeLeft in net.Nodes)
            {
                d[nodeLeft] = new Dictionary<INode,int>();
                b[nodeLeft] = new Dictionary<INode, Dictionary<INode, int>>();              

                foreach (var nodeRight in net.Nodes)
                {
		            d[nodeLeft][nodeRight] = Int32.MaxValue;
                    b[nodeLeft][nodeRight] = new Dictionary<INode, int>();                    
                    foreach (var betweeness in net.Nodes)
                    {
                        b[nodeLeft][nodeRight][betweeness] = Int32.MaxValue;                        
                    }
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
                            b[i][j][k] = d[i][j];
                            
                            c.Add(d[i][k] + d[k][j]);
                        }
                        else if (directConnected && !indirectConnected)
                        {
                            d[i][j] = d[i][j];
                        }
                        else if (!directConnected && indirectConnected)
                        {
                            d[i][j] = d[i][k] + d[k][j];
                            b[i][j][k] = d[i][j];
                            c.Add(d[i][j]);
                        }
                    }
                }
            }
            return d;
        }
    }
}
