using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("ItemEffect")]
    public sealed class ItemEffectEntry
    {
        [Index]
        public int ItemID;
        public int SpellID;
        public int Cooldown;
        public int CategoryCooldown;
        public short Charges;
        public ushort Category;
        public ushort ChrSpecializationID;
        public byte LegacySlotIndex;
        public byte TriggerType;
        [RelationField]
        public int ParentItemID;
    }
}
