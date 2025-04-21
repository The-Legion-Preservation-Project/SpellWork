using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellEffect")]
    public class SpellEffectEntry
    {
        [Index]
        public uint ID;
        public uint Effect;
        public int EffectBasePoints;
        public uint EffectIndex;
        public uint EffectAura;
        public uint DifficultyID;
        public float EffectAmplitude;
        public uint EffectAuraPeriod;
        public float EffectBonusCoefficient;
        public float EffectChainAmplitude;
        public uint EffectChainTargets;
        public int EffectDieSides;
        public uint EffectItemType;
        public uint EffectMechanic;
        public float EffectPointsPerResource;
        public float EffectRealPointsPerLevel;
        public uint EffectTriggerSpell;
        public float EffectPosFacing;
        public uint EffectAttributes;
        public float BonusCoefficientFromAP;
        public float PvPMultiplier;
        public float Coefficient;
        public float Variance;
        public float ResourceCoefficient;
        public float GroupSizeBasePointsCoefficient;
        [ArraySize(4)]
        public uint[] EffectSpellClassMask;
        [ArraySize(2)]
        public int[] EffectMiscValues;
        [ArraySize(2)]
        public uint[] EffectRadiusIndex;
        [ArraySize(2)]
        public uint[] ImplicitTarget;
        [RelationField]
        public int SpellID;

        public SpellEffectScalingEntry SpellEffectScalingEntry { get; set; }

        public string MaxRadius
        {
            get
            {
                if (EffectRadiusIndex[1] == 0 || !DBC.SpellRadius.ContainsKey((int)EffectRadiusIndex[1]))
                    return string.Empty;

                return $"Max Radius (Id {EffectRadiusIndex[1]}) {DBC.SpellRadius[(int)EffectRadiusIndex[1]].Radius:F}" +
                       $" (Min: {DBC.SpellRadius[(int)EffectRadiusIndex[1]].RadiusMin:F} Max: {DBC.SpellRadius[(int)EffectRadiusIndex[1]].MaxRadius:F})";
            }
        }

        public string Radius
        {
            get
            {
                if (EffectRadiusIndex[0] == 0 || !DBC.SpellRadius.ContainsKey((int)EffectRadiusIndex[0]))
                    return string.Empty;

                return $"Radius (Id {EffectRadiusIndex[0]}) {DBC.SpellRadius[(int)EffectRadiusIndex[0]].Radius:F}" +
                       $" (Min: {DBC.SpellRadius[(int)EffectRadiusIndex[0]].RadiusMin:F} Max: {DBC.SpellRadius[(int)EffectRadiusIndex[0]].MaxRadius:F})";
            }
        }
    }
}
