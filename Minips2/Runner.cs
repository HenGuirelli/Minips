using Minips2.Memories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Minips2
{
    public class Runner
    {
        private readonly string _filepath;
        private const int WordSize = 4;

        public Runner(string filepath)
        {
            _filepath = filepath;
        }

        public void Run()
        {
            Registers.Register.Reset();
            var dataFileName = $"{_filepath}.data";
            ReadDataFromFileAndPutIntoMemory(dataFileName);

            var textFileName = $"{_filepath}.text";
            ReadIstructionsFromFileAndPutIntoMemory(textFileName);

            var rodataFileName = $"{_filepath}.rodata";
            ReadRodataFromFileAndPutIntoMemory(rodataFileName);

            ExecuteInstructionFromMemory();
        }

        private void ReadRodataFromFileAndPutIntoMemory(string rodataFileName)
        {
            if (!File.Exists(rodataFileName)) return;
            ReadFileAndPutIntoMemory(rodataFileName, VolatileMemory.RodataStartAddress);
        }

        private void ReadDataFromFileAndPutIntoMemory(string dataFileName)
        {
            if (!File.Exists(dataFileName)) return;
            ReadFileAndPutIntoMemory(dataFileName, VolatileMemory.DataStartAddress);
        }

        public void ReadIstructionsFromFileAndPutIntoMemory(string filename)
        {
            ReadFileAndPutIntoMemory(filename, VolatileMemory.InstructionStartAddress);
        }

        public void ReadFileAndPutIntoMemory(string filename, int startAddress)
        {
            // https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa
            using var fs = new FileStream(filename, FileMode.Open);
            string hex = string.Empty;

            int hexIn;
            for (int i = 0; (hexIn = fs.ReadByte()) != -1; i++)
            {
                hex += string.Format("{0:X2}", hexIn);
            }

            var chunks = Split(hex, 8);
            int offset = 0;
            foreach (var instruction in chunks)
            {
                var it = Converter.StringToByteArray(instruction);
                var intValue = BitConverter.ToInt32(it, 0);
                Program.PrincipalMemory.InternalWriteInt(startAddress + offset, intValue);
                offset += 4;
            }
        }

        private void ExecuteInstructionFromMemory()
        {
            GlobalStatistics.SimulationTime.Restart();
            while (true)
            {
                ExecuteInstruction(Registers.Register.Get("$pc").Value);
            }
        }

        private void ExecuteInstruction(int address)
        {
            var command = BitConverter.GetBytes(MemoryHierarchy.FirstLevel.ReadInstruction(address));
            var hexlitleEndian = ToLittleEndian(Converter.ByteArrayToString(command));
            var instruction = Instruction.FromString(hexlitleEndian);
            GlobalStatistics.InstructionCount++;
            GlobalStatistics.TotalCycles += 1;
            instruction.Exec();

            if (instruction is IJump jumpInstruction)
            {
                // Caso a instrução seja um jump executamos a proxima isntrução obrigatóriamente
                var command2 = BitConverter.GetBytes(MemoryHierarchy.FirstLevel.ReadInt(address + WordSize));
                var hexlitleEndian2 = ToLittleEndian(Converter.ByteArrayToString(command2));
                var instruction2 = Instruction.FromString(hexlitleEndian2);
                GlobalStatistics.InstructionCount++;
                GlobalStatistics.TotalCycles += 1;
                instruction2.Exec();

                if (!jumpInstruction.IsJumped)
                {
                    // Se o salto não aconteceu devemos ir para a proxima instrução
                    // Como executamos a branch e mais ou outro comando o pc é incrementado em 8
                    // ao invés de 4
                    Registers.Register.SetValue("$pc", Registers.Register.Get("$pc").Value + 8);
                }
                return;
            }

            Registers.Register.SetValue("$pc", Registers.Register.Get("$pc").Value + WordSize);
        }

        // https://stackoverflow.com/questions/1450774/splitting-a-string-into-chunks-of-a-certain-size
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        private string ToLittleEndian(string hex)
        {
            return $"{hex[6..8]}{hex[4..6]}{hex[2..4]}{hex[0..2]}";
        }
    }
}
