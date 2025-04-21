using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellScaling")]
    public class SpellScalingEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        public ushort ScalesFromItemLevel;
        public int ScalingClass;
        public uint MinScalingLevel;
        public uint MaxScalingLevel;
    }
}
