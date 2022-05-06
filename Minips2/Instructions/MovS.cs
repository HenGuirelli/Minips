using Minips2.Registers;
using System;

namespace Minips2.Instructions
{
    internal class MovS : Instruction
    {
        public MovS(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var fs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 11, 16));
            var fd = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 6, 11));
            var fmt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));

            if (fmt == 16)
            {
                var valueToMove = Coproc1.Get(fs).Value;
                Coproc1.SetValue(fd, valueToMove);
                return;
            }
            else if (fmt == 17)
            {
                Coproc1.SetValue(fd, Coproc1.Get(fs).Value);
                Coproc1.SetValue(fd + 1, Coproc1.Get(fs + 1).Value);
                return;
            }

            throw new NotImplementedException();
        }
    }
}
