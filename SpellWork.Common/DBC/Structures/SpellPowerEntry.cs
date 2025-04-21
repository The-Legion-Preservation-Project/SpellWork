using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellPower")]
    public sealed class SpellPowerEntry
    {
        public uint ManaCost;
        public float ManaCostPercentage;
        public float ManaCostPercentagePerSecond;
        public uint RequiredAura;
        public float HealthCostPercentage;
        public byte PowerIndex;
        public byte PowerType;
        [Index]
        public uint ID;
        public uint ManaCostPerLevel;
        public uint ManaCostPerSecond;
        public uint ManaCostAdditional;   // Spell uses [ManaCost, ManaCost+ManaCostAdditional] power - affects tooltip parsing as multiplier on SpellEffectEntry::EffectPointsPerResource
                                          //   only SPELL_EFFECT_WEAPON_DAMAGE_NOSCHOOL, SPELL_EFFECT_WEAPON_PERCENT_DAMAGE, SPELL_EFFECT_WEAPON_DAMAGE, SPELL_EFFECT_NORMALIZED_WEAPON_DMG
        public uint PowerDisplayID;
        public uint UnitPowerBarID;
        [RelationField]
        public uint SpellID;
    }
}
