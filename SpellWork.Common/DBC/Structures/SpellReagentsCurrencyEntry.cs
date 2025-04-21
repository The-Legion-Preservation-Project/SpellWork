using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellReagentsCurrency")]
    public class SpellReagentsCurrencyEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        public ushort CurrencyTypeID;
        public ushort CurrencyCount;
        [RelationField]
        public int SpellIdRelation;
    }
}
