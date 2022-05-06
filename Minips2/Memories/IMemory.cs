namespace Minips2.Memories
{
    public interface IMemory
    {
        int ReadInt(int address);
        int ReadInstruction(int address);
        byte[] ReadBytes(int address, int lenght);
        void WriteInt(int address, int value);
        double WriteLatency { get; }
        double ReadLatency { get; }

        IMemory NextLevel { get; }
        MemoryStatistics Statistics { get; }
        string Name { get; }
    }

    public class MemoryStatistics
    {
        public int TotalHits { get; set; }
        public int TotalMisses { get; set; }
        public double MissRate => (TotalMisses / (double)(TotalHits + TotalMisses)) * 100;
        public double EstimatedWriteLatency { get; set; }
        public double EstimatedReadLatency { get; set; }
    }
}