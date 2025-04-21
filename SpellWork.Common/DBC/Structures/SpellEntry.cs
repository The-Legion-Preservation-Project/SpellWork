using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("Spell")]
    public sealed class SpellEntry
    {
        [Index]
        public int ID;
        public string Name;
        public string NameSubtext;
        public string Description;
        public string AuraDescription;
    }
}
