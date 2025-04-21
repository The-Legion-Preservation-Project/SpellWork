using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellTotems")]
    public class SpellTotemsEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        [ArraySize(2)]
        public int[] Totem;
        [ArraySize(2)]
        public ushort[] RequiredTotemCategoryID;
    }
}
