using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("OverrideSpellData")]
    public sealed class OverrideSpellDataEntry
    {
        [Index]
        public int ID;
        [ArraySize(10)]
        public uint[] Spells;
        public uint PlayerActionbarFileDataID;
        public byte Flags;
    }
}
