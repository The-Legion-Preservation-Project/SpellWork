using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellLevels")]
    public class SpellLevelsEntry
    {
        [Index]
        public uint ID;
        public ushort BaseLevel;
        public ushort MaxLevel;
        public ushort SpellLevel;
        public byte Difficulty;
        public byte MaxUsableLevel;
        [RelationField]
        public int SpellID;
    }
}
