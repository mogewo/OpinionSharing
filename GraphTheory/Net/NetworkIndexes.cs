using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphTheory.Net
{
    public class NetworkIndexes
    {

        //nodeのクラスタ係数を算出
        public static double cluster(INode node, Network net)
        {
            int nodeID = node.ID;//nodeの固定
            int k = node.Neighbours.Count;//次数
            int triangleNum = 0;//三角形の総数
            double c = 0.0;//クラスタ係数   

            //

            //近隣エージェントは別になるように
            for (int i = 0; i < node.Neighbours.Count; i++)
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
    }
}
