using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace GraphTheory.Net
{
    public class Link
    {
        public readonly INode Node1;
        public readonly INode Node2;

        public Link(INode n1, INode n2)
        {
            Node1 = n1;
            Node2 = n2;
        }

        public INode getAnother(INode n)
        {
            if (n == Node1)
            {
                return Node2;
            }
            else if (n == Node2)
            {
                return Node1;
            }
            else
            {
                throw new Exception("ノード{0}はリンク{1}に含まれません。");
            }
        }

        public override string ToString()
        {
            return string.Format("[link {0} -- {1}]", Node1, Node2);
        }

    }
}
