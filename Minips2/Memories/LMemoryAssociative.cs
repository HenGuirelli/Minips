using System;
using System.Collections.Generic;
using System.Linq;

namespace Minips2.Memories
{
    public class LMemoryAssociative : Cache
    {
        private readonly Dictionary<int, List<LMemoryLine>> _LMemory = new();
        private readonly SubstitutionPolicy _substitutionPolicy;
        private static readonly Random _random = new();

        public LMemoryAssociative(SubstitutionPolicy substitutionPolicy, int cacheSize = 1024, int ways = 4, int dataSizeBytes = 4, int addressSizeBits = 32)
            : base (addressSizeBits: addressSizeBits, dataSizeBytes: dataSizeBytes)
        {
            _substitutionPolicy = substitutionPolicy;
            _binIndexSize = (int)Math.Log2(cacheSize / ways); // Index da cache, qual linha o dado se encontra na cache

            foreach (var i in Enumerable.Range(0, cacheSize / ways))
            {
                var setValue = new List<LMemoryLine>();
                _LMemory[i] = setValue;
                for (int j = 0; j < ways; j++)
                {
                    setValue.Add(new LMemoryLine());
                }
            }
        }

        protected override LMemoryLine GetMemoryLine(int address)
        {
            var addressBits = Converter.Int32StringToBinaryString(address);
            var index = GetIndex(addressBits);
            var tag = GetTag(addressBits);
            var cacheSet = _LMemory[index];
            foreach (var line in cacheSet)
            {
                if (line.Tag == tag) return line;
            }

            if (_substitutionPolicy == SubstitutionPolicy.Random)
            {
                foreach (var line in cacheSet)
                {
                    if (!line.Valid && line.Tag is null) return line;
                }
                return cacheSet[_random.Next(cacheSet.Count)];
            }
            else
            {
                var lru = cacheSet[0];
                foreach (var item in cacheSet)
                {
                    if (lru.LastUse > item.LastUse)
                    {
                        lru = item;
                    }
                }
                lru.LastUse = DateTime.Now;
                return lru;
            }
        }
    }
}
