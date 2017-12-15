using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets
{
    /// <summary>
    /// This class is used for crypt/uncrypt packets or MD5
    /// </summary>
    class Cryption
    {
        internal class AdvancedXOR
        {
            public static byte[] encrypt(byte[] inputByte)
            {
                string key = "NULLABCD`abcdefghijklmnopqrstuvwxyz{|}~x/*##{}EFGHIJKLM0932N7686O1P243QRSTUVWXYZ[\\]^_";
                int keyLength = key.Length;
                int index = 3;
                for (int i = 0; i < inputByte.Length; i++)
                {
                    if (index >= keyLength) index = 3;
                    inputByte[i] = (byte)(inputByte[i] ^ key[index++]);
                }
                return inputByte;
            }

            public static byte[] decrypt(byte[] inputByte)
            {
                string key = "NULLABCDEFGHIJK3QRSTUVWXYZ[\\]^_`abcdefghijklmnopLM0932N7686O1P24qrstuvwxyz{|}~x/*##{}";
                int keyLength = key.Length;
                int index = 3;
                for (int i = 0; i < inputByte.Length; i++)
                {
                    if (index >= keyLength) index = 3;
                    inputByte[i] = (byte)(inputByte[i] ^ key[index++]);
                }
                return inputByte;
            }
        }
        internal class XOR
        {
            public static readonly byte clientXor = 0xC3;
            public static readonly byte serverXor = 0x96;

            public static byte[] encrypt(byte[] inputByte)
            {
                for (int i = 0; i < inputByte.Length; i++)
                {
                    inputByte[i] = (byte)(inputByte[i] ^ serverXor);
                }
                return inputByte;
            }

            public static byte[] decrypt(byte[] inputByte)
            {
                for (int i = 0; i < inputByte.Length; i++)
                {
                    inputByte[i] = (byte)(inputByte[i] ^ clientXor);
                }
                return inputByte;
            }
        }
    }
}
