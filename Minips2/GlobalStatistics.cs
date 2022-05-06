using System.Diagnostics;

namespace Minips2
{
    internal static class GlobalStatistics
    {
        public static int InstructionCount { get; set; }
        public static Stopwatch SimulationTime { get; } = new();
        public static int TotalCycles { get; set; }
        public static double FrequencyPipeline { get; set; }
        public static double PicosecondsPipeline { get; internal set; }
    }
}
