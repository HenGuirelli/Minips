using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minips2.Memories;

namespace Minips2.Tests.Caches
{
    [TestClass]
    public class LMemoryAssociativeTests
    {
        [TestMethod]
        public void OnGet()
        { 
            // arrange
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 0, 0);
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 4, 4);
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 8, 8);
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 12, 12);
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 16, 16);

            var memory = new LMemoryAssociative(SubstitutionPolicy.Random) { NextLevel = VolatileMemory.Instance };
            Assert.AreEqual(0, memory.ReadInt(VolatileMemory.DataStartAddress + 0));
            Assert.AreEqual(4, memory.ReadInt(VolatileMemory.DataStartAddress + 4));
            Assert.AreEqual(8, memory.ReadInt(VolatileMemory.DataStartAddress + 8));
            Assert.AreEqual(12, memory.ReadInt(VolatileMemory.DataStartAddress + 12));
            Assert.AreEqual(16, memory.ReadInt(VolatileMemory.DataStartAddress + 16));
        }

        [TestMethod]
        public void OnGetLRU_ShouldReplaceLeastRecenclyUsed()
        {
            // arrange
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 0, 0);
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 16, 4);
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress + 32, 8);

            var memory = new LMemoryAssociative(SubstitutionPolicy.LRU, cacheSize: 8, ways: 2) { NextLevel = VolatileMemory.Instance };
            Assert.AreEqual(0, memory.ReadInt(VolatileMemory.DataStartAddress + 0));
            Assert.AreEqual(4, memory.ReadInt(VolatileMemory.DataStartAddress + 16));
            Assert.AreEqual(8, memory.ReadInt(VolatileMemory.DataStartAddress + 32));
        }

        [TestMethod]
        public void OnCacheCoeherence()
        {
            VolatileMemory.Instance.WriteInt(VolatileMemory.DataStartAddress, 49);
            var l1i = new LMemoryAssociative(SubstitutionPolicy.Random, cacheSize: 1024)
            {
                Name = "L1I",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Instruction,
                NextLevel = VolatileMemory.Instance
            };
            var l1d = new LMemoryAssociative(SubstitutionPolicy.Random, cacheSize: 1024)
            {
                Name = "L1D",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Data,
                NextLevel = VolatileMemory.Instance
            };

            l1d.InstructionCache = l1i;

            var coeherence = new CacheCoherence();
            coeherence.AddCache(l1d);
            coeherence.AddCache(l1i);

            Assert.AreEqual(49, l1d.ReadInt(VolatileMemory.DataStartAddress));
            Assert.AreEqual(49, l1i.ReadInt(VolatileMemory.DataStartAddress));

            l1d.WriteInt(VolatileMemory.DataStartAddress, 50);
            Assert.AreEqual(50, l1d.ReadInt(VolatileMemory.DataStartAddress));
            Assert.AreEqual(50, l1i.ReadInt(VolatileMemory.DataStartAddress));
        }
    }
}
