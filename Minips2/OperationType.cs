namespace Minips2
{
    public enum OperationType
    {
        Invalid,

        // Primeira linha da tabela
        Rformat,
        BltGez,
        Jump,
        JumpAndLink,
        BranchEQ,
        BranchNE,
        Blez,
        BGTZ,

        // Segunda linha da tabela
        AddImmediate,
        Addiu,
        SetLessThanImm,
        SetLessThanImmUnsigned,
        Andi,
        Ori,
        Xori,
        LoadUpperImmediate,

        // Terceira linha da tabela
        TBL,
        FlPt,

        // Quinta linha da tabela
        LoadByte,
        LoadHalf,
        Lwl,
        LoadWord,
        LoadByteUnsigned,
        LoadHalfUnsigned,
        Lwr,

        // Sexta linha da tabela
        StoreByte,
        StoreHalf,
        Swl,
        StoreWord,
        Swr,


        // Setima linha da tabela
        LoadLinkedWord,
        Lwcl,

        // Oitava linha da tabela
        StoreCondWord,
        Swcl,

        // ??
        Ldcl,
    }
}
