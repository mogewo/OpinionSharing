﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public interface INode
    {
        //自分が属するネットワーク
        Network Network { get; set; }//Networkぐらいにしかいじってほしくない

        //IDを返す
        int ID { get;}

        //近隣を返す
        ISet<INode> Neighbours{get;}//重複がないSet

        //他とつながれたときにNetworkから呼ばれる
        void connected(INode neighbour);

        //他と分断されたときにNetworkから呼ばれる
        void disconnected(INode neighbour); 

        //各ノードとの距離を返す
        IDictionary<INode, int> Distances { get; }

        //近隣との重みを返す　valueに近隣を与えたい
        //IDictionary<int, double> Edgeweights { get; }

        //媒介中心距離
        IList<Dictionary<INode, int>> Betweens { get; }

        //ステータス
        string Status { get;}

        string CsvStatus { get; }

        //string CsvAwarenessRate { get; }

    }
}
