using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class ZBase32Encoder
    {
        private const int DefaultEncodeBufferLength = 13;
        private const int DefaultDecodeBufferLength = 8;

        private const string EncodingTable = "ybndrfg8ejkmcpqxot1uwisza345h769";
        private static readonly byte[] DecodingTable = new byte[128];

        [ThreadStatic]
        private static StringBuilder encodeResult;
        [ThreadStatic]
        private static List<byte> decodeResult;

        static ZBase32Encoder()
        {
            for (var i = 0; i < DecodingTable.Length; ++i)
            {
                DecodingTable[i] = byte.MaxValue;
            }

            for (var i = 0; i < EncodingTable.Length; ++i)
            {
                DecodingTable[EncodingTable[i]] = (byte)i;
            }
        }

        public static string Encode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (encodeResult == null)
            {
                encodeResult = new StringBuilder(DefaultEncodeBufferLength);
            }
            else
            {
                encodeResult.Clear();
            }

            for (var i = 0; i < data.Length; i += 5)
            {
                var byteCount = Math.Min(5, data.Length - i);

                ulong buffer = 0;
                for (var j = 0; j < byteCount; ++j)
                {
                    buffer = (buffer << 8) | data[i + j];
                }

                var bitCount = byteCount * 8;
                while (bitCount > 0)
                {
                    var index = bitCount >= 5
                                ? (int)(buffer >> (bitCount - 5)) & 0x1f
                                : (int)(buffer & (ulong)(0x1f >> (5 - bitCount))) << (5 - bitCount);

                    encodeResult.Append(EncodingTable[index]);
                    bitCount -= 5;
                }
            }

            return encodeResult.ToString();
        }
        public static byte[] Decode(string data)
        {
            if (data == string.Empty)
            {
                return new byte[0];
            }

            if (decodeResult == null)
            {
                decodeResult = new List<byte>(DefaultDecodeBufferLength);
            }
            else
            {
                decodeResult.Clear();
            }

            var index = new int[8];
            for (var i = 0; i < data.Length; )
            {
                i = CreateIndexByOctetAndMovePosition(ref data, i, ref index);

                var shortByteCount = 0;
                ulong buffer = 0;
                for (var j = 0; j < 8 && index[j] != -1; ++j)
                {
                    buffer = (buffer << 5) | (DecodingTable[index[j]] & 0x1fUL);
                    shortByteCount++;
                }

                var bitCount = shortByteCount * 5;
                while (bitCount >= 8)
                {
                    decodeResult.Add((byte)((buffer >> (bitCount - 8)) & 0xff));
                    bitCount -= 8;
                }
            }

            return decodeResult.ToArray();
        }

        private static int CreateIndexByOctetAndMovePosition(ref string data, int currentPosition, ref int[] index)
        {
            var j = 0;
            while (j < 8)
            {
                if (currentPosition >= data.Length)
                {
                    index[j++] = -1;
                    continue;
                }

                if (IgnoredSymbol(data[currentPosition]))
                {
                    currentPosition++;
                    continue;
                }

                index[j] = data[currentPosition];
                j++;
                currentPosition++;
            }

            return currentPosition;
        }
        private static bool IgnoredSymbol(char checkedSymbol)
        {
            return checkedSymbol >= DecodingTable.Length || DecodingTable[checkedSymbol] == byte.MaxValue;
        }
    }
}
