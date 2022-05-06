using Minips2.Registers;
using System;

namespace Minips2.Instructions
{
    internal class C_Eq : Instruction, IJump
    {
        public bool IsJumpInstruction { get;set; }
        public bool IsJumped { get; set; }

        public C_Eq(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var bc = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));
            var cc = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 18, 21));
            var offset = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 0, 16) + "00");

            if (ConditonFlags.Get(cc) == true)
            {
                IsJumped = true;
                var actualAddress = Register.Get("$pc").Value;
                Register.SetValue("$pc", actualAddress + offset + 4);
            }
        }
    }

    internal class C_lt : Instruction
    {
        public C_lt(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var ft = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 16, 21));
            var fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 11, 16));
            var fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 6, 11));
            var fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));
            var cond = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 0, 4));
            var cc = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 8, 11));

            // c.s
            if (fmt == 16)
            {
                // lt = less than
                if (cond == 12)
                {
                    ConditonFlags.Set(cc, fs < ft);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            // c.d
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    internal class FloatMul : Instruction
    {
        public FloatMul(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var ft = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 16, 21));
            var fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 11, 16));
            var fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 6, 11));
            var fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));

            // mul.s
            if (fmt == 16)
            {
                Coproc1.SetValue(fd, Coproc1.Get(fs).Value * Coproc1.Get(ft).Value);
            }
            // mul.d
            else
            {
                var result = Coproc1.GetDoubleValue(fs) * Coproc1.GetDoubleValue(ft);
                Coproc1.SetValue(fd, result);
            }
        }
    }
}
