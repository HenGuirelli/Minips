namespace Minips2.Instructions
{
    internal class JFormat
    {
        public int Opcode { get; }
        public int Address { get; }

        public JFormat(InstructionRepresentation instructionRepresentation)
        {
            var binFormat = instructionRepresentation.Binary;
            Opcode = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 26, 32));
            Address = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 0, 26) + "00");
        }
    }

    internal class BlezFormat
    {
        public int Opcode { get; }
        public int Rs { get; }
        public int Offset { get; }

        public BlezFormat(InstructionRepresentation instructionRepresentation)
        {
            var binFormat = instructionRepresentation.Binary;
            Opcode = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 26, 32));
            Rs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 21, 26));
            Offset = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 0, 16) + "00");
        }
    }

    internal class FloatFormat2
    {
        public int Ft { get; }
        public int Fs { get; }
        public int Fd { get; }
        public int Fmt { get; }
        public bool IsSingle => Fmt == 16;

        public FloatFormat2(InstructionRepresentation instructionRepresentation)
        {
            Fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(instructionRepresentation.Binary, 21, 26));
            Ft = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(instructionRepresentation.Binary, 16, 21));
            Fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(instructionRepresentation.Binary, 11, 16));
            Fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(instructionRepresentation.Binary, 6, 11));
        }
    }

    internal class FloatFormat
    {
        public int Opcode { get; }
        public int Mf { get; }
        public int Rt { get; }
        public int Fs { get; }

        public FloatFormat(InstructionRepresentation instructionRepresentation)
        {
            var binFormat = instructionRepresentation.Binary;
            Opcode = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 26, 32));
            Mf = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 21, 26));
            Rt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 16, 21));
            Fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 11, 16));
        }
    }

    internal class IFormat
    {
        public int Opcode { get; }
        public int Rs { get; }
        public int Rt { get; }
        public int Immediate { get; }

        public IFormat(InstructionRepresentation instructionRepresentation)
        {
            var binFormat = instructionRepresentation.Binary;
            Opcode = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 26, 32));
            Rs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 21, 26));
            Rt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 16, 21));
            Immediate = Converter.BinaryStringToInt32_2Complement(Converter.GetSignificativeBits(binFormat, 0, 16));
        }
    }

    internal class RFormat
    {
        public int Opcode { get; }
        public int Rs { get; }
        public int Rt { get; }
        public int Rd { get; }
        public int Shamt { get; }
        public int Funct { get; }

        public RFormat(InstructionRepresentation instructionRepresentation)
        {
            var binFormat = instructionRepresentation.Binary;
            Opcode = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 26, 32));
            Rs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 21, 26));
            Rt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 16, 21));
            Rd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 11, 16));
            Shamt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 6, 11));
            Funct = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(binFormat, 0, 6));
        }
    }
}
