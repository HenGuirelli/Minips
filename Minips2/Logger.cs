using System;
using System.IO;

namespace Minips2
{
    internal static class Logger
    {
        static string FilePath = @"D:\Projetos\Git\UFABC\EstudosDirigidos\Minips2\Minips2\Entries\minips2.trace";
        static Logger()
        {
            if (Program.RunnerMethod == RunnerMethod.Trace || Program.RunnerMethod == RunnerMethod.Debug)
            {
                using var streamWritter = new StreamWriter(FilePath);
                streamWritter.Write("");
            }
        }

        public static void Trace(string value)
        {
            if (Program.RunnerMethod == RunnerMethod.Trace || Program.RunnerMethod == RunnerMethod.Debug)
               File.AppendAllText(FilePath, value + "\n");
        }

        public static void Debug(string value)
        {
            if (Program.RunnerMethod == RunnerMethod.Debug)
                File.AppendAllText(FilePath, value + "\n");
        }
    }
}
