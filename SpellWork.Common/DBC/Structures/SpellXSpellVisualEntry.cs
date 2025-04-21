using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellXSpellVisual")]
    public sealed class SpellXSpellVisualEntry
    {
        public uint SpellVisualID;
        [Index]
        public uint ID;
        public float Chance;
        public ushort CasterPlayerConditionID;
        public ushort CasterUnitConditionID;
        public ushort PlayerConditionID;
        public ushort UnitConditionID;
        public uint IconFileDataID;
        public uint ActiveIconFileDataID;
        public byte Flags;
        public byte DifficultyID;
        public byte Priority;
        [RelationField]
        public int SpellID;
    }
}
