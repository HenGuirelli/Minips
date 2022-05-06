using System;
using System.Collections.Generic;

namespace Minips2.Memories
{
    public class MemoryHierarchy
    {
        public static IMemory FirstLevel { get; set; }
    }

    public class VolatileMemory : IPrincipalMemory
    {
        public IMemory NextLevel { get; set; }
        public string Name => "RAM";
        public MemoryStatistics Statistics { get; } = new();
        public double WriteLatency { get; set; } = 100;
        public double ReadLatency { get; set; } = 100;

        public const int InstructionStartAddress = 4194304;
        public const int DataStartAddress = 268500992; // 0x10010000
        public const int RodataStartAddress = 8388608; // 0x00800000
        private readonly Dictionary<int, byte> _memoryValue = new();

        private VolatileMemory() { }

        private static VolatileMemory _instance;
        public static VolatileMemory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VolatileMemory();
                }
                return _instance;
            }
        }


        public void InternalWriteInt(int address, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            int i = 0;
            foreach (byte @byte in bytes)
            {
                Write(address + i, @byte);
                i++;
            }
        }

        public void WriteInt(int address, int value)
        {
            GlobalStatistics.TotalCycles += (int)WriteLatency;
            Statistics.EstimatedWriteLatency += WriteLatency;
            Statistics.TotalHits++;
            InternalWriteInt(address, value);
            NextLevel?.WriteInt(address, value);
        }

        private void Write(int address, byte value)
        {
            _memoryValue[address] = value;
        }

        public int ReadInt(int address)
        {
            var value = ReadBytes(address, 4);
            return BitConverter.ToInt32(value);
        }

        public byte[] ReadBytes(int address, int length)
        {
            var result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (!_memoryValue.TryGetValue(address + i, out result[i]))
                {
                    result[i] = default(byte);
                }
            }
            Statistics.EstimatedReadLatency += ReadLatency;
            Statistics.TotalHits++;
            GlobalStatistics.TotalCycles += (int)ReadLatency;

            var addressHex = address.ToString("X").PadLeft(8, '0');
            bool isInstruction = address < RodataStartAddress;
            var line = (((address - (isInstruction ? InstructionStartAddress : DataStartAddress)) / Program.CacheSize) + 131072)
                .ToString("X")
                .PadLeft(8, '0');
            Logger.Trace($"{(isInstruction ? "I" : "R")} 0x{addressHex} (line# 0x{line})");
            Logger.Debug($"\tRAM: read");
            Logger.Debug($"\tRAM: Hit");

            return result;
        }

        public int ReadInstruction(int address)
        {
            var value = ReadBytes(address, 4);
            return BitConverter.ToInt32(value);
        }
    }
}
