using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    /// <summary>ブランチクラス</summary>
    public class DijkstraBranch
    {
        public readonly DijkstraNode Node1; //node1
        public readonly DijkstraNode Node2; //node2
        public readonly int Distnce;        //距離

        public DijkstraBranch(DijkstraNode node1, DijkstraNode node2, int distance)
        {
            Node1 = node1;
            Node2 = node2;
            Distnce = distance;
        }
    }

    /// <summary>ノードクラス</summary>
    public class DijkstraNode
    {
        public enum NodeSatus
        {
            NotYet,     //未確定状態
            Temporary,  //仮確定状態
            Completed   //確定状態
        }

        public int Distance;                //距離
        public DijkstraNode SourceNoode;    //ソースコード
        public NodeSatus Status;                   //最短経路確定状態
        //public Point Position;              //node位置

    }

    //public class Dijkstra
    //{
    //    public List<DijkstraBranch> Branches
    //    {

    //    }
    //}

}
