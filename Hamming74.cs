using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hamming74
{
    class Hamming74
    {
        /*
        private static Random RNG = new Random();
        private const int RNGbound = 10;
        */
        public static byte[] code(byte[] data)
        {
            byte[] hamming = new byte[data.Length * 2];

            for (int i = 0; i < data.Length; ++i)
            {
                byte syn = (byte)((data[i] & 0x11) ^ ((data[i] & 0x22) >> 1) ^ ((data[i] & 0x88) >> 3)
                         | ((data[i] & 0x11) << 1) ^ ((data[i] & 0x44) >> 1) ^ ((data[i] & 0x88) >> 2)
                         | ((data[i] & 0x22) << 2) ^ ((data[i] & 0x44) << 1) ^  (data[i] & 0x88));

                hamming[i*2]   = (byte)( ((syn & 0x0F)     ) | ((data[i] & 0x01) << 2) | ((data[i] & 0x0E) << 3) );
                hamming[i*2+1] = (byte)( ((syn & 0xF0) >> 4) | ((data[i] & 0x10) >> 2) | ((data[i] & 0xE0) >> 1) );
            }

            return hamming;
        }
        
        public static byte[] decode(byte[] ham)
        {
            /*
            // Test error generator
            int rng = RNG.Next(RNGbound);
            Console.WriteLine(rng);
            if (rng == 0)
            {
                return null;
            }
            */

            byte[] data = new byte[ham.Length / 2];

            for (int i = 0; i < data.Length; ++i)
            {
                short h = (short)(ham[i*2] | ham[i*2+1] << 8);
                short syn = (short)(((h & 0x0101)     ) ^ ((h & 0x0404) >> 2) ^ ((h & 0x1010) >> 4) ^ ((h & 0x4040) >> 6)
                                  | ((h & 0x0202)     ) ^ ((h & 0x0404) >> 1) ^ ((h & 0x2020) >> 4) ^ ((h & 0x4040) >> 5)
                                  | ((h & 0x0808) >> 1) ^ ((h & 0x1010) >> 2) ^ ((h & 0x2020) >> 3) ^ ((h & 0x4040) >> 4));

                /*
                if((syn & 0x00FF) != 0)
                {
                    h = (short)(h ^ ( (0x0001 << (syn & 0x00FF)) >> 1));
                }
                if((syn & 0xFF00) != 0)
                {
                    h = (short)(h ^ ( (0x0100 << ((syn & 0xFF00) >> 8)) >> 1));
                }
                */

                // error
                if (((syn & 0x00FF) != 0) || ((syn & 0xFF00) != 0))
                {
                    return null;
                }

                data[i] = (byte)(((h & 0x0004) >> 2) | ((h & 0x0070) >> 3)
                               | ((h & 0x0400) >> 6) | ((h & 0x7000) >> 7));
            }

            return data;
        }
    }
}
