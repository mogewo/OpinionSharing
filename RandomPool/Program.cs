using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRandom
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomPool.Declare("aiu", 0);
            RandomPool.Declare("abc", 0);

            Console.WriteLine(RandomPool.Get("aiu").Next());
            Console.WriteLine(RandomPool.Get("aiu").Next());
            Console.WriteLine(RandomPool.Get("aiu").Next());
            Console.WriteLine(RandomPool.Get("aiu").Next());

            Console.WriteLine(RandomPool.Get("abc").Next());
            Console.WriteLine(RandomPool.Get("abc").Next());
            Console.WriteLine(RandomPool.Get("abc").Next());
            Console.WriteLine(RandomPool.Get("abc").Next());
        }
    }
}
