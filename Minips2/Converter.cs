using System;
using System.Text;

namespace Minips2
{
    internal class Converter
    {
        public static string Int32StringToBinaryString(int value, int padSize = 32)
        {
            return Convert.ToString(value, 2).PadLeft(padSize, '0');
        }

        public static int BinaryStringToInt32_2Complement(string value)
        {
            value = value.PadLeft(32, value[0]);
            return Convert.ToInt32(value, 2);
        }

        public static int BinaryStringToInt32(string value)
        {
            return Convert.ToInt32(value, 2);
        }

        public static string GetSignificativeBits(string bits, int start, int end)
        {
            var realStart = Math.Abs(start - 32);
            var realEnd = Math.Abs(end - 32);
            return bits[realEnd..realStart];
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
