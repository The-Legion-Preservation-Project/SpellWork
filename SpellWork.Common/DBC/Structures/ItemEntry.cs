using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("Item")]
    public sealed class ItemEntry
    {
        [Index]
        public int ID;
        public uint IconFileDataID;
        public byte ClassID;
        public byte SubclassID;
        public sbyte SoundOverrideSubclassID;
        public sbyte Material;
        public byte InventoryType;
        public byte SheatheType;
        public byte ItemGroupSoundsID;
    }
}
