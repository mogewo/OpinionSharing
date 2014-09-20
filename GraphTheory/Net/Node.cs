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


        public override string ToString()
        {
 	         return string.Format("[Node id:{0}]",ID);
        }


    }
}
