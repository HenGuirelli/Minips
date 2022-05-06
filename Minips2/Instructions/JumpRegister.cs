using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class JumpRegister : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public JumpRegister(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var nextAddress = Register.Get(rFormat.Rs).Value;
            Register.SetValue("$pc", nextAddress);
            IsJumped = true;
        }
    }
}
