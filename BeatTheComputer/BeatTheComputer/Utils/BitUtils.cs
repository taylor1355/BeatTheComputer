using System.Collections.Generic;

namespace BeatTheComputer.Utils
{
    class BitUtils
    {
        // a mapping of powers of 2 to their base 2 logarithms (1->0, 2->1, 4->2, ...)
        private static Dictionary<ulong, int> powersOf2Log;

        static BitUtils()
        {
            powersOf2Log = new Dictionary<ulong, int>();

            ulong powerOf2 = 0x1;
            int log = 0;
            while (powerOf2 > 0) {
                powersOf2Log.Add(powerOf2, log);
                powerOf2 <<= 1;
                log++;
            }
        }

        private BitUtils() { }

        public static int nthBit(int n, ulong bitVector)
        {
            return (int) (bitVector >> n) & 0x1;
        }

        public static List<int> getSetBits(ulong bitVector)
        {
            List<int> setBits = new List<int>();

            while (bitVector > 0) {
                // get position of the least significant set bit
                setBits.Add(powersOf2Log[bitVector & ~(bitVector - 1)]);

                // reset least significant set bit
                bitVector = bitVector & (bitVector - 1);
            }

            return setBits;
        }

        public static void setNthBit(int n, ref ulong bitVector)
        {
            bitVector = bitVector | (0x1uL << n);
        }

        public static void resetNthBit(int n, ref ulong bitVector)
        {
            bitVector = bitVector & ~(0x1uL << n);
        }
    }
}
