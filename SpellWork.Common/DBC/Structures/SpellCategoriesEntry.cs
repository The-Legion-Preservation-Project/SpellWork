using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellCategories")]
    public class SpellCategoriesEntry
    {
        [Index]
        public int ID;
        public ushort Category;
        public ushort StartRecoveryCategory;
        public ushort ChargeCategory;
        public byte DifficultyID;
        public byte DefenseType;
        public byte DispelType;
        public byte Mechanic;
        public byte PreventionType;
        [RelationField]
        public int SpellID;
    }
}
