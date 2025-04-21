using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellReagents")]
    public class SpellReagentsEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        [ArraySize(8)]
        public uint[] Reagent;
        [ArraySize(8)]
        public ushort[] ReagentCount;
    }
}
