namespace Minips2.Instructions
{
    internal class Add : Instruction
    {
        public Add(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            Registers.Register.SetValue(
                rFormat.Rd,
                 Registers.Register.Get(rFormat.Rs).Value + Registers.Register.Get(rFormat.Rt).Value);
        }
    }
}
