using Minips2.Memories;
using Minips2.Registers;
using System;

namespace Minips2.Instructions
{
    internal class LoadLinkedWord : Instruction
    {
        public LoadLinkedWord(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var @base = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 21, 26));
            var rt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 16, 21));
            var offset = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(InstructionRepresentation.Binary, 0, 16) + "00");

            Register.SetValue(rt, MemoryHierarchy.FirstLevel.ReadInt(offset + @base));
        }
    }

    internal class Swcl : Instruction
    {
        public Swcl(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rs = iFormat.Rs;
            var rt = Coproc1.Get(iFormat.Rt);
            var immediate = iFormat.Immediate;
            MemoryHierarchy.FirstLevel.WriteInt(immediate + rs, BitConverter.ToInt32(BitConverter.GetBytes(rt.Value)));
        }
    }

    internal class BltGez : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public BltGez(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var blezFormat = new BlezFormat(InstructionRepresentation);
            if (blezFormat.Rs >= 0)
            {
                IsJumped = true;
                var actualAddress = Register.Get("$pc").Value;
                Register.SetValue("$pc", actualAddress + blezFormat.Offset + 4);

            }
        }
    }

    internal class Blez : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public Blez(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var blezFormat = new BlezFormat(InstructionRepresentation);
            if (blezFormat.Rs <= 0)
            {
                IsJumped = true;
                var actualAddress = Register.Get("$pc").Value;
                Register.SetValue("$pc", actualAddress + blezFormat.Offset + 4);

            }
        }
    }

    internal class Beq : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public Beq(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var actualAddress = Register.Get("$pc").Value;

            var rsValue = Register.Get(iFormat.Rs).Value;
            var rtValue = Register.Get(iFormat.Rt).Value;

            if (rsValue == rtValue)
            {
                IsJumped = true;
                Register.SetValue("$pc", actualAddress + (iFormat.Immediate * 4) + 4);
            }
            else
            {
                Register.SetValue("$pc", actualAddress);
            }
        }
    }
}
