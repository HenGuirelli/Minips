using Minips2.Registers;

namespace Minips2.Instructions
{
    internal class Xor : Instruction
    {
        public Xor(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var rd = rFormat.Rd;
            var rs = rFormat.Rs;
            var rt = rFormat.Rt;

            var result = rs ^ rt;
            Register.SetValue(rd, result);
        }
    }

    internal class Mfhi : Instruction
    {
        public Mfhi(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var result = Register.Get("hi").Value;
            var rd = rFormat.Rd;
            Register.SetValue(rd, result);
        }
    }

    internal class Mflo : Instruction
    {
        public Mflo(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var rd = rFormat.Rd;
            var result = Register.Get("lo").Value;
            Register.SetValue(rd, result);
        }
    }

    internal class Mult : Instruction
    {
        public Mult(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var rs = rFormat.Rs;
            var rt = rFormat.Rt;

            var result = Register.Get(rs).Value * Register.Get(rt).Value;
            Register.SetValue("lo", result);
        }
    }

    internal class And : Instruction
    {
        public And(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);
            var rs = rFormat.Rs;
            var rt = rFormat.Rt;
            var rd = rFormat.Rd;

            var result = Register.Get(rs).Value & Register.Get(rt).Value;
            Register.SetValue(rd, result);
        }
    }

    internal class Or : Instruction
    {
        public Or(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation); 
            var rs = rFormat.Rs;
            var rt = rFormat.Rt;
            var rd = rFormat.Rd;

            var result = Register.Get(rs).Value | Register.Get(rt).Value;
            Register.SetValue(rd, result);
        }
    }

    internal class Jarl : Instruction, IJump
    {
        public bool IsJumpInstruction { get; private set; }
        public bool IsJumped { get; private set; }

        public Jarl(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
            IsJumpInstruction = true;
        }

        public override void Exec()
        {
            var rFormat = new RFormat(InstructionRepresentation);

            var returnAddress = Register.Get("$pc").Value + 8;

            var rd = rFormat.Rd;
            Register.SetValue(rd, returnAddress);

            var nextAddress = Register.Get(rFormat.Rs).Value;
            Register.SetValue("$pc", nextAddress);
            IsJumped = true;
        }
    }
}
