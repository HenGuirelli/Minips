using System.Collections.Generic;
using System.Linq;

namespace Minips2.Registers
{
    public class RegisterTable
    {
        private static readonly Dictionary<int, Register> _registerByNumber = new Dictionary<int, Register>();
        public static Dictionary<string, Register> RegisterByName { get; set; } = new Dictionary<string, Register>();

        static RegisterTable()
        {
            _registerByNumber[0] = new Register(number: 0, "$zero");
            _registerByNumber[1] = new Register(number: 1, "$at");
            _registerByNumber[2] = new Register(number: 2, "$v0");
            _registerByNumber[3] = new Register(number: 3, "$v1");
            _registerByNumber[4] = new Register(number: 4, "$a0");
            _registerByNumber[5] = new Register(number: 5, "$a1");
            _registerByNumber[6] = new Register(number: 6, "$a2");
            _registerByNumber[7] = new Register(number: 7, "$a3");
            _registerByNumber[8] = new Register(number: 8, "$t0");
            _registerByNumber[9] = new Register(number: 9, "$t1");
            _registerByNumber[10] = new Register(number: 10, "$t2");
            _registerByNumber[11] = new Register(number: 11, "$t3");
            _registerByNumber[12] = new Register(number: 12, "$t4");
            _registerByNumber[13] = new Register(number: 13, "$t5");
            _registerByNumber[14] = new Register(number: 14, "$t6");
            _registerByNumber[15] = new Register(number: 15, "$t7");
            _registerByNumber[16] = new Register(number: 16, "$s0");
            _registerByNumber[17] = new Register(number: 17, "$s1");
            _registerByNumber[18] = new Register(number: 18, "$s2");
            _registerByNumber[19] = new Register(number: 19, "$s3");
            _registerByNumber[20] = new Register(number: 20, "$s4");
            _registerByNumber[21] = new Register(number: 21, "$s5");
            _registerByNumber[22] = new Register(number: 22, "$s6");
            _registerByNumber[23] = new Register(number: 23, "$s7");
            _registerByNumber[24] = new Register(number: 24, "$t8");
            _registerByNumber[25] = new Register(number: 25, "$t9");
            _registerByNumber[26] = new Register(number: 26, "$k0");
            _registerByNumber[27] = new Register(number: 27, "$k1");
            _registerByNumber[28] = new Register(number: 28, "$gp");
            _registerByNumber[29] = new Register(number: 29, "$sp");
            _registerByNumber[30] = new Register(number: 30, "$fp");
            _registerByNumber[31] = new Register(number: 31, "$ra");
            _registerByNumber[32] = new Register(number: 32, "$pc");
            _registerByNumber[33] = new Register(number: 33, "hi");
            _registerByNumber[34] = new Register(number: 34, "lo");

            RegisterByName = _registerByNumber.Values.ToDictionary(x => x.Name);
        }
    
        public Register GetRegisterByNumber(int number)
        {
            return _registerByNumber[number];
        }

        public Register GetRegisterByName(string name)
        {
            return RegisterByName[name];
        }
    }
}
