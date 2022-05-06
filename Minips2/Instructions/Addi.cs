namespace Minips2.Instructions
{
    internal class Addi : Instruction
    {
        private readonly string _binarystring;

        public Addi(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            _binarystring = instructionRepresentation.Binary;
        }

        public override void Exec()
        {
            var immediate = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(_binarystring, 0, 16));
            var rt = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(_binarystring, 16, 21));
            var rs = Converter.BinaryStringToInt32(Converter.GetSignificativeBits(_binarystring, 21, 26));

            Registers.Register.SetValue(rt, Registers.Register.Get(rs).Value + immediate);
        }
    }
}
