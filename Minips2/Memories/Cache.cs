using System;
using System.Collections.Generic;

namespace Minips2.Memories
{
    public abstract class Cache : ICache
    {
        public CacheType CacheType { get; set; }
        public ICache InstructionCache { get; set; }
        public double WriteLatency { get; set; }
        public double ReadLatency { get; set; }
        public IMemory NextLevel { get; set; }
        public MemoryStatistics Statistics { get; set; } = new();
        public string Name { get; set; }
        public bool IsSplit { get; private set; }

        private readonly int _addressSizeBits;
        protected readonly int _offsetSize;
        private readonly int _dataSizeBytes;
        protected int _binIndexSize;
        protected CacheCoherence _cacheCoherence;

        protected Cache(int dataSizeBytes = 4, int addressSizeBits = 32)
        {
            _addressSizeBits = addressSizeBits; // Tamanho de um endereço em bits, MIPS é 32 bits
            _offsetSize = (int)Math.Log2(dataSizeBytes); // Ultimos bits que indentificam os bytes dentro da linha da cache
            _dataSizeBytes = dataSizeBytes;
        }

        public virtual int ReadInt(int address)
        {
            GlobalStatistics.TotalCycles += (int)ReadLatency;
            Statistics.EstimatedReadLatency += ReadLatency;
            var oldMisses = Statistics.TotalMisses;

            var value = ReadBytes(address, 4);

            if (oldMisses == Statistics.TotalMisses)
            {
                Statistics.TotalHits++;
            }
            return BitConverter.ToInt32(value);
        }

        public byte[] ReadBytes(int address, int length)
        {
            var addressHex = address.ToString("X").PadLeft(8, '0');
            bool isInstruction = address < VolatileMemory.RodataStartAddress;
            var line = (((address - (isInstruction ? VolatileMemory.InstructionStartAddress : VolatileMemory.DataStartAddress)) / Program.CacheSize) + 
                    (isInstruction ? 65536 : 4195328))
                .ToString("X")
                .PadLeft(8, '0');
            Logger.Trace($"{(isInstruction ? "I" : "R")} 0x{addressHex} (line# 0x{line})");
            Logger.Debug($"\t{Name}: read");
            Logger.Debug($"\t{Name}: Hit");

            var result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Get(address + i);
            }
            return result;
        }

        internal bool TryGetValueWithoutNextLevel(int address, out byte[] result)
        {
            var addressBits = Converter.Int32StringToBinaryString(address);
            var tag = GetTag(addressBits);
            var byteOffset = GetOffSet(addressBits);
            var line = GetMemoryLine(address);
            if (!line.Valid || line.Tag != tag)
            {
                result = null;
                return false;
            }

            if (line.CacheStateMesi == CacheStateMesi.Modified)
            {
                var qtyloop = line.Data.Length / 4;
                for (int i = 0; i < qtyloop; i++)
                {
                    NextLevel.WriteInt(address, BitConverter.ToInt32(line.Data, i * 4));
                }
            }

            line.CacheStateMesi = CacheStateMesi.Shared;
            result = line.Data;
            return true;
        }

        private byte Get(int address)
        {
            var addressBits = Converter.Int32StringToBinaryString(address);
            var tag = GetTag(addressBits);
            var byteOffset = GetOffSet(addressBits);
            var line = GetMemoryLine(address);
            if (!line.Valid || line.Tag != tag)
            {
                if (line.IsModified)
                {
                    NextLevel.WriteInt(line.Address, BitConverter.ToInt32(line.Data));
                }

                byte[] memoryBytes;
                if (_cacheCoherence != null && _cacheCoherence.TryGetCacheValue(this, address, out var resultFromAnotherCache))
                {
                    line.CacheStateMesi = CacheStateMesi.Shared;
                    line.Valid = true;
                    line.Address = address;
                    line.Data = resultFromAnotherCache;
                    line.Tag = GetTag(Converter.Int32StringToBinaryString(address));
                    memoryBytes = resultFromAnotherCache;
                }
                else
                {
                    var addressToRead = address - byteOffset;
                    memoryBytes = NextLevel.ReadBytes(addressToRead, _dataSizeBytes);
                }
                Statistics.TotalMisses++;

                // TODO: Remover gambiarra
                if (Name == "L2")
                {
                    GlobalStatistics.TotalCycles += (int)ReadLatency;
                }

                line.Valid = true;
                line.Address = address;
                line.Data = memoryBytes;
                line.Tag = tag;
                return memoryBytes[byteOffset];
            }
            return line.Data[byteOffset];
        }

        internal void InvalidCache(int address)
        {
            var line = GetMemoryLine(address);
            line.CacheStateMesi = CacheStateMesi.Invalid;
            line.Valid = false;
        }

        public virtual void WriteInt(int address, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            var addressBits = Converter.Int32StringToBinaryString(address);
            var tag = GetTag(addressBits);
            var line = GetMemoryLine(address);

            if (line.CacheStateMesi == CacheStateMesi.Shared ||
                line.CacheStateMesi == CacheStateMesi.Invalid)
            {
                _cacheCoherence?.InvalidCache(this, address);
                line.CacheStateMesi = CacheStateMesi.Modified;
            }

            line.Data = bytes;
            line.Tag = tag;
            line.Valid = true;
            line.Address = address;
            line.IsModified = true;

            GlobalStatistics.TotalCycles += (int)WriteLatency;
            Statistics.EstimatedWriteLatency += WriteLatency;
            Statistics.TotalHits++;
        }

        internal void AddCoeherence(CacheCoherence cacheCoherence)
        {
            _cacheCoherence = cacheCoherence;
            IsSplit = cacheCoherence != null;
        }

        public virtual int ReadInstruction(int address)
        {
            if (CacheType == CacheType.Instruction)
            {
                GlobalStatistics.TotalCycles += (int)ReadLatency;
                Statistics.EstimatedReadLatency += ReadLatency;

                var oldMisses = Statistics.TotalMisses;

                var value = ReadBytes(address, 4);

                if (oldMisses == Statistics.TotalMisses)
                {
                    Statistics.TotalHits++;
                }
                return BitConverter.ToInt32(value);
            }
            return InstructionCache.ReadInstruction(address);
        }

        protected int GetTag(string addressBits)
        {
            var endIndex = _addressSizeBits - _binIndexSize - _offsetSize;
            var binIndex = addressBits.Substring(0, endIndex);
            return Converter.BinaryStringToInt32(binIndex);
        }

        protected int GetIndex(string addressBits)
        {
            var startIndex = _addressSizeBits - _binIndexSize - _offsetSize;
            var binIndex = addressBits.Substring(startIndex, _binIndexSize);
            return Converter.BinaryStringToInt32(binIndex);
        }

        protected int GetOffSet(string addressBits)
        {
            var binResult = addressBits.Substring(addressBits.Length - _offsetSize, _offsetSize);
            return Converter.BinaryStringToInt32(binResult);
        }

        protected abstract LMemoryLine GetMemoryLine(int address);
    }
}
