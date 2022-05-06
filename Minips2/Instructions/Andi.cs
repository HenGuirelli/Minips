namespace Minips2.Instructions
{
    internal class Andi : Instruction
    {
        public Andi(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);

            var rs = Registers.Register.Get(iFormat.Rs);
            var andi = rs.Value & iFormat.Immediate;

            Registers.Register.SetValue(iFormat.Rt, andi);
        }
    }
}
