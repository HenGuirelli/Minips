using System;
using System.Collections.Generic;
using System.Linq;

namespace Minips2.Registers
{
    public class ConditonFlags
    {
        private static Dictionary<int, bool> _values = new();

        public static void Set(int flag, bool value)
        {
            _values[flag] = value;
        }

        public static bool Get(int flag)
        {
            return _values[flag];
        }
    }

    public class Coproc1
    {
        public int Number { get; }
        public string Name { get; }
        public float Value { get; set; }

        public static Dictionary<string, Coproc1> All => CoprocTable.RegisterByName;

        private static readonly CoprocTable _table = new CoprocTable();

        public Coproc1(int number, string name)
        {
            Number = number;
            Name = name;
        }

        public static void Reset()
        {
            for (int i = 0; i < 32; i++)
            {
                var Coproc1 = _table.GetRegisterByNumber(i);
                Coproc1.InternalReset();
            }
        }

        internal void InternalReset()
        {
            Value = 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static double GetDoubleValue(string coproc1)
        {
            var coproc1Number = _table.GetRegisterByName(coproc1);
            return GetDoubleValue(coproc1Number.Number);
        }

        public static double GetDoubleValue(int coproc1Number)
        {
            var f1 = Get(coproc1Number).Value;
            var f2 = Get(coproc1Number + 1).Value;

            return BitConverter.ToDouble(BitConverter.GetBytes(f1).Concat(BitConverter.GetBytes(f2)).ToArray());
        }

        public static void SetValue(int coproc1Number, double value)
        {
            var dbytes = BitConverter.GetBytes(value);
            SetValue(coproc1Number, BitConverter.ToSingle(dbytes, 0));
            SetValue(coproc1Number+1, BitConverter.ToSingle(dbytes, dbytes.Length / 2));
        }

        public static void SetValue(int coproc1Number, float value)
        {
            var coproc1 = _table.GetRegisterByNumber(coproc1Number);
            if (coproc1 != null) coproc1.Value = value;
        }

        public static void SetValue(string coproc1Name, float value)
        {
            var coproc1 = _table.GetRegisterByName(coproc1Name);
            if (coproc1 != null) coproc1.Value = value;
        }

        public static Coproc1 Get(int coproc1Number)
        {
            return _table.GetRegisterByNumber(coproc1Number);
        }

        public static Coproc1 Get(string coproc1Name)
        {
            return _table.GetRegisterByName(coproc1Name);
        }
    }
}
