using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpinionSharing;
using OpinionSharing.Agt;
using OpinionSharing.Agt.Factory;
using OpinionSharing.Agt.Algorithm;
using MyRandom;
using GraphTheory.Net;
using OpinionSharing.Env;

namespace Test
{
    public class Program
    {
        static Node createNode()
        {
            return new Node();
        }

        static void Main(string[] args)
        {
            RandomPool.Declare("envset", 0);
            //RandomPool.Get();
            
            
            NetworkGenerator generator = new WSmodelNetworkGenerator(100, 8, 0.12);
            generator.NodeCreate += () => new Node();
            //          =  generator.NodeCreate += createNode;
            Network net = generator.create();
            
            //Console.WriteLine(net);

            //NetworkGenerator generator2 = new LeaderNetworkGenerator(100, 8, 0.12, 30);
            //環境を作る            
            //generator2.NodeCreate += () => new Node();
            //Network leaderNet = generator2.create();             
            //double c = LeaderNetworkGenerator.cluster(leaderNet.Nodes.ElementAt(0),leaderNet);             
            //Console.WriteLine(leaderNet);

            for (int i = 0; i < net.Nodes.Count(); i++)
            {
                double C = WSmodelNetworkGenerator.cluster(net.Nodes.ElementAt(i), net);
                Console.Write("clusters{0} = ", i);
                Console.WriteLine( + C);
            }
            

        }
    }
}
