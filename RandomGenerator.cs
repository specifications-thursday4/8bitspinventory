using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _8BitIMS
{
    // makes unsigned 32 bit random numbers for game ID, DB accepts 64bit
    class RandomGenerator
    {
        private static readonly RandomNumberGenerator generator;

        static RandomGenerator()
        {
            generator = RandomNumberGenerator.Create();

        }
        public static int GetNext()
        {
            byte[] rndArray = new byte[4];
            generator.GetBytes(rndArray);
            return BitConverter.ToInt32(rndArray, 0);
        }

        public static uint GetNextUnsigned()
        {
            byte[] rndArray = new byte[4];
            generator.GetBytes(rndArray);
            return BitConverter.ToUInt32(rndArray, 0);
        }
    }
    
}
