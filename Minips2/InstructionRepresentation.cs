using System;
using System.Linq;

namespace Minips2
{
    public class InstructionRepresentation
    {
        public string Binary { get; }
        public string Hex { get; }
        public byte[] Bytes { get; }

        public InstructionRepresentation(string binary, string hex)
        {
            Binary = binary;
            Hex = hex;
            Bytes = StringToByteArray(hex);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
