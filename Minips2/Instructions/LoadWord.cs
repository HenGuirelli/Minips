using Minips2.Memories;
using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class LoadWord : Instruction
    {
        public LoadWord(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rs = Register.Get(iFormat.Rs);
            var immediate = iFormat.Immediate;

            var memoryValue = MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value);
            Register.SetValue(iFormat.Rt, memoryValue);
        }
    }
}
