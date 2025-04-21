using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellMisc")]
    public class SpellMiscEntry
    {
        [Index]
        public uint ID;
        public ushort CastingTimeIndex;
        public ushort DurationIndex;
        public ushort RangeIndex;
        public byte SchoolMask;
        public uint IconFileDataID;
        public float Speed;
        public uint ActiveIconFileDataID;
        public float LaunchDelay;
        public byte DifficultyID;
        [ArraySize(14)]
        public uint[] Attributes;
        [RelationField]
        public int SpellID;
    }
}
