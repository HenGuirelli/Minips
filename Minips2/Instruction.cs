using Minips2.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minips2
{
    public enum FlPtFormatOp
    {
        Mfc1,
        Cfc1,
        Mtc1,
        Ctc1,
        Mov,
        Add,
        Sub,
        Mul,
        Div,
        Abs,
        Neg,
        CvtS,
        CvtD,
        CvtW,
        C_f,
        C_Un,
        C_Eq,
        C_Ueq,
        C_Olt,
        C_Ult,
        C_Ole,
        C_Ule,
        C_sf,
        C_ngle,
        C_seq,
        C_ngl,
        C_lt,
        C_nge,
        C_le,
        C_ngt
    }

    public interface IJump
    {
        public bool IsJumpInstruction { get; }
        bool IsJumped { get; }
    }

    public abstract class Instruction : IInstruction
    {
        private readonly static Dictionary<string, Dictionary<string, OperationType>> _opType = new();
        private readonly static Dictionary<string, Dictionary<string, RFormatOp>> _RFormatOpType = new();
        private readonly static Dictionary<string, Dictionary<string, FlPtFormatOp>> _FlPtFormatOpType = new();
        public InstructionRepresentation InstructionRepresentation { get; protected set; }

        static Instruction()
        {
            PopulateOpType();
            PopulateRFormatType();
            PopulateFlPtFormatType();
        }

        public static Instruction FromString(string instruction)
        {
            var instructionRepresentation = new InstructionRepresentation(
                string.Join(string.Empty,
                    instruction.Select(
                        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                    )
            ), instruction);

            OperationType operationType = GetOperationType(instructionRepresentation);
            switch (operationType)
            {
                case OperationType.AddImmediate:
                    {
                        return new Addi(instructionRepresentation);
                    }
                case OperationType.Rformat:
                    {
                        var RFormatOp = GetRFormatOp(instructionRepresentation);
                        return GetRFormatInstuction(RFormatOp, instructionRepresentation);
                    }
                case OperationType.LoadUpperImmediate:
                    {
                        return new Lui(instructionRepresentation);
                    }
                case OperationType.Ori:
                    {
                        return new Ori(instructionRepresentation);
                    }
                case OperationType.Addiu:
                    {
                        return new Addiu(instructionRepresentation);
                    }
                case OperationType.Andi:
                    {
                        return new Andi(instructionRepresentation);
                    }
                case OperationType.BranchEQ:
                    {
                        return new Beq(instructionRepresentation);
                    }
                case OperationType.BranchNE:
                    {
                        return new Bne(instructionRepresentation);
                    }
                case OperationType.Jump:
                    {
                        return new Jump(instructionRepresentation);
                    }
                case OperationType.JumpAndLink:
                    {
                        return new JumpAndLink(instructionRepresentation);
                    }
                case OperationType.LoadWord:
                    {
                        return new LoadWord(instructionRepresentation);
                    }
                case OperationType.StoreWord:
                    {
                        return new StoreWord(instructionRepresentation);
                    }
                case OperationType.SetLessThanImm:
                    {
                        return new SetLessThanImm(instructionRepresentation);
                    }
                case OperationType.LoadByte:
                    {
                        return new LoadByte(instructionRepresentation);
                    }
                case OperationType.Lwcl:
                    {
                        return new Lwcl(instructionRepresentation);
                    }
                case OperationType.FlPt:
                    {
                        FlPtFormatOp FlPtFormatOp = GetFlPtFormatOp(instructionRepresentation);
                        return GetFlPtFormatInstuction(FlPtFormatOp, instructionRepresentation);
                    }
                case OperationType.Ldcl:
                    {
                        return new Ldcl(instructionRepresentation);
                    }
                case OperationType.Blez:
                    {
                        return new Blez(instructionRepresentation);
                    }
                case OperationType.BltGez:
                    {
                        return new BltGez(instructionRepresentation);
                    }
                case OperationType.Swcl:
                    {
                        return new Swcl(instructionRepresentation);
                    }
                case OperationType.LoadLinkedWord:
                    {
                        return new LoadLinkedWord(instructionRepresentation);
                    }
            }

            throw new NotImplementedException($"{operationType} not implemented!");
        }

        private static Instruction GetFlPtFormatInstuction(FlPtFormatOp flPtFormatOp, InstructionRepresentation instructionRepresentation)
        {
            switch (flPtFormatOp)
            {
                case FlPtFormatOp.Mfc1:
                    {
                        return new Mfc1(instructionRepresentation);
                    }
                case FlPtFormatOp.Mov:
                    {
                        return new MovS(instructionRepresentation);
                    }
                case FlPtFormatOp.Mtc1:
                    {
                        return new Mtc1(instructionRepresentation);
                    }
                case FlPtFormatOp.Add:
                    {
                        return new AddFloat(instructionRepresentation);
                    }
                case FlPtFormatOp.CvtD:
                    {
                        return new CvtD(instructionRepresentation);
                    }
                case FlPtFormatOp.CvtS:
                    {
                        return new CvtS(instructionRepresentation);
                    }
                case FlPtFormatOp.Div:
                    {
                        return new FloatDiv(instructionRepresentation);
                    }
                case FlPtFormatOp.Mul:
                    {
                        return new FloatMul(instructionRepresentation);
                    }
                case FlPtFormatOp.C_lt:
                    {
                        return new C_lt(instructionRepresentation);
                    }
                case FlPtFormatOp.C_Eq:
                    {
                        return new C_Eq(instructionRepresentation);
                    }
            }

            throw new NotImplementedException($"{flPtFormatOp} not implemented!");
        }

        private static FlPtFormatOp GetFlPtFormatOp(InstructionRepresentation instructionRepresentation)
        {
            if (_FlPtFormatOpType.TryGetValue(Converter.GetSignificativeBits(instructionRepresentation.Binary, 24, 26), out var result3))
            {
                if (result3.TryGetValue(Converter.GetSignificativeBits(instructionRepresentation.Binary, 21, 24), out var result2))
                {
                    return result2;
                }
            }

            if (_FlPtFormatOpType.TryGetValue(Converter.GetSignificativeBits(instructionRepresentation.Binary, 3, 6), out var result))
            {
                if (result.TryGetValue(Converter.GetSignificativeBits(instructionRepresentation.Binary, 0, 3), out var result2))
                {
                    return result2;
                }
            }

            throw new NotImplementedException();
        }

        private static Instruction GetRFormatInstuction(RFormatOp rFormatOp, InstructionRepresentation instructionRepresentation)
        {
            switch (rFormatOp)
            {
                case RFormatOp.Add:
                    {
                        return new Add(instructionRepresentation);
                    }
                case RFormatOp.Syscall:
                    {
                        return new Syscall(instructionRepresentation);
                    }
                case RFormatOp.Addu:
                    {
                        return new Addu(instructionRepresentation);
                    }
                case RFormatOp.Div:
                    {
                        return new Div(instructionRepresentation);
                    }
                case RFormatOp.SetLT:
                    {
                        return new SetLT(instructionRepresentation);
                    }
                case RFormatOp.JumpRegister:
                    {
                        return new JumpRegister(instructionRepresentation);
                    }
                case RFormatOp.ShiftRightLogical:
                    {
                        return new ShiftRightLogical(instructionRepresentation);
                    }
                case RFormatOp.ShiftLeftLogical:
                    {
                        return new ShiftLeftLogical(instructionRepresentation);
                    }
                case RFormatOp.Jarl:
                    {
                        return new Jarl(instructionRepresentation);
                    }
                case RFormatOp.Or:
                    {
                        return new Or(instructionRepresentation);
                    }
                case RFormatOp.Mult:
                    {
                        return new Mult(instructionRepresentation);
                    }
                case RFormatOp.Mflo:
                    {
                        return new Mflo(instructionRepresentation);
                    }
                case RFormatOp.Mfhi:
                    {
                        return new Mfhi(instructionRepresentation);
                    }
                case RFormatOp.Xor:
                    {
                        return new Xor(instructionRepresentation);
                    }
                case RFormatOp.And:
                    {
                        return new And(instructionRepresentation);
                    }
                case RFormatOp.SetLTUnsigned:
                    {
                        return new SetLTUnsigned(instructionRepresentation);
                    }
                case RFormatOp.Subu:
                    {
                        return new Subu(instructionRepresentation);
                    }
                case RFormatOp.Sra:
                    {
                        return new Sra(instructionRepresentation);
                    }
            }

            throw new NotImplementedException($"{rFormatOp} not implemented!");
        }

        private static RFormatOp GetRFormatOp(InstructionRepresentation instructionRepresentation)
        {
            return _RFormatOpType
                [Converter.GetSignificativeBits(instructionRepresentation.Binary, 3, 6)]
                [Converter.GetSignificativeBits(instructionRepresentation.Binary, 0, 3)];
        }

        private static OperationType GetOperationType(InstructionRepresentation InstructionRepresentation)
        {
            return _opType[InstructionRepresentation.Binary[0..3]][InstructionRepresentation.Binary[3..6]];
        }

        private static void PopulateFlPtFormatType()
        {
            _FlPtFormatOpType["00"] = new();
            _FlPtFormatOpType["00"]["000"] = FlPtFormatOp.Mfc1;
            _FlPtFormatOpType["00"]["010"] = FlPtFormatOp.Cfc1;
            _FlPtFormatOpType["00"]["100"] = FlPtFormatOp.Mtc1;
            _FlPtFormatOpType["00"]["110"] = FlPtFormatOp.Ctc1;


            _FlPtFormatOpType["000"] = new();
            _FlPtFormatOpType["000"]["000"] = FlPtFormatOp.Add;
            _FlPtFormatOpType["000"]["001"] = FlPtFormatOp.Sub;
            _FlPtFormatOpType["000"]["010"] = FlPtFormatOp.Mul;
            _FlPtFormatOpType["000"]["011"] = FlPtFormatOp.Div;
            _FlPtFormatOpType["000"]["101"] = FlPtFormatOp.Abs;
            _FlPtFormatOpType["000"]["110"] = FlPtFormatOp.Mov;
            _FlPtFormatOpType["000"]["111"] = FlPtFormatOp.Neg;

            _FlPtFormatOpType["100"] = new();
            _FlPtFormatOpType["100"]["000"] = FlPtFormatOp.CvtS;
            _FlPtFormatOpType["100"]["001"] = FlPtFormatOp.CvtD;
            _FlPtFormatOpType["100"]["100"] = FlPtFormatOp.CvtW;

            _FlPtFormatOpType["110"] = new();
            _FlPtFormatOpType["110"]["000"] = FlPtFormatOp.C_f;
            _FlPtFormatOpType["110"]["001"] = FlPtFormatOp.C_Un;
            _FlPtFormatOpType["110"]["010"] = FlPtFormatOp.C_Eq;
            _FlPtFormatOpType["110"]["011"] = FlPtFormatOp.C_Ueq;
            _FlPtFormatOpType["110"]["100"] = FlPtFormatOp.C_Olt;
            _FlPtFormatOpType["110"]["101"] = FlPtFormatOp.C_Ult;
            _FlPtFormatOpType["110"]["110"] = FlPtFormatOp.C_Ole;
            _FlPtFormatOpType["110"]["111"] = FlPtFormatOp.C_Ule;

            _FlPtFormatOpType["111"] = new();
            _FlPtFormatOpType["111"]["000"] = FlPtFormatOp.C_sf;
            _FlPtFormatOpType["111"]["001"] = FlPtFormatOp.C_ngle;
            _FlPtFormatOpType["111"]["010"] = FlPtFormatOp.C_seq;
            _FlPtFormatOpType["111"]["011"] = FlPtFormatOp.C_ngl;
            _FlPtFormatOpType["111"]["100"] = FlPtFormatOp.C_lt;
            _FlPtFormatOpType["111"]["101"] = FlPtFormatOp.C_nge;
            _FlPtFormatOpType["111"]["110"] = FlPtFormatOp.C_le;
            _FlPtFormatOpType["111"]["111"] = FlPtFormatOp.C_ngt;
        }

        private static void PopulateRFormatType()
        {
            // Primeira linha RFormat
            _RFormatOpType["000"] = new();
            _RFormatOpType["000"]["000"] = RFormatOp.ShiftLeftLogical;
            _RFormatOpType["000"]["010"] = RFormatOp.ShiftRightLogical;
            _RFormatOpType["000"]["011"] = RFormatOp.Sra;
            _RFormatOpType["000"]["100"] = RFormatOp.Sllv;
            _RFormatOpType["000"]["110"] = RFormatOp.Srlv;
            _RFormatOpType["000"]["111"] = RFormatOp.Srav;

            // Segunda linha RFormat
            _RFormatOpType["001"] = new();
            _RFormatOpType["001"]["000"] = RFormatOp.JumpRegister;
            _RFormatOpType["001"]["001"] = RFormatOp.Jarl;
            _RFormatOpType["001"]["100"] = RFormatOp.Syscall;
            _RFormatOpType["001"]["101"] = RFormatOp.Break;


            // Terceira linha RFormat
            _RFormatOpType["010"] = new();
            _RFormatOpType["010"]["000"] = RFormatOp.Mfhi;
            _RFormatOpType["010"]["001"] = RFormatOp.Mthi;
            _RFormatOpType["010"]["010"] = RFormatOp.Mflo;
            _RFormatOpType["010"]["011"] = RFormatOp.Mtlo;

            // Quarta linha RFormat
            _RFormatOpType["011"] = new();
            _RFormatOpType["011"]["000"] = RFormatOp.Mult;
            _RFormatOpType["011"]["001"] = RFormatOp.Multu;
            _RFormatOpType["011"]["010"] = RFormatOp.Div;
            _RFormatOpType["011"]["011"] = RFormatOp.Divu;

            // Quinta linha RFormat
            _RFormatOpType["100"] = new();
            _RFormatOpType["100"]["000"] = RFormatOp.Add;
            _RFormatOpType["100"]["001"] = RFormatOp.Addu;
            _RFormatOpType["100"]["010"] = RFormatOp.Subtract;
            _RFormatOpType["100"]["011"] = RFormatOp.Subu;
            _RFormatOpType["100"]["100"] = RFormatOp.And;
            _RFormatOpType["100"]["101"] = RFormatOp.Or;
            _RFormatOpType["100"]["110"] = RFormatOp.Xor;
            _RFormatOpType["100"]["111"] = RFormatOp.Nor;

            // Sexta linha RFormat
            _RFormatOpType["101"] = new();
            _RFormatOpType["101"]["010"] = RFormatOp.SetLT;
            _RFormatOpType["101"]["011"] = RFormatOp.SetLTUnsigned;
        }

        private static void PopulateOpType()
        {
            // Primeira linha da tabela
            _opType["000"] = new();
            _opType["000"]["000"] = OperationType.Rformat;
            _opType["000"]["001"] = OperationType.BltGez;
            _opType["000"]["010"] = OperationType.Jump;
            _opType["000"]["011"] = OperationType.JumpAndLink;
            _opType["000"]["100"] = OperationType.BranchEQ;
            _opType["000"]["101"] = OperationType.BranchNE;
            _opType["000"]["110"] = OperationType.Blez;
            _opType["000"]["111"] = OperationType.BGTZ;


            // Segunda linha da tabela
            _opType["001"] = new();
            _opType["001"]["000"] = OperationType.AddImmediate;
            _opType["001"]["001"] = OperationType.Addiu;
            _opType["001"]["010"] = OperationType.SetLessThanImm;
            _opType["001"]["011"] = OperationType.SetLessThanImmUnsigned;
            _opType["001"]["100"] = OperationType.Andi;
            _opType["001"]["101"] = OperationType.Ori;
            _opType["001"]["110"] = OperationType.Xori;
            _opType["001"]["111"] = OperationType.LoadUpperImmediate;

            // Terceira linha da tabela
            _opType["010"] = new();
            _opType["010"]["000"] = OperationType.TBL;
            _opType["010"]["001"] = OperationType.FlPt;


            // Quinta linha da tabela
            _opType["100"] = new();
            _opType["100"]["000"] = OperationType.LoadByte;
            _opType["100"]["001"] = OperationType.LoadHalf;
            _opType["100"]["010"] = OperationType.Lwl;
            _opType["100"]["011"] = OperationType.LoadWord;
            _opType["100"]["100"] = OperationType.LoadByteUnsigned;
            _opType["100"]["101"] = OperationType.LoadHalfUnsigned;
            _opType["100"]["110"] = OperationType.Lwr;

            // Sexta linha da tabela
            _opType["101"] = new();
            _opType["101"]["000"] = OperationType.StoreByte;
            _opType["101"]["001"] = OperationType.StoreHalf;
            _opType["101"]["010"] = OperationType.Swl;
            _opType["101"]["011"] = OperationType.StoreWord;
            _opType["101"]["110"] = OperationType.Swr;

            // Setima linha da tabela
            _opType["110"] = new();
            _opType["110"]["000"] = OperationType.LoadLinkedWord;
            _opType["110"]["001"] = OperationType.Lwcl;


            // Oitava linha da tabela
            _opType["111"] = new();
            _opType["111"]["000"] = OperationType.StoreCondWord;
            _opType["111"]["001"] = OperationType.Swcl;


            _opType["110"]["101"] = OperationType.Ldcl;
        }

        public abstract void Exec();
    }
}
