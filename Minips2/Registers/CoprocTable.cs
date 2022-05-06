using System.Collections.Generic;
using System.Linq;

namespace Minips2.Registers
{
    public class CoprocTable
    {
        private static readonly Dictionary<int, Coproc1> _registerByNumber = new Dictionary<int, Coproc1>();
        public static Dictionary<string, Coproc1> RegisterByName { get; set; } = new Dictionary<string, Coproc1>();

        static CoprocTable()
        {
            _registerByNumber[0] =  new Coproc1(number: 0,  "$f0");
            _registerByNumber[1] =  new Coproc1(number: 1,  "$f1");
            _registerByNumber[2] =  new Coproc1(number: 2,  "$f2");
            _registerByNumber[3] =  new Coproc1(number: 3,  "$f3");
            _registerByNumber[4] =  new Coproc1(number: 4,  "$f4");
            _registerByNumber[5] =  new Coproc1(number: 5,  "$f5");
            _registerByNumber[6] =  new Coproc1(number: 6,  "$f6");
            _registerByNumber[7] =  new Coproc1(number: 7,  "$f7");
            _registerByNumber[8] =  new Coproc1(number: 8,  "$f8");
            _registerByNumber[9] =  new Coproc1(number: 9,  "$f9");
            _registerByNumber[10] = new Coproc1(number: 10, "$f10");
            _registerByNumber[11] = new Coproc1(number: 11, "$f11");
            _registerByNumber[12] = new Coproc1(number: 12, "$f12");
            _registerByNumber[13] = new Coproc1(number: 13, "$f13");
            _registerByNumber[14] = new Coproc1(number: 14, "$f14");
            _registerByNumber[15] = new Coproc1(number: 15, "$f15");
            _registerByNumber[16] = new Coproc1(number: 16, "$f16");
            _registerByNumber[17] = new Coproc1(number: 17, "$f17");
            _registerByNumber[18] = new Coproc1(number: 18, "$f18");
            _registerByNumber[19] = new Coproc1(number: 19, "$f19");
            _registerByNumber[20] = new Coproc1(number: 20, "$f20");
            _registerByNumber[21] = new Coproc1(number: 21, "$f21");
            _registerByNumber[22] = new Coproc1(number: 22, "$f22");
            _registerByNumber[23] = new Coproc1(number: 23, "$f23");
            _registerByNumber[24] = new Coproc1(number: 24, "$f24");
            _registerByNumber[25] = new Coproc1(number: 25, "$f25");
            _registerByNumber[26] = new Coproc1(number: 26, "$f26");
            _registerByNumber[27] = new Coproc1(number: 27, "$f27");
            _registerByNumber[28] = new Coproc1(number: 28, "$f28");
            _registerByNumber[29] = new Coproc1(number: 29, "$f29");
            _registerByNumber[30] = new Coproc1(number: 30, "$f30");
            _registerByNumber[31] = new Coproc1(number: 30, "$f31");

            RegisterByName = _registerByNumber.Values.ToDictionary(x => x.Name);
        }
    
        public Coproc1 GetRegisterByNumber(int number)
        {
            return _registerByNumber[number];
        }

        public Coproc1 GetRegisterByName(string name)
        {
            return RegisterByName[name];
        }
    }
}
