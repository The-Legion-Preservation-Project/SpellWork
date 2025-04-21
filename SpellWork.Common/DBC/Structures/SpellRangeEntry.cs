using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellRange")]
    public sealed class SpellRangeEntry
    {
        [Index]
        public int ID;
        public string DisplayName;
        public string DisplayNameShort;
        [ArraySize(2)]
        public float[] MinRange;
        [ArraySize(2)]
        public float[] MaxRange;
        public byte Flags;
    }
}
