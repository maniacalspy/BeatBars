using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class RandomQueue
    {
        static Random random = new Random();



        public static int GetRandomInt(int max)
        {
            return random.Next(max);
        }
    }
}
