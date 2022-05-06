using Minips2.Registers;
using System;

namespace Minips2.Instructions
{
    internal class FloatDiv : Instruction
    {
        public FloatDiv(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var floatFormat = new FloatFormat2(InstructionRepresentation);
            // single
            if (floatFormat.Fmt == 16)
            {
                var valueFs = Coproc1.Get(floatFormat.Fs).Value;
                var valueFt = Coproc1.Get(floatFormat.Ft).Value;
                Coproc1.SetValue(floatFormat.Fd, valueFs / valueFt);
            }
            // double
            else
            {
                var valueFs = Coproc1.GetDoubleValue(floatFormat.Fs);
                var valueFt = Coproc1.GetDoubleValue(floatFormat.Ft);
                Coproc1.SetValue(floatFormat.Fd, valueFs / valueFt);
            }
        }
    }

    internal class Div : Instruction
    {
        public Div(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var rs = rFormat.Rs;
            var rt = rFormat.Rt;

            var resultqo = Registers.Register.Get(rs).Value / Registers.Register.Get(rt).Value;
            Registers.Register.SetValue("lo", resultqo);

            var resultmod = Registers.Register.Get(rs).Value % Registers.Register.Get(rt).Value;
            Registers.Register.SetValue("hi", resultmod);
        }
    }
}
