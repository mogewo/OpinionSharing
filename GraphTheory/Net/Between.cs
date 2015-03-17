using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class Between
    {
        /// <summary>
        /// 媒介距離測定メソッドの実装
        /// ある点を経由した二点の媒介最短経路を求めます（主に媒介中心性で使用）
        /// </summary>
        public static List<Dictionary<INode, int>> Between_Distance(Network net)
        {
            int n = net.Nodes.Count();

            //二次元連想配列
            Dictionary<INode, Dictionary<INode, int>> bd = new Dictionary<INode, Dictionary<INode, int>>();  //媒介距離連想配列
            List<Dictionary<INode, int>> valuesList = new List<Dictionary<INode, int>>(bd.Values);

            //初期化
            foreach (var nodeMiddle in net.Nodes)
            {
                bd[nodeMiddle] = new Dictionary<INode, int>();
                foreach (var nodeLeft in net.Nodes)
                {
                    foreach (var nodeRight in net.Nodes)
                    {
                        bd[nodeLeft][nodeMiddle] = Int32.MaxValue;
                        bd[nodeMiddle][nodeRight] = Int32.MaxValue;
                    }
                }
            }
            //接続されているリンクには距離を入れておく
            foreach (var link in net.Links)
            {
                bd[link.Node1][link.Node2] = 1;
                bd[link.Node2][link.Node1] = 1;
            }

            foreach (var k in net.Nodes)
            {
                foreach (var i in net.Nodes)
                {
                    foreach (var j in net.Nodes)
                    {
                        //iとjが繋がっている
                        bool directConnected = bd[i][j] != Int32.MaxValue;
                        //iとjがk経由で繋がっている
                        bool indirectConnected = (bd[i][k] != Int32.MaxValue) && (bd[k][j] != Int32.MaxValue);

                        if (directConnected && indirectConnected)
                        {
                            if (bd[i][j] >= bd[i][k] + bd[k][j])
                            {
                                bd[i][j] = bd[i][k] + bd[k][j];
                            }
                        }
                        else if (directConnected && !indirectConnected)
                        {
                            bd[i][j] = Int32.MaxValue;
                        }
                        else if (!directConnected && indirectConnected)
                        {
                            bd[i][j] = bd[i][k] + bd[k][j];
                        }
                        valuesList = bd.Values.ToList();
                    }
                }
            }

            return valuesList;
        }
    }
}
