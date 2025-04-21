using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellShapeshift")]
    public class SpellShapeshiftEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        [ArraySize(2)]
        public uint[] ShapeshiftExclude;
        [ArraySize(2)]
        public uint[] ShapeshiftMask;
        public int StanceBarOrder;
    }
}
