using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class JumpAndLink : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public JumpAndLink(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var jFormat = new JFormat(InstructionRepresentation);
            Register.SetValue("$ra", Register.Get("$pc").Value + 8);
            Register.SetValue("$pc", jFormat.Address);
            IsJumped = true;
        }
    }
}
