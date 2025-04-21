using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellEquippedItems")]
    public class SpellEquippedItemsEntry
    {
        [Index]
        public uint ID;
        public int SpellID;
        public uint EquippedItemInventoryTypeMask;
        public uint EquippedItemSubClassMask;
        public byte EquippedItemClass;
    }
}
