using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellAuraRestrictions")]
    public class SpellAuraRestrictionsEntry
    {
        [Index]
        public int ID;
        public uint CasterAuraSpell;
        public uint TargetAuraSpell;
        public uint ExcludeCasterAuraSpell;
        public uint ExcludeTargetAuraSpell;
        public byte DifficultyID;
        public byte CasterAuraState;
        public byte TargetAuraState;
        public byte ExcludeCasterAuraState;
        public byte ExcludeTargetAuraState;
        [RelationField]
        public int SpellID;
    }
}
