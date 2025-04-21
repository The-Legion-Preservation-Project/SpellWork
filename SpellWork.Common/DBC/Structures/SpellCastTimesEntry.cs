using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellCastTimes")]
    public class SpellCastTimesEntry
    {
        [Index]
        public int ID;
        public int CastTime;
        public int MinCastTime;
        public short CastTimePerLevel;
    }
}
