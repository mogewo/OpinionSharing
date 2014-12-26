using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace GraphTheory.Net
{
    public class Network
    {
        //こいつがagentsとmembersをあらわす。エージェントからそのご近所さんへのマップ
        private Dictionary<INode, HashSet<INode>> members = new Dictionary<INode, HashSet<INode>>();
        //距離の再計算を行うフラグ
        private bool calculateDistance = true;
        public Network() {}
        //再計算フラグのプロパティ
        public bool EnableCalculateDistance
        {
            get { return calculateDistance; }
            set { calculateDistance = value; }
        }

        //距離計算
        public void updateDistance()
        {
            Dictionary<INode, Dictionary<INode, int>> distance = Distance.warshall_Floyd(this,Nodes,);
            foreach (var nodeLeft in Nodes)
            {
                //とりあえず空にしとく（リンクの増減に対応するため）
                nodeLeft.Distances.Clear();
                //全通り探索
                foreach (var nodeRight in Nodes)
                {
                    //自分自身を考慮しない
                    if (nodeLeft != nodeRight)
                    {
                        int d = distance[nodeLeft][nodeRight];
                        //接続されていない場合は書き込まない
                        if (d != Int32.MaxValue)
                        {
                            nodeLeft.Distances[nodeRight] = d;
                        }
                    }                   
                }
            }
        }
        public void AddNode(INode node)
        {
            //Nodeがネットワークに属していなければ、OK. 自分に登録する
            if (node.Network == null)
            {
                node.Network = this;
                members.Add(node, new HashSet<INode>());
            }

            //自分自身に属していたら、なにもしない
            else if (node.Network == this)
            {
                Console.WriteLine("the Node" + node.ToString() + " is already a member of this network");
            }

            //ほかのネットに属していたら，おかしい．
            else
            {
                throw new Exception("the Node" + node.ToString() + " is already a member of some other network");
            }
        }
        
        public void ConnectNodes(INode a1, INode a2)
        {
            //エージェントがネットワークに登録されてなかったら
            if (!members.ContainsKey(a1)){
                AddNode(a1);
            }
            if (!members.ContainsKey(a2)){
                AddNode(a2);
            }
            //自分自身との接続は無効
            if (a1 == a2)
            {
                throw new Exception("自分自身と接続することはできません。");
            }

            members[a1].Add(a2);
            members[a2].Add(a1);

            //距離を再計算
            if (calculateDistance)
            {
                updateDistance();
            }           
        }

        public void ConnectNode(Link ln)
        {
            ConnectNodes(ln.Node1, ln.Node2);
        }


        public void DisconnectNode(INode a1, INode a2)
        {
            //エージェントがネットワークに登録されてなかったら
            if ( !members.ContainsKey(a1) || !members.ContainsKey(a2) ) {
                throw new Exception("agent is not a member of this network");
            }
            members[a1].Remove(a2);
            members[a2].Remove(a1);

            //距離を再計算
            if (calculateDistance)
            {
                updateDistance();
            }           
        }

        public void DisconnectNode(Link ln)
        {
            DisconnectNode(ln.Node1, ln.Node2);
        }


        public IEnumerable<INode> Nodes
        {
            get
            {
                foreach (var a in members.Keys)
                {
                    yield return a;
                }
            }
        }

        //使えない可能性あり
        public IEnumerable<Link> Links
        {
            get
            {
                //まずはコピーを作成
                var copied_member = new Dictionary<INode, HashSet<INode>>(members);


                foreach (var key in members.Keys)//ノードごとに走査
                {
                    copied_member[key] = new HashSet<INode>(members[key]);//コピーを作成
                }

                foreach (var key in copied_member.Keys)//ノードごとに走査
                {
                    foreach (var neighbour in copied_member[key])//選んだノードのご近所で走査
                    {
                        copied_member[neighbour].Remove(key);//ご近所さんのご近所さんからも削除
                        yield return new Link(key, neighbour);
                    }

                    //ご近所を舐め終わったら、消す。yieldreturnなのでここまで来れる。
                    copied_member[key].Clear();
                }
            }
        }
        
        public ISet<INode> GetNeighbour(INode key)
        {
            if (members.ContainsKey(key))
            {
                return members[key];
            }
            else
            {
                throw new Exception(key.ToString() + " is not a member of this network");
            }
        }

        public double ExpectedDegree
        {
            get
            {
                double sum = 0;
                foreach (var node in members.Keys)
                {
                    sum += members[node].Count();
                }

                return sum / members.Keys.Count;
            }
        }
              

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            
            writer.WriteLine("{ Network");
            writer.WriteLine("\tNodes:({0})", Nodes.Count());
            foreach (var node in Nodes)
            {
                writer.WriteLine("\t\t" + node);
            }
            writer.WriteLine();

            writer.WriteLine("\tmembers(Neighbours):");
            foreach (var key in members.Keys)
            {
                writer.WriteLine("\t\tneighbour of " + key.ToString());

                var neighbour = members[key];

                foreach (var neighbourAgent in neighbour)
                {
                    writer.WriteLine("\t\t\t" + neighbourAgent.ToString());
                }
            }


            return writer.ToString();
        }

    }
}
