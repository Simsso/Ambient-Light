using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientLight
{
    static class MapExtension
    {
        public static byte Map(this byte x, byte inMin, byte inMax, byte outMin, byte outMax)
        {
            return (byte)((int)x).Map(inMin, inMax, outMin, outMax);
        }

        public static int Map(this int x, int inMin, int inMax, int outMin, int outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        public static long Map(this long x, long inMin, long inMax, long outMin, long outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}
