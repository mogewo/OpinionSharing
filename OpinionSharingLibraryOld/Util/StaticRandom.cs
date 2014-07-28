using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionSharing.Util
{
    public static class RandomPool
    {
        static Dictionary<string, InitableRandom> dict = new Dictionary<string, InitableRandom>();

        public static void Declare(string name, int seed)
        {
            InitableRandom rand = new InitableRandom(seed);
            dict[name] = rand; //
        }

        public static InitableRandom Get(string name)
        {
            if (!dict.ContainsKey(name))
            {
                throw new Exception(name + "は宣言されていません。");
            }

            return dict[name];
        }

    }
}
