using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class ShiftLeftLogical : Instruction
    {
        public ShiftLeftLogical(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var rd = Register.Get(rFormat.Rd);
            var rt = Register.Get(rFormat.Rt);
            var shamt = Register.Get(rFormat.Shamt);

            var shiftResult = rt.Value << shamt.Number;
            Register.SetValue(rd.Number, shiftResult);
        }
    }
}
