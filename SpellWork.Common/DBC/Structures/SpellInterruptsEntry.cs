using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellInterrupts")]
    public class SpellInterruptsEntry
    {
        [Index]
        public uint ID;
        public byte Difficulty;
        public ushort InterruptFlags;
        [ArraySize(2)]
        public uint[] AuraInterruptFlags;
        [ArraySize(2)]
        public uint[] ChannelInterruptFlags;
        [RelationField]
        public int SpellID;
    }
}
