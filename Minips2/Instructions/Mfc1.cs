using Minips2.Registers;
using System;
using System.Linq;

namespace Minips2.Instructions
{
    internal class CvtS : Instruction
    {
        public CvtS(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var ft = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 16, 21));
            var fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 11, 16));
            var fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 6, 11));
            var fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));
            // cvt.s.w FRdest, FRsrcConvert Integer to Double
            if (fmt == 20)
            {
                var floatValue = Coproc1.Get(fs).Value;
                var @float = (float)BitConverter.ToInt32(BitConverter.GetBytes(floatValue));

                Coproc1.SetValue(fd, @float);
            }
            // cvt.s.d 
            if (fmt == 17)
            {
                var float1 = Coproc1.Get(fs).Value;
                var float2 = Coproc1.Get(fs + 1).Value;
                var @float = (float)BitConverter.ToDouble(BitConverter.GetBytes(float1).Concat(BitConverter.GetBytes(float2)).ToArray());

                Coproc1.SetValue(fd, @float);
            }
        }
    }

    internal class CvtD : Instruction
    {
        public CvtD(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var ft = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 16, 21));
            var fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 11, 16));
            var fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 6, 11));
            var fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));
            // cvt.d.w FRdest, FRsrcConvert Integer to Double
            if (fmt == 20)
            {
                var floatValue = Coproc1.Get(fs).Value;
                var floatValue2 = Coproc1.Get(fs + 1).Value;
                var doubleResult = BitConverter.ToDouble(BitConverter.GetBytes(floatValue).Concat(BitConverter.GetBytes(floatValue2)).ToArray());

                var integerReprentation = BitConverter.ToInt64(BitConverter.GetBytes(doubleResult));
                var float1 = BitConverter.ToSingle(BitConverter.GetBytes((double)BitConverter.ToSingle(BitConverter.GetBytes((float)integerReprentation))), 0);
                var float2 = BitConverter.ToSingle(BitConverter.GetBytes((double)BitConverter.ToSingle(BitConverter.GetBytes((float)integerReprentation))), 4);

                Coproc1.SetValue(fd, float1);
                Coproc1.SetValue(fd + 1, float2);
            }
            // cvt.d.s FRdest, FRsrcConvert Single to Double
            if (fmt == 10)
            {
                throw new Exception();
            }
        }
    }

    internal class AddFloat : Instruction
    {
        public AddFloat(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var opcode = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 26, 32));
            var ft = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 16, 21));
            var fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 11, 16));
            var fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 6, 11));
            var fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));

            if (fmt == 16)
            {
                var result = Coproc1.Get(fs).Value + Coproc1.Get(ft).Value;
                Coproc1.SetValue(fd, result);
            }
            else if (fmt == 17)
            {
                var fs1 = Coproc1.Get(fs).Value;
                var fs2 = Coproc1.Get(fs + 1).Value;
                var d1 = BitConverter.ToDouble(BitConverter.GetBytes(fs1).Concat(BitConverter.GetBytes(fs2)).ToArray());


                var ft1 = Coproc1.Get(ft).Value;
                var ft2 = Coproc1.Get(ft + 1).Value;
                var d2 = BitConverter.ToDouble(BitConverter.GetBytes(ft1).Concat(BitConverter.GetBytes(ft2)).ToArray());

                var result = d1 + d2;
                var resultBytes = BitConverter.GetBytes(result);
                Coproc1.SetValue(fd, BitConverter.ToSingle(resultBytes, 0));
                Coproc1.SetValue(fd + 1, BitConverter.ToSingle(resultBytes, resultBytes.Length / 2));
            }
            else
            {
                throw new Exception();
            }
        }
    }

    internal class Mtc1 : Instruction
    {
        public Mtc1(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var floatFormat = new FloatFormat(InstructionRepresentation);
            var floatValue = Register.Get(floatFormat.Rt).Value;
            var intValue = BitConverter.ToSingle(BitConverter.GetBytes(floatValue));
            Coproc1.SetValue(floatFormat.Fs, intValue);
        }
    }

    internal class Mfc1 : Instruction
    {
        public Mfc1(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var floatFormat = new FloatFormat(InstructionRepresentation);
            var floatValue = Coproc1.Get(floatFormat.Fs).Value;
            var intValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue));
            Register.SetValue(floatFormat.Rt, intValue);
        }
    }
}
