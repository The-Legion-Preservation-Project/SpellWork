using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellTargetRestrictions")]
    public sealed class SpellTargetRestrictionsEntry
    {
        [Index]
        public uint ID;
        public float ConeAngle;
        public float Width;
        public uint Targets;
        public ushort TargetCreatureType;
        public byte DifficultyID;
        public byte MaxAffectedTargets;
        public uint MaxTargetLevel;
        [RelationField]
        public int SpellID;
    }
}
