using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellCastingRequirements")]
    public class SpellCastingRequirementsEntry
    {
        [Index]
        public int ID;
        public int SpellID;
        public ushort MinFactionID;
        public ushort RequiredAreasID;
        public ushort RequiresSpellFocus;
        public byte FacingCasterFlags;
        public byte MinReputation;
        public byte RequiredAuraVision;
    }
}
