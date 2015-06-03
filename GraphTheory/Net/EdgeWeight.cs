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
        public static Dictionary<INode, Dictionary<INode, int>> edgeWeight_up(Network net)
        {
            Dictionary<INode, Dictionary<INode, int>> ew = new Dictionary<INode, Dictionary<INode, int>>();//重み保存配列

            //初期化
            foreach (var nodeLeft in net.Nodes)
            {
                ew[nodeLeft] = new Dictionary<INode, int>();
                            
                foreach (var nodeRight in net.Nodes)
                {
                    ew[nodeLeft][nodeRight] = Int32.MaxValue;                                      
                }
            }

            //接続されているリンクに重みを配置する 0~10までの整数を代入
            foreach (var link in net.Links)
            {
                ew[link.Node1][link.Node2] = 1;//RandomPool.Get("weight").Next(10);
                ew[link.Node2][link.Node1] = 1;//RandomPool.Get("weight").Next(10);
            }
      


            return ew;
        }
    }
}
