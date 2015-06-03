using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class Node :INode
    {
        //*** static members ***//
        static private int maxId = 0;

        //*** private members ***//
        // 表示のため便宜上の識別ID。
        private int id;

        // 自分が属するネットワーク
        private Network net = null;

        //距離配列
        private Dictionary<INode, int> distance = new Dictionary<INode, int>();

        //重みedge配列
        private Dictionary<INode, int> edgeweight = new Dictionary<INode, int>();

        //媒介距離リスト
        private List<Dictionary<INode, int>> between = new List<Dictionary<INode, int>>();


        public　string Status
        {
            get { return "test"; }

        }

        public string CsvStatus
        {
            get
            {
                return "test";
            }

        }

        public Node()
        {
            id = maxId;
            maxId++;
        }       

        
        public int ID
        {
            get
            {
                return id;
            }
        }        

        public Network Network { 
            get { 
                return net; 
            }
            set{
                if(value == null)
                    throw new NullReferenceException();
                net = value;
            }
        }


        public ISet<INode> Neighbours
        {
            get
            {
                return net.GetNeighbour(this);
            }
        }

        public IDictionary<INode, int> Distances
        {
            get
            {
                return distance;
            }            
        }

        //重みエッジ
        public IDictionary<INode, int> Edgeweights
        {
            get
            {
                return edgeweight;
            }
        }

        public IList<Dictionary<INode, int>> Betweens
        {
            get
            {
                return between;
            }
        }
       
        public override string ToString()
        {
 	         return string.Format("[Node id:{0}]",ID);
        }


    }
}
