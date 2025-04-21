using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellRadius")]
    public sealed class SpellRadiusEntry
    {
        [Index]
        public uint ID;
        public float Radius;
        public float RadiusPerLevel;
        public float RadiusMin;
        public float MaxRadius;
    }
}
