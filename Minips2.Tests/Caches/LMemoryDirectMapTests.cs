using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minips2.Memories;

namespace Minips2.Tests.Caches
{
    [TestClass]
    public class LMemoryDirectMapTests
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

            var memory = new LMemoryDirectMap { NextLevel = VolatileMemory.Instance };
            Assert.AreEqual(0, memory.ReadInt(VolatileMemory.DataStartAddress + 0));
            Assert.AreEqual(4, memory.ReadInt(VolatileMemory.DataStartAddress + 4));
            Assert.AreEqual(8, memory.ReadInt(VolatileMemory.DataStartAddress + 8));
            Assert.AreEqual(12, memory.ReadInt(VolatileMemory.DataStartAddress + 12));
            Assert.AreEqual(16, memory.ReadInt(VolatileMemory.DataStartAddress + 16));
        }

        [TestMethod]
        public void OnSet_ShouldBeWriteBack()
        {
            var lMemoryDirectMap = new LMemoryDirectMap { NextLevel = VolatileMemory.Instance };

            lMemoryDirectMap.WriteInt(VolatileMemory.DataStartAddress, 1); // Não é escrito na memoria
            var exceptionRaised = false;
            try
            {
                Assert.AreEqual(1, VolatileMemory.Instance.ReadInt(VolatileMemory.DataStartAddress));
            }
            catch
            {
                exceptionRaised = true;
            }
            // O valor inserido na cache não pode estar na memoria principal agora.
            // Quando acontece uma leitura em um endereço invalido uma exceção é lançada.
            Assert.IsTrue(exceptionRaised);

            lMemoryDirectMap.WriteInt(VolatileMemory.DataStartAddress, 2); // É escrito na memória principal na substituição
            Assert.AreEqual(2, VolatileMemory.Instance.ReadInt(VolatileMemory.DataStartAddress));
        }
    }
}
