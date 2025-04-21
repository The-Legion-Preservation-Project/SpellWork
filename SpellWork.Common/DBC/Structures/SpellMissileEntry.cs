using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellMissile")]
    public sealed class SpellMissileEntry
    {
        [Index]
        public uint ID;
        public uint Flags;
        public float DefaultPitchMin;
        public float DefaultPitchMax;
        public float DefaultSpeedMin;
        public float DefaultSpeedMax;
        public float RandomizeFacingMin;
        public float RandomizeFacingMax;
        public float RandomizePitchMin;
        public float RandomizePitchMax;
        public float RandomizeSpeedMin;
        public float RandomizeSpeedMax;
        public float Gravity;
        public float MaxDuration;
        public float CollisionRadius;
        public byte UnkLegion;
    }
}
