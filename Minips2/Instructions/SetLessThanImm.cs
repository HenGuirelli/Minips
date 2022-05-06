using Minips2.Memories;
using Minips2.Registers;
using System;
using System.Linq;

namespace Minips2.Instructions
{
    internal class Ldcl : Instruction
    {
        public Ldcl(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation); 
            var rs = Register.Get(iFormat.Rs);
            var immediate = iFormat.Immediate;

            var int1 = MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value);
            var int2 = MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value + 4);

            var memoryValue = BitConverter.ToInt64(BitConverter.GetBytes(int1).Concat(BitConverter.GetBytes(int2)).ToArray());

            var longbytes = BitConverter.GetBytes(memoryValue);
            var doubleResult = BitConverter.ToDouble(longbytes, 0);
            Coproc1.SetValue(iFormat.Rt, doubleResult);

            var v1 = MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value);
            var v2 = MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value + 4);

            Coproc1.SetValue(iFormat.Rt, v1);
            Coproc1.SetValue(iFormat.Rt+1, v2);
        }
    }

    internal class Lwcl : Instruction
    {
        public Lwcl(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation); var rs = Register.Get(iFormat.Rs);
            var immediate = iFormat.Immediate;

            var memoryValue = MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value);

            var intbytes = BitConverter.GetBytes(memoryValue);
            var f = BitConverter.ToSingle(intbytes, 0);

            Coproc1.SetValue(iFormat.Rt, f);
        }
    }

    internal class LoadByte : Instruction
    {
        public LoadByte(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rs = Register.Get(iFormat.Rs);
            var immediate = iFormat.Immediate;

            var memoryValue = BitConverter.GetBytes(MemoryHierarchy.FirstLevel.ReadInt(immediate + rs.Value))[0];
            Register.SetValue(iFormat.Rt, memoryValue);
        }
    }

    internal class SetLessThanImm : Instruction
    {
        public SetLessThanImm(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var iFormat = new IFormat(InstructionRepresentation);
            var rsValue = Register.Get(iFormat.Rs).Value;
            Register.SetValue(iFormat.Rt, rsValue < iFormat.Immediate ? 1 : 0);
        }
    }
}
