namespace Minips2.Instructions
{
    internal class Lui : Instruction
    {
        public Lui(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var binary = Converter.Int32StringToBinaryString(iFormat.Immediate, 16);
            var intRepresentation = Converter.BinaryStringToInt32(binary.PadRight(32, '0'));

            Registers.Register.SetValue(iFormat.Rt, intRepresentation);
        }
    }
}
