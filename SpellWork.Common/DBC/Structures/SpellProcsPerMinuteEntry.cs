using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellProcsPerMinute")]
    public class SpellProcsPerMinuteEntry
    {
        [Index]
        public uint ID;
        public float BaseProcRate;
        public byte Flags;
    }
}
