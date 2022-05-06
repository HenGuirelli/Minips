namespace Minips2.Instructions
{
    internal class Ori : Instruction
    {
        public Ori(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rt = Registers.Register.Get(iFormat.Rt);
            var rs = Registers.Register.Get(iFormat.Rs);

            Registers.Register.SetValue(rt.Number, Registers.Register.Get(rs.Number).Value | iFormat.Immediate);
        }
    }
}
