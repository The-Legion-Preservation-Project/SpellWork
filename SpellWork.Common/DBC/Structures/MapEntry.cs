using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("Map")]
    public sealed class MapEntry
    {
        [Index]
        public int ID;
        public string Directory;
        public string MapName;
        public string MapDescriptionHorde;
        public string MapDescriptionAlliance;
        public string PvpShortDescription;
        public string PvpLongDescription;
        [ArraySize(2)]
        public uint[] Flags;
        public float MinimapIconScale;
        [ArraySize(2)]
        public float[] CorpsePosition; // entrance coordinates in ghost mode  (in most cases = normal entrance)
        public ushort AreaTableID;
        public short LoadingScreenID;
        public short CorpseMapID; // map_id of entrance map in ghost mode (continent always and in most cases = normal entrance)
        public short TimeOfDayOverride;
        public short ParentMapID;
        public short CosmeticParentMapID;
        public short WindSettingsID;
        public byte InstanceType;
        public byte MapType;
        public byte ExpansionID;
        public byte MaxPlayers;
        public byte TimeOffset;
    }
}
