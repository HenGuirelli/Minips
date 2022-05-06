namespace Minips2
{
    public enum RFormatOp
    {
        // Primeira linha do formato tipo R
        ShiftLeftLogical,
        ShiftRightLogical,
        Sra,
        Sllv,
        Srlv,
        Srav,

        // Segunda linha RFormat
        JumpRegister,
        Jarl,
        Syscall,
        Break,

        // Terceira linha RFormat
        Mfhi,
        Mthi,
        Mflo,
        Mtlo,

        // Quarta linha RFormat
        Mult,
        Multu,
        Div,
        Divu,

        // Quinta linha RFormat
        Add,
        Addu,
        Subtract,
        Subu,
        And,
        Or,
        Xor,
        Nor,

        // Sexta linha RFormat
        SetLT,
        SetLTUnsigned,
    }
}
