namespace Minips2.Instructions
{
    internal class Addu : Instruction
    {
        public Addu(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var rsValue = Registers.Register.Get(rFormat.Rs).Value;
            var rtValue = Registers.Register.Get(rFormat.Rt).Value;

            // TODO: uint?
            //uint result = rsValue + rtValue;
            int result = rsValue + rtValue;

            Registers.Register.SetValue(rFormat.Rd, result);
        }
    }
}
