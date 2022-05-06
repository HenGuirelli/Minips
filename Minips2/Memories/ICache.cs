namespace Minips2.Memories
{
    public interface ICache : IMemory
    {
        CacheType CacheType { get; set; }
        // Se a cache não for split, aqui vira uma dependencia circular
        ICache InstructionCache { get; set; }
    }
}
