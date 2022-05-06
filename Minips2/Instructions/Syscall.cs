using Minips2.Memories;
using Minips2.Registers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Minips2.Instructions
{
    internal class Syscall : Instruction
    {
        public Syscall(InstructionRepresentation instructionRepresentation)
        {
            InstructionRepresentation = instructionRepresentation;
        }

        public override void Exec()
        {
            var reg = Register.Get("$v0");
            switch (reg.Value)
            {
                case 1:
                    {
                        Console.Write(Register.Get("$a0"));
                        return;
                    }
                case 2:
                    {
                        var register = Coproc1.Get("$f12");
                        Console.Write(register.Value);
                        return;
                    }
                case 3:
                    {
                        Console.Write(Coproc1.GetDoubleValue("$f12"));
                        return;
                    }
                case 4:
                    {
                        GlobalStatistics.TotalCycles--;
                        var register = Register.Get("$a0");
                        int i = 0;
                        while (true)
                        {
                            var val = MemoryHierarchy.FirstLevel.ReadInt(register.Value + (i * 4));
                            foreach (var caractere in BitConverter.GetBytes(val))
                            {
                                Console.Write((char)caractere);
                                if (caractere == '\0')
                                {
                                    return;
                                }
                            }
                            i++;
                        }
                    }
                case 5:
                    {
                        var value = int.Parse(Console.ReadLine());
                        Register.SetValue("$v0", value);
                        return;
                    }
                case 6:
                    {
                        var value = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Coproc1.SetValue("$f0", value);
                        return;
                    }
                case 7:
                    {
                        var value = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        var bytes = BitConverter.GetBytes(value);
                        var f1 = new byte[4];
                        var f2 = new byte[4];

                        for (int i = 0; i < 4; i++)
                        {
                            f1[i] = bytes[i];
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            f2[i] = bytes[i + 4];
                        }

                        Coproc1.SetValue("$f0", BitConverter.ToSingle(f1, 0));
                        Coproc1.SetValue("$f1", BitConverter.ToSingle(f2, 0));
                        return;
                    }
                case 10:
                    {
                        // Statistics ------------------------------------
                        GlobalStatistics.SimulationTime.Stop();

                        Console.WriteLine();
                        Console.WriteLine();

                        Console.WriteLine($"Instruction Count: {GlobalStatistics.InstructionCount}");
                        Console.WriteLine($"Simulation Time: {GlobalStatistics.SimulationTime.ElapsedMilliseconds} ms");


                        Console.WriteLine();
                        Console.WriteLine();

                        Console.WriteLine("Simulated execution times for:");
                        Console.WriteLine("--------------------------------------------------------------");
                        Console.WriteLine("Monocycle");
                        Console.WriteLine($"\tCycles: {GlobalStatistics.TotalCycles}");
                        Console.WriteLine($"\tFrequency: {GlobalStatistics.FrequencyPipeline / 4} MHz");
                        var estimatedExecutionTimeMonocycle = (GlobalStatistics.PicosecondsPipeline * 4 * GlobalStatistics.TotalCycles / 1000000000000);
                        Console.WriteLine($"\tEstimated execution time: {estimatedExecutionTimeMonocycle.ToString("0.0000")} sec");

                        Console.WriteLine("Pipelined");
                        Console.WriteLine($"\tCycles: {GlobalStatistics.TotalCycles + 4}");
                        Console.WriteLine($"\tFrequency: {GlobalStatistics.FrequencyPipeline} MHz");
                        var estimatedExecutionTimePipelined = (GlobalStatistics.PicosecondsPipeline * (GlobalStatistics.TotalCycles + 4) / 1000000000000);
                        Console.WriteLine($"\tEstimated execution time: {estimatedExecutionTimePipelined.ToString("0.0000")} sec");

                        Console.WriteLine($"Speedup Monocycle/Pipeline: {(estimatedExecutionTimeMonocycle / estimatedExecutionTimePipelined).ToString("0.00")}x");

                        Console.WriteLine();
                        Console.WriteLine();

                        var memory = MemoryHierarchy.FirstLevel;
                        while (memory != null)
                        {
                            Console.WriteLine($"{memory.Name} | Total hits: {memory.Statistics.TotalHits} | Total Misses: {memory.Statistics.TotalMisses} | Miss Rate: {(memory.Statistics.MissRate is double.NaN ? "0" : memory.Statistics.MissRate.ToString())}%");
                            if (memory is ICache cache && cache.InstructionCache != memory && cache.InstructionCache != null)
                            {
                                Console.WriteLine($"{cache.InstructionCache.Name} | Total hits: {cache.InstructionCache.Statistics.TotalHits} | Total Misses: {cache.InstructionCache.Statistics.TotalMisses} | Miss Rate: {(cache.InstructionCache.Statistics.MissRate is double.NaN ? "0" : cache.InstructionCache.Statistics.MissRate.ToString())}%");
                            }
                            memory = memory.NextLevel;
                        }

                        // Statistics ------------------------------------
                        Environment.Exit(0);
                        return;
                    }
                case 11:
                    {
                        var register = Register.Get("$a0");
                        Console.Write((char)register.Value);
                        return;
                    }
            }
            throw new System.Exception();
        }
    }
}
