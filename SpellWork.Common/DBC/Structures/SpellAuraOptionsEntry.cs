using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellAuraOptions")]
    public class SpellAuraOptionsEntry
    {
        [Index]
        public int ID;
        public uint ProcCharges;
        public uint ProcTypeMask;
        public uint ProcCategoryRecovery;
        public ushort CumulativeAura;
        public ushort SpellProcsPerMinuteID;
        public byte DifficultyID;
        public byte ProcChance;
        [RelationField]
        public int SpellID;
    }
}
