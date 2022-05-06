using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class Bne : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public Bne(InstructionRepresentation instructionRepresentation)
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

            if (rsValue != rtValue)
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
