using Minips2.Memories;
using System;

namespace Minips2
{
    public enum RunnerMethod
    {
        Run,
        Decode,
        Trace,
        Debug
    }

    internal class Program
    {
        public static RunnerMethod RunnerMethod { get; private set; }
        public static int CacheSize { get; private set; } = 32;

        public static IPrincipalMemory PrincipalMemory { get; private set; } = VolatileMemory.Instance;

        static void Main(string[] args)
        {
            var option = args[0].ToLower();
            if (option == "run")
            {
                RunnerMethod = RunnerMethod.Run;
                GlobalStatistics.FrequencyPipeline = 33.8688;
                GlobalStatistics.PicosecondsPipeline = 29525.6991686;
                Run(args[2], args[1]);
            }

            if (option == "trace")
            {
                RunnerMethod = RunnerMethod.Trace;
                GlobalStatistics.FrequencyPipeline = 33.8688;
                GlobalStatistics.PicosecondsPipeline = 29525.6991686;
                Run(args[2], args[1]);
            }

            if (option == "debug")
            {
                RunnerMethod = RunnerMethod.Debug;
                GlobalStatistics.FrequencyPipeline = 33.8688;
                GlobalStatistics.PicosecondsPipeline = 29525.6991686;
                Run(args[2], args[1]);
            }
        }

        private static void Run(string filename, string conf)
        {
            MemoryHierarchy.FirstLevel = CreateMemoryHierarchy(conf);

            var runner = new Runner(filename);
            runner.Run();
        }

        private static IMemory CreateMemoryHierarchy(string conf)
        {
            if (conf == "1") return Conf1();
            if (conf == "2") return Conf2();
            if (conf == "3") return Conf3();
            if (conf == "4") return Conf4();
            if (conf == "5") return Conf5();
            if (conf == "6") return Conf6();
            if (conf == "7") return Conf7();

            throw new ApplicationException();
        }

        private static IMemory Conf1()
        {
            return VolatileMemory.Instance;
        }

        private static IMemory Conf2()
        {
            return new LMemoryDirectMap(1024, dataSizeBytes: 32)
            {
                Name = "L1",
                ReadLatency = 1,
                WriteLatency = 1,
                NextLevel = VolatileMemory.Instance
            };
        }

        private static IMemory Conf3()
        {
            var l1i = new LMemoryDirectMap(cacheSize: 512, dataSizeBytes: 32)
            {
                Name = "L1I",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Instruction,
                NextLevel = VolatileMemory.Instance
            };
            var l1d = new LMemoryDirectMap(cacheSize: 512, dataSizeBytes: 32)
            {
                Name = "L1D",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Data,
                InstructionCache = l1i,
                NextLevel = VolatileMemory.Instance
            };
            var cohenceCache = new CacheCoherence();
            cohenceCache.AddCache(l1i);
            cohenceCache.AddCache(l1d);

            l1d.InstructionCache = l1i;
            return l1d;
        }

        private static IMemory Conf4()
        {
            var l1i = new LMemoryDirectMap(cacheSize: 512, dataSizeBytes: 32)
            {
                Name = "L1I",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Instruction,
                NextLevel = VolatileMemory.Instance
            };
            var l1d = new LMemoryDirectMap(cacheSize: 512, dataSizeBytes: 32)
            {
                Name = "L1D",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Data,
                InstructionCache = l1i,
                NextLevel = VolatileMemory.Instance
            };
            var cohenceCache = new CacheCoherence();
            cohenceCache.AddCache(l1i);
            cohenceCache.AddCache(l1d);

            l1d.InstructionCache = l1i;
            return l1d;
        }

        private static IMemory Conf5()
        {
            var l1i = new LMemoryAssociative(SubstitutionPolicy.LRU, ways: 4, cacheSize: 512, dataSizeBytes: 32)
            {
                Name = "L1I",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Instruction,
                NextLevel = VolatileMemory.Instance
            };
            var l1d = new LMemoryAssociative(SubstitutionPolicy.LRU, ways: 4, cacheSize: 512, dataSizeBytes: 32)
            {
                Name = "L1D",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Data,
                NextLevel = VolatileMemory.Instance
            };
            var cohenceCache = new CacheCoherence();
            cohenceCache.AddCache(l1i);
            cohenceCache.AddCache(l1d);

            l1d.InstructionCache = l1i;
            return l1d;
        }

        private static IMemory Conf6()
        {
            CacheSize = 64;

            var l2 = new LMemoryAssociative(SubstitutionPolicy.LRU, ways: 8, cacheSize: 2048, dataSizeBytes: 64)
            {
                Name = "L2",
                ReadLatency = 10,
                WriteLatency = 10,
                CacheType = CacheType.Instruction,
                NextLevel = VolatileMemory.Instance
            };

            var l1i = new LMemoryAssociative(SubstitutionPolicy.LRU, ways: 4, cacheSize: 512, dataSizeBytes: 64)
            {
                Name = "L1I",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Instruction,
                NextLevel = l2
            };
            var l1d = new LMemoryAssociative(SubstitutionPolicy.LRU, ways: 4, cacheSize: 512, dataSizeBytes: 64)
            {
                Name = "L1D",
                ReadLatency = 1,
                WriteLatency = 1,
                CacheType = CacheType.Data,
                InstructionCache = l1i,
                NextLevel = l2
            };
            var cohenceCache = new CacheCoherence();
            cohenceCache.AddCache(l1i);
            cohenceCache.AddCache(l1d);

            l1d.InstructionCache = l1i;
            return l1d;
        }

        private static IMemory Conf7()
        {
            PrincipalMemory = PersistentMemory.Instance;

            var l2 = new LMemoryDirectMap(2048, dataSizeBytes: 32)
            {
                Name = "L2",
                ReadLatency = 10,
                WriteLatency = 10,
                NextLevel = PersistentMemory.Instance
            };
            var l1 = new LMemoryDirectMap(1024, dataSizeBytes: 32)
            {
                Name = "L1",
                ReadLatency = 1,
                WriteLatency = 1,
                NextLevel = l2
            };

            return l1;
        }
    }
}
