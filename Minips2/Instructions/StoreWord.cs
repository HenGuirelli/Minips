using Minips2.Memories;
using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class StoreWord : Instruction
    {
        public StoreWord(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rs = Register.Get(iFormat.Rs);
            var rt = Register.Get(iFormat.Rt);
            var immediate = iFormat.Immediate;

            MemoryHierarchy.FirstLevel.WriteInt(immediate + rs.Value, rt.Value);
        }
    }
}
