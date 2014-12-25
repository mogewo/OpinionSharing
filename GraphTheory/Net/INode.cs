using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public interface INode
    {
        //自分が属するネットワーク
        Network Network { get; set; }//Networkぐらいにしかいじってほしくない

        //IDを返すkikurage
        int ID { get;}

        //近隣を返すkikurage
        ISet<INode> Neighbours{get;}//重複がないSet

        //各ノードとの距離を返す
        IDictionary<int, double> Distances { get; }
    }
}
