using Minips2.Registers;
using System;

namespace Minips2.Instructions
{
    internal class Sra : Instruction
    {
        public Sra(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var rsValue = Register.Get(rFormat.Rs).Value;
            var sa = Register.Get(rFormat.Shamt).Value;

            Register.SetValue(rFormat.Rd, rsValue >> sa);
        }
    }

    internal class Subu : Instruction
    {
        public Subu(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var rsValue = BitConverter.ToUInt32(BitConverter.GetBytes(Register.Get(rFormat.Rs).Value));
            var rtValue = BitConverter.ToUInt32(BitConverter.GetBytes(Register.Get(rFormat.Rt).Value));

            Register.SetValue(rFormat.Rd, BitConverter.ToInt32(BitConverter.GetBytes(rsValue - rtValue)));
        }
    }

    internal class SetLTUnsigned : Instruction
    {
        public SetLTUnsigned(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var rsValue = BitConverter.ToUInt32(BitConverter.GetBytes(Register.Get(rFormat.Rs).Value));
            var rtValue = BitConverter.ToUInt32(BitConverter.GetBytes(Register.Get(rFormat.Rt).Value));

            if (rsValue < rtValue) Register.SetValue(rFormat.Rd, 1);
            else Register.SetValue(rFormat.Rd, 0);
        }
    }

    internal class SetLT : Instruction
    {
        public SetLT(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var rsValue = Register.Get(rFormat.Rs).Value;
            var rtValue = Register.Get(rFormat.Rt).Value;

            if (rsValue < rtValue) Register.SetValue(rFormat.Rd, 1);
            else Register.SetValue(rFormat.Rd, 0);
        }
    }
}
