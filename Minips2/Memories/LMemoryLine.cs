using System;
using System.Linq;

namespace Minips2.Memories
{
    public class LMemoryLine
    {
        public bool Valid { get; set; }
        public int? Tag { get; set; }
        private byte[] _data;
        public byte[] Data
        {
            get => _data;
            set => _data = value;
        }
        public DateTime LastUse { get; set; }
        public bool IsModified { get; set; }
        public CacheStateMesi CacheStateMesi { get; set; } = CacheStateMesi.Invalid;
        public int Address { get; set; }

        public override string ToString()
        {
            return $"Valid={Valid}|Tag={Tag}|Data={string.Join("", Data.Select(it => it.ToString()))}|LastUse={LastUse}";
        }
    }
}
