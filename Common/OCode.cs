using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class OCode
    {
        #region private static readonly byte[] EncodingTable = new byte[]
        private static readonly byte[] EncodingTable = new byte[]
        {
            0x7A,    0x73,    0x36,    0x24,    0xC5,    0xC1,    0xB6,    0x57,
            0x09,    0xCF,    0x38,    0x15,    0xA5,    0x34,    0xD6,    0x3F,
            0x87,    0x3C,    0xA4,    0x19,    0xC8,    0xE1,    0xAC,    0xE9,
            0x92,    0x3E,    0x2A,    0xC4,    0xAA,    0x86,    0x99,    0x4F,
            0xD4,    0xB4,    0xFD,    0xA3,    0x88,    0x30,    0x14,    0x6E,
            0x18,    0xBF,    0x65,    0x49,    0x26,    0x6F,    0x7B,    0x74,
            0x35,    0x2B,    0x8C,    0x90,    0xB0,    0x79,    0x63,    0x6A,
            0xB5,    0x95,    0xF9,    0x6B,    0x02,    0xAD,    0x3A,    0x23,
            0x32,    0x5F,    0x0C,    0xFE,    0x45,    0x58,    0x5E,    0x9D,
            0x33,    0xC9,    0x13,    0x9E,    0xA1,    0xE8,    0x4D,    0x28,
            0xE7,    0xA2,    0x68,    0x43,    0x60,    0xE0,    0x61,    0x7E,
            0xFF,    0x64,    0xC7,    0x1E,    0xF3,    0x03,    0xD8,    0x66,
            0x50,    0x70,    0x53,    0x5C,    0x46,    0x31,    0x07,    0xDC,
            0xC0,    0xF2,    0xD7,    0xD2,    0x06,    0x21,    0xE6,    0xCA,
            0x81,    0x0E,    0xAB,    0xF4,    0x76,    0xA7,    0xBD,    0x42,
            0xA8,    0xDB,    0x71,    0x85,    0x72,    0xD3,    0xCB,    0x8A,
            0x47,    0x22,    0xEA,    0x82,    0x1C,    0xF6,    0xED,    0xEE,
            0xBE,    0xD1,    0x9B,    0xC6,    0x97,    0x54,    0x44,    0x80,
            0xB9,    0x6C,    0xF0,    0x62,    0x7F,    0x10,    0x89,    0x59,
            0x3B,    0x5A,    0x2D,    0x39,    0x52,    0x51,    0x4C,    0x40,
            0x2F,    0x7C,    0x93,    0x7D,    0xDF,    0xAF,    0x29,    0xCE,
            0xEC,    0xB8,    0xB1,    0x91,    0x1A,    0x77,    0x1F,    0xA0,
            0x25,    0x27,    0xC3,    0xF7,    0xD5,    0xDD,    0x83,    0x48,
            0xD0,    0x9F,    0x8F,    0x1D,    0xE5,    0xF1,    0xBA,    0x84,
            0x00,    0x67,    0x9A,    0xBC,    0x0A,    0x0D,    0xB7,    0x1B,
            0xD9,    0x75,    0xCC,    0x4E,    0x01,    0xE3,    0xC2,    0x04,
            0x4A,    0x5B,    0xEF,    0x56,    0xE2,    0x0F,    0xEB,    0x98,
            0x6D,    0x12,    0x11,    0x55,    0x9C,    0xB3,    0x41,    0xFB,
            0x16,    0xAE,    0x17,    0x69,    0x2E,    0x8D,    0xFC,    0x8B,
            0xFA,    0x08,    0x94,    0xDA,    0xA9,    0xA6,    0xCD,    0x20,
            0xF8,    0xF5,    0x78,    0x05,    0x2C,    0x5D,    0xB2,    0x3D,
            0x8E,    0x37,    0xBB,    0x0B,    0xDE,    0x4B,    0x96,    0xE4,
        };
        #endregion
        private static readonly byte[] DecodingTable;

        static OCode()
        {
            DecodingTable = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                byte b = EncodingTable[i];
                DecodingTable[b] = (byte)i;
            }
        }

        private static ulong Encode(ulong val)
        {
            // Get the 64-bit value as bytes
            byte[] bytes = BitConverter.GetBytes(val);
            // Replace each byte with the corresponding value from the encoding table, after XOR.
            byte xorValue = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte b = EncodingTable[bytes[i]];
                if (i == 0)
                {
                    xorValue = b;
                }
                else
                {
                    b ^= xorValue;
                }
                bytes[i] = b;
                ++xorValue;
            }
            return BitConverter.ToUInt64(bytes, 0);
        }
        private static ulong Decode(ulong val)
        {
            // Get the 64-bit value as bytes
            byte[] bytes = BitConverter.GetBytes(val);
            // Replace each byte with the corresponding value from the decoding table, after XOR.
            byte xorValue = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte b = bytes[i];
                if (i == 0)
                {
                    xorValue = b;
                }
                else
                {
                    b ^= xorValue;
                }
                b = DecodingTable[b];
                bytes[i] = b;
                ++xorValue;
            }
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static string Get(long? val)
        {
            if (val.HasValue)
                return Get(val.Value);
            return null;
        }
        public static string Get(ulong? val)
        {
            if (val.HasValue)
                return Get(val.Value);
            return null;
        }

        public static string Get(long val)
        {
            if (val == 0)
                throw new ArgumentOutOfRangeException(nameof(val), $"val cannot be 0");

            unchecked
            {
                var enc = Encode((ulong)val);
                return ZBase32Encoder.Encode(BitConverter.GetBytes(enc));
            }
        }
        public static string Get(ulong val)
        {
            if (val == 0)
                throw new ArgumentOutOfRangeException(nameof(val), $"val cannot be 0");

            unchecked
            {
                val = Encode(val);
                return ZBase32Encoder.Encode(BitConverter.GetBytes(val));
            }
        }
        public static long ToInt64(string ocode)
        {
            if (String.IsNullOrWhiteSpace(ocode))
                throw new ArgumentNullException(nameof(ocode));

            unchecked
            {
                var bytes = ZBase32Encoder.Decode(ocode);
                if (bytes.Length != 8)
                    throw new ArgumentException("Invalid OCode", nameof(ocode));

                var val = BitConverter.ToUInt64(bytes, 0);
                return (long)Decode(val);
            }
        }
        public static ulong ToUInt64(string ocode)
        {
            if (String.IsNullOrWhiteSpace(ocode))
                throw new ArgumentNullException("ocode");

            unchecked
            {
                var bytes = ZBase32Encoder.Decode(ocode);
                if (bytes.Length != 8)
                    throw new ArgumentException("Invalid OCode", nameof(ocode));

                var val = BitConverter.ToUInt64(bytes, 0);
                return Decode(val);
            }
        }

        public static bool TryGetInt64(string ocode, out long value)
        {
            if (!String.IsNullOrWhiteSpace(ocode))
            {
                unchecked
                {
                    var bytes = ZBase32Encoder.Decode(ocode);
                    if (bytes.Length == 8)
                    {
                        var val = BitConverter.ToUInt64(bytes, 0);
                        value = (long)Decode(val);
                        return true;
                    }
                }
            }

            value = default(long);
            return false;
        }
        public static bool TryGetUInt64(string ocode, out ulong value)
        {
            if (!String.IsNullOrWhiteSpace(ocode))
            {
                unchecked
                {
                    var bytes = ZBase32Encoder.Decode(ocode);
                    if (bytes.Length == 8)
                    {
                        var val = BitConverter.ToUInt64(bytes, 0);
                        value = Decode(val);
                        return true;
                    }
                }
            }

            value = default(ulong);
            return false;
        }

        public static string GenerateNew()
        {
            return Get(IDGenerator.GenerateID());
        }
    }
}
