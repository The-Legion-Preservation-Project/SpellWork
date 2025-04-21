using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellMissileMotion")]
    public sealed class SpellMissileMotionEntry
    {
        [Index]
        public uint ID;
        public string Name;
        public string Script;
        public byte Flags;
        public byte MissileCount;
    }
}
