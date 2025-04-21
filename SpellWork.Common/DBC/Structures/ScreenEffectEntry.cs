using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("ScreenEffect")]
    public sealed class ScreenEffectEntry
    {
        [Index]
        public int ID;
        //TODO
        public string Name;
        public int[] field04;
        public ushort field14;
        public ushort field16;
        public ushort field18;
        public ushort field1A;
        public byte field1C;
        public byte field1D;
        public byte field1E;
        public int field1F;
        public int field23;
        public int field27;
    }
}
