using Minips2.Memories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minips2.Registers
{
    public class Register
    {
        public int Number { get; }
        public string Name { get; }
        public List<int> History { get; }
        private int _value;
        public int Value
        {
            get => _value;
            private set
            {
                History.Add(_value);
                _value = value;
            }
        }

        public static Dictionary<string, Register> All => RegisterTable.RegisterByName;

        private static readonly RegisterTable _table = new RegisterTable();

        public Register(int number, string name)
        {
            Number = number;
            Name = name;
            History = new List<int>();
        }

        public static void Reset()
        {
            for (int i = 0; i < 32; i++)
            {
                var register = _table.GetRegisterByNumber(i);
                register.InternalReset();
            }

            SetValue("$sp", 2147479548);
            SetValue("$gp", 268468224);
            SetValue("$pc", VolatileMemory.InstructionStartAddress);
        }

        internal void InternalReset()
        {
            Value = 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static void SetValue(int registerNumber, int value)
        {
            if (registerNumber == 0) return;
            var register = _table.GetRegisterByNumber(registerNumber);
            if (register != null) register.Value = value;
        }

        public static void SetValue(string registerName, int value)
        {
            var register = _table.GetRegisterByName(registerName);
            if (register != null) register.Value = value;
        }

        public static Register Get(int registerNumber)
        {
            return _table.GetRegisterByNumber(registerNumber);
        }

        public static Register Get(string registerName)
        {
            return _table.GetRegisterByName(registerName);
        }
    }
}
