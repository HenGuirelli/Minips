using System;
using System.Collections.Generic;
using System.Linq;

namespace Minips2.Memories
{
    public class LMemoryDirectMap : Cache
    {
        private readonly int _cacheSize;
        private readonly Dictionary<int, LMemoryLine> _LMemory = new();

        public LMemoryDirectMap(int cacheSize = 1024, int dataSizeBytes = 4, int addressSizeBits = 32)
            : base(addressSizeBits: addressSizeBits, dataSizeBytes: dataSizeBytes)
        {
            _cacheSize = cacheSize; // Tamanho da cache (somente os dados) (Quantidade total de blocos)
            _binIndexSize = (int)Math.Log2(_cacheSize); // Index da cache, qual linha o dado se encontra na cache

            foreach (var i in Enumerable.Range(0, cacheSize))
            {
                _LMemory[i] = new LMemoryLine();
            }
        }

        protected override LMemoryLine GetMemoryLine(int address)
        {
            var addressBits = Converter.Int32StringToBinaryString(address);
            var index = GetIndex(addressBits);
            return _LMemory[index];
        }
    }
}
