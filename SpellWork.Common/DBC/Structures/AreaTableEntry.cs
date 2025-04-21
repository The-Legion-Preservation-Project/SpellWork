using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("AreaTable")]
    public sealed class AreaTableEntry
    {
        [Index]
        public uint ID;
        public string ZoneName;
        public string AreaName;
        [ArraySize(2)]
        public uint[] Flags;
        public float AmbientMultiplier;
        public ushort MapID;
        public ushort ParentAreaID;
        public short AreaBit;
        public ushort AmbienceID;
        public ushort ZoneMusic;
        public ushort IntroSound;
        [ArraySize(4)]
        public ushort[] LiquidTypeID;
        public ushort UWZoneMusic;
        public ushort UWAmbience;
        public ushort PvPCombatWorldStateID;
        public byte SoundProviderPref;
        public byte SoundProviderPrefUnderwater;
        public byte ExplorationLevel;
        public byte FactionGroupMask;
        public byte MountFlags;
        public byte WildBattlePetLevelMin;
        public byte WildBattlePetLevelMax;
        public byte WindSettingsID;
        public uint UWIntroSound;
    }
}
