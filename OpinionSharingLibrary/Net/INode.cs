using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Net
{
    public interface INode
    {
        //自分が属するネットワーク
        Network Network { get; set; }//Networkぐらいにしかいじってほしくない

        //IDを返す
        int ID { get;}

        //近隣を返す
        ISet<INode> Neighbours{get;}//重複がないSet
    }
}
