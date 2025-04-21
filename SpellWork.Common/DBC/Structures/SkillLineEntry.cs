using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SkillLine")]
    public sealed class SkillLineEntry
    {
        [Index]
        public int ID;
        public string DisplayName;
        public string Description;
        public string AlternateVerb;
        public ushort Flags;
        public byte CategoryID;
        public byte CanLink;
        public uint IconFileDataID;
        public uint ParentSkillLineID;
    }
}
