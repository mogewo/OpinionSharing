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
        //        public static double betweenness(INode node, Network net, List<Link> edge)// , Network edge)
        //        {
        //            int nodeNum,
        //                edgeNum,
        //                node_start,
        //                node_end,
        //                u,
        //                w,
        //                first,
        //                nodeID;
        //            nodeNum = net.Nodes.Count();//ノード数
        //            edgeNum = net.Links.Count();//枝数

        //            /*10/16ここまでやりました，最短距離がわからないので挫折*/
        //            node_start = edge[].Node1;
        //            node_end = edge[].Node2;
        //            /**/

        //            nodeID = node.ID;//どの頂点かを示す
        //            int[] d = new int[nodeNum];//作業用配列
        //            double[] sigma = new double[nodeNum];//sigma[i]は頂点iを通る最短経路の数
        //            double[] delta = new double[nodeNum];//媒介中心性への貢献度
        //            int [] pair = new int[2 * edgeNum];//最短路上にある親子関係の数
        //            int[] s = new int[nodeNum];//スタック
        //            int[] q = new int[nodeNum];//キュー
        //            //int N_pair = pair.Length;//配列の実質的な要素数
        //            //int N_s = s.Length;//配列の実質的な要素数
        //            int q_start;//q開始地点
        //            int q_end;//q終了地点
        //            int N_pair,N_s;//配列の実質的な要素数

        //            //枝（Va,Vb）と枝（Vb,Va）を区別したリスト
        //            //List<Link> edge_list = new List<Link>(4 * edgeNum);
        //            ////List<int> edge_list = new List<int>(4 * edgeNum);
        //            //for (int i = 0; i < edge_list.Count; i++)
        //            //{
        //            //    edge_list[i] = node.Network.Links.ElementAt(i);//ノードのリンクとして扱いますよー，ElementAt(i)でそのリンク？
        //            //}

        //            //Console.WriteLine(edge.Count);
        //            //同じ枝の組み合わせは排除する，どうやる？→Linkクラスでやってあった流石石井ちゃん
        //            //for (int i = edgeNum - 1; i >= 0; i--)
        //            //{
        //            //    edge_list[4 * i] = edge_list[4 * i + 3] = edge[2 * i];
        //            //    edge_list[4 * i + 1] = edge_list[4 * i + 2] = edge[2 * i + 1];
        //            //}
        //            ////上記のリストの昇順ソート
        //            //edge_list.Sort();

        //            /* なにしてるかようわからんけど書いた
        //               たぶんPの設定かも */
        //            int[] P = new int[nodeNum];//P[i]は始点の番号がi以下の枝数
        //            for (int i = 0; i < nodeNum; i++)
        //            {
        //                P[i] = 0;
        //            }
        //            for (int i = 0; i < edgeNum; i++)
        //            {
        //                P[edge_list[2 * i]]++;//edge_ListのIDを渡す
        //            }
        //            for (int i = 0; i < nodeNum; i++)
        //            {
        //                P[i] += P[i - 1];
        //            }
        //            /* 媒介中心性とその初期化 */
        //            double[] betweenness = new double[nodeNum];//媒介中心性
        //            for (int i = 0; i < nodeNum; i++)
        //            {
        //                betweenness[i] = 0;
        //            }

        //            /* 全ての始点について頂点iからの距離を初期化 */
        //            for (node_start = 0; node_start < nodeNum; node_start++)//全ての始点(node_start)について
        //            {
        //                for (int i = 0; i < nodeNum; i++)
        //                {
        //                    d[i] = -1;                  //頂点iからの距離を初期化
        //                    sigma[i] = delta[i] = 0;
        //                }
        //            }

        //            d[node_start] = 0;
        //            sigma[node_start] = 1.0;
        //            N_s = 0;//スタックSは空
        //            /*キューにnode_startを入れる*/
        //            q[0] = node_start;
        //            q_start = 0;
        //            q_end = 1;
        //            N_pair = 0;//配列pairは空

        //            /*キューが空でない限り次の処理を行う*/
        //            while (q_start != q_end)
        //            {
        //                /*キューから要素を一つ取り出す*/
        //                u = q[q_start];
        //                q_start++;
        //                /*スタックにuを入れる*/
        //                s[N_s] = u;
        //                N_s++;

        //                if (u == 0)
        //                {
        //                    first = 0;
        //                }
        //                else
        //                {
        //                    first = P[u - 1];
        //                }

        //                /*uの全ての隣接点について次の処理を行う*/
        //                for (int i = first; i < P[u]; i++)
        //                {
        //                    node_end = edge_list[2 * i + 1];//nodeEndのIDを渡す
        //;
        //                    if (d[node_end] < 0)//node_endが初めて見つかったならば
        //                    {
        //                        d[node_end] = d[u] + 1;//最短路である
        //                        /*キューにnode_endを入れる*/
        //                        q[q_end] = node_end;
        //                        q_end++;
        //                    }
        //                    if (d[node_end] == d[u] + 1)//最短路であるならば
        //                    {
        //                        delta[node_end] += delta[u];
        //                        pair[2 * N_pair] = node_end;//子
        //                        pair[2 * N_pair] = u;//親
        //                        N_pair++;//親子関係の数
        //                    }
        //                }
        //            }

        //            Array.Sort(pair);//子について昇順にソートする

        //            /*スタックsが空になるまで次の処理を行う*/
        //            while (N_s > 0)
        //            {
        //                /*スタックから要素を取り出す*/
        //                w = s[N_s - 1];
        //                N_s--;

        //                if (w != node_start)
        //                {
        //                    int i = 0;
        //                    /*wとiが親子である最小のiを探す*/
        //                    if (w > 0)
        //                    {
        //                        /*二分探索などで高速化する*/
        //                        while (pair[2 * i] != w)
        //                        {
        //                            i++;
        //                        }
        //                        while (pair[2 * i] == w && i < N_pair)//pair[2*i+1]はwの親
        //                        {
        //                            delta[pair[2 * i + 1]] += sigma[pair[2 * i + 1]] / sigma[w] * (1 + delta[w]);
        //                        }
        //                        betweenness[w] += delta[w];
        //                    }
        //                }
        //            }

        //            //Console.WriteLine(betweenness[nodeID]);
        //            return betweenness[nodeID];//出力はそのノードの媒介中心性
        //        }

        //public static double Modularity(INode node1,INode node2, Network net)
        //{
        //    //int communityNum = 0;//コミュニティ数
        //    //List<INode>community = new List<INode>(communityNum);
        //    int a;
        //    int x;
        //    int edgeNum = net.Links.Count();//総枝数
        //    int k_a = node1.Neighbours.Count;//node1次数
        //    int k_b = node2.Neighbours.Count;//node2次数
        //    int k_sum;//次数和
        //    double q;//Modularity

        //    //node1とnode2の間に枝があるなら
        //    if (node1.Neighbours.Contains(node2))
        //    {
        //        a++;
        //    }

        //    //communityのノード
        //    for (int i = 0; i < 100; i++)
        //    {
        //        for (int j = 0; j < i; j++)
        //        {
        //            x = a - ((k_a * k_b) / 2 * edgeNum);

        //        }

        //    }

        //    //for (int i = 0; i < net.Nodes.Count(); i++)
        //    //{
        //    //    k_sum+=k;
        //    //}

        //     //コミュニティかどうかの判断
        //    if (q > 0.3)
        //    {
        //       //communityNum++;
        //    }


        //    //q = (1 / 2 * edgeNum) * 
        //    return q;
        //}


        /// <summary>
        /// 近接中心性の実装（単純に次数，実装済み）
        /// リーダー，コミュニティの検出などネットワーク分析
        /// </summary>
        //public static double closenessCentrality(INode node, Network net)
        //{
            
        //}
    }
}
