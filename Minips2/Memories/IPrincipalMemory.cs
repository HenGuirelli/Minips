namespace Minips2.Memories
{
    public interface IPrincipalMemory : IMemory
    {
        void InternalWriteInt(int address, int value);
    }
}