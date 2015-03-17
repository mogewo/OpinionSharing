using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphTheory.Net;

namespace GraphTheory
{
    class Program
    {
        static void Main(string[] args)
        {
            MyRandom.RandomPool.Declare("envset",0);
            //var gen = new BAModelNetworkGenerator(100, 4 , 3);
            var gen = new RandomNetworkGenerator(100, 10, 0.4);
            gen.NodeCreate = () => new Node();
            var net = gen.create();
            Console.WriteLine(net);

        }
    }
}
