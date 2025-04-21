using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("AreaGroupMember")]
    public sealed class AreaGroupMemberEntry
    {
        [Index]
        public int ID;
        public ushort AreaId;
        [RelationField]
        public ushort AreaGroupId;
    }
}
