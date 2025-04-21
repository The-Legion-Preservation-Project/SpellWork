using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("RandPropPoints")]
    public sealed class RandPropPointsEntry
    {
        [Index]
        public int ID;
        [ArraySize(5)]
        public uint[] Epic;
        [ArraySize(5)]
        public uint[] Superior;
        [ArraySize(5)]
        public uint[] Good;
    }
}
