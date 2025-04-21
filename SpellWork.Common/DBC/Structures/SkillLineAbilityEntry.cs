using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SkillLineAbility")]
    public sealed class SkillLineAbilityEntry
    {
        public ulong RaceMask;
        [Index]
        public uint ID;
        public int SpellID;
        public int SupercedesSpell;
        public ushort SkillLine;
        public ushort TrivialSkillLineRankHigh;
        public ushort TrivialSkillLineRankLow;
        public ushort UniqueBit;
        public ushort TradeSkillCategoryID;
        public byte NumSkillUps;
        public int ClassMask;
        public ushort MinSkillLineRank;
        public byte AquireMethod;
        public byte Flags;
        [RelationField]
        public ushort SkillLineRelation;
    }
}
