namespace Minips2.Instructions
{
    internal class Addiu : Instruction
    {
        public Addiu(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rsValue = Registers.Register.Get(iFormat.Rs).Value;
            Registers.Register.SetValue(iFormat.Rt, rsValue + iFormat.Immediate);
        }
    }
}
