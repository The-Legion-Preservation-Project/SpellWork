using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellClassOptions")]
    public class SpellClassOptionsEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        [ArraySize(4)]
        public uint[] SpellFamilyFlags;
        public byte SpellClassSet;
        public uint ModalNextSpell;
    }
}
