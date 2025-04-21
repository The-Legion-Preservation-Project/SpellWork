using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using SpellWork.Database;
using SpellWork.DBC.Structures;
using SpellWork.Extensions;
using SpellWork.GameTables;
using SpellWork.GameTables.Structures;
using System.Collections.Concurrent;

namespace SpellWork.Spell
{
    public class SpellInfo
    {
        public SpellEntry Spell { get; set; }
        public SpellAuraOptionsEntry AuraOptions { get; set; }
        public SpellAuraRestrictionsEntry AuraRestrictions { get; set; }
        public SpellCastingRequirementsEntry CastingRequirements { get; set; }
        public SpellCategoriesEntry Categories { get; set; }
        public SpellClassOptionsEntry ClassOptions { get; set; }
        public SpellCooldownsEntry Cooldowns { get; set; }
        public SpellEquippedItemsEntry EquippedItems { get; set; }
        public SpellInterruptsEntry Interrupts { get; set; }
        public SpellLevelsEntry Levels { get; set; }
        public SpellMiscEntry Misc { get; set; }
        public SpellReagentsEntry Reagents { get; set; }
        public List<SpellReagentsCurrencyEntry> ReagentsCurrency { get; } = new List<SpellReagentsCurrencyEntry>();
        public SpellScalingEntry Scaling { get; set; }
        public SpellShapeshiftEntry Shapeshift { get; set; }
        public List<SpellTargetRestrictionsEntry> TargetRestrictions { get; } = new List<SpellTargetRestrictionsEntry>();
        public SpellTotemsEntry Totems { get; set; }
        public SpellXSpellVisualEntry SpellXSpellVisual { get; set; }
        public List<SpellEffectEntry> Effects { get; } = new List<SpellEffectEntry>(32);
        public SpellProcsPerMinuteEntry ProcsPerMinute { get; set; }
        public SpellDescriptionVariablesEntry DescriptionVariables { get; set; }
        public SpellDurationEntry DurationEntry { get; set; }
        public SpellRangeEntry Range { get; set; }

        // Helper
        public readonly IDictionary<uint, SpellEffectInfo> SpellEffectInfoStore = new ConcurrentDictionary<uint, SpellEffectInfo>();

        #region SpellDuration
        public int Duration => DurationEntry?.Duration ?? 0;
        public int DurationPerLevel => DurationEntry?.DurationPerLevel ?? 0;
        public int MaxDuration => DurationEntry?.MaxDuration ?? 0;
        #endregion

        #region Spell
        public int ID => Spell.ID;
        public string Name => Spell.Name;
        public string Description => Spell.Description;
        public string Tooltip => Spell.AuraDescription;
        public uint MiscID => Misc?.ID ?? 0;
        #endregion

        #region SpellMisc
        // SpellMisc
        public uint Attributes => Misc?.Attributes[0] ?? 0;
        public uint AttributesEx => Misc?.Attributes[1] ?? 0;
        public uint AttributesEx2 => Misc?.Attributes[2] ?? 0;
        public uint AttributesEx3 => Misc?.Attributes[3] ?? 0;
        public uint AttributesEx4 => Misc?.Attributes[4] ?? 0;
        public uint AttributesEx5 => Misc?.Attributes[5] ?? 0;
        public uint AttributesEx6 => Misc?.Attributes[6] ?? 0;
        public uint AttributesEx7 => Misc?.Attributes[7] ?? 0;
        public uint AttributesEx8 => Misc?.Attributes[8] ?? 0;
        public uint AttributesEx9 => Misc?.Attributes[9] ?? 0;
        public uint AttributesEx10 => Misc?.Attributes[10] ?? 0;
        public uint AttributesEx11 => Misc?.Attributes[11] ?? 0;
        public uint AttributesEx12 => Misc?.Attributes[12] ?? 0;
        public uint AttributesEx13 => Misc?.Attributes[13] ?? 0;
        public float Speed => Misc?.Speed ?? 0;
        public int CastingTimeIndex => Misc?.CastingTimeIndex ?? 0;
        public uint ActiveIconFileDataID => Misc?.ActiveIconFileDataID ?? 0;
        public uint IconFileDataID => Misc?.IconFileDataID ?? 0;
        public int RangeIndex => Misc?.RangeIndex ?? 0;
        public uint SchoolMask => (uint)(Misc?.SchoolMask ?? 0);
        #endregion

        #region SpellClassOptions
        // SpellClassOptions
        public uint ModalNextSpell => ClassOptions?.ModalNextSpell ?? 0;
        public uint SpellFamilyName => ClassOptions?.SpellClassSet ?? 0;
        public uint[] SpellFamilyFlags => ClassOptions?.SpellFamilyFlags ?? new uint[4];
        #endregion

        #region SpellCategories
        // SpellCategories
        public int DamageClass => Categories?.DefenseType ?? 0;
        public int PreventionType => Categories?.PreventionType ?? 0;
        public uint Category => Categories?.Category ?? 0;
        public int Dispel => Categories?.DispelType ?? 0;
        public int Mechanic => Categories?.Mechanic ?? 0;
        #endregion

        #region SpellShapeshift
        // SpellShapeshift
        public ulong Stances => ((Shapeshift?.ShapeshiftMask[0] ?? 0) << 32) | (Shapeshift?.ShapeshiftMask[1] ?? 0);
        public ulong StancesNot => ((Shapeshift?.ShapeshiftExclude[0] ?? 0) << 32) | (Shapeshift?.ShapeshiftExclude[1] ?? 0);
        #endregion

        #region SpellCooldowns
        // SpellCooldowns
        public uint CategoryRecoveryTime => Cooldowns?.CategoryRecoveryTime ?? 0;
        public uint RecoveryTime => Cooldowns?.RecoveryTime ?? 0;
        public uint StartRecoveryTime => Cooldowns?.StartRecoveryTime ?? 0;
        public uint StartRecoveryCategory => Categories?.StartRecoveryCategory ?? 0;
        #endregion

        #region SpellAuraRestrictions
        // SpellAuraRestrictions
        public int CasterAuraState => AuraRestrictions?.CasterAuraState ?? 0;
        public int TargetAuraState => AuraRestrictions?.TargetAuraState ?? 0;
        public int CasterAuraStateNot => AuraRestrictions?.ExcludeCasterAuraState ?? 0;
        public int TargetAuraStateNot => AuraRestrictions?.ExcludeTargetAuraState ?? 0;
        public int CasterAuraSpell => (int)(AuraRestrictions?.CasterAuraSpell ?? 0);
        public int TargetAuraSpell => (int)(AuraRestrictions?.TargetAuraSpell ?? 0);
        public int ExcludeCasterAuraSpell => (int)(AuraRestrictions?.ExcludeCasterAuraSpell ?? 0);
        public int ExcludeTargetAuraSpell => (int)(AuraRestrictions?.ExcludeTargetAuraSpell ?? 0);
        #endregion

        #region SpellAuraOptions
        public uint ProcCharges => AuraOptions?.ProcCharges ?? 0;
        public uint ProcChance => AuraOptions?.ProcChance ?? 0;
        public uint ProcFlags => AuraOptions?.ProcTypeMask ?? 0;
        public uint CumulativeAura => AuraOptions?.CumulativeAura ?? 0;
        public uint ProcCooldown => AuraOptions?.ProcCategoryRecovery ?? 0;
        #endregion

        #region SpellLevels
        // SpellLevels
        public int BaseLevel => Levels?.BaseLevel ?? 0;
        public int MaxLevel => Levels?.MaxLevel ?? 0;
        public int SpellLevel => Levels?.SpellLevel ?? 0;
        public int MaxUsableLevel => Levels?.MaxUsableLevel ?? 0;
        #endregion

        #region EquippedItems
        // Equippeditems
        public int EquippedItemClass => EquippedItems?.EquippedItemClass ?? 0;
        public uint EquippedItemInventoryTypeMask => EquippedItems?.EquippedItemInventoryTypeMask ?? 0;
        public uint EquippedItemSubClassMask => EquippedItems?.EquippedItemSubClassMask ?? 0;
        #endregion

        #region SpellXSpellVisual
        public uint SpellVisualID => SpellXSpellVisual?.SpellVisualID ?? 0;
        #endregion

        #region CastingRequirements
        public uint RequiredAreasId => CastingRequirements?.RequiredAreasID ?? 0;
        public uint FacingCasterFlags => CastingRequirements?.FacingCasterFlags ?? 0;
        public uint MinFactionID => CastingRequirements?.MinFactionID ?? 0;
        public uint MinReputation => CastingRequirements?.MinReputation ?? 0;
        public uint RequiredAuraVision => CastingRequirements?.RequiredAuraVision ?? 0;
        public uint RequiresSpellFocus => CastingRequirements?.RequiresSpellFocus ?? 0;
        #endregion

        #region SpellProcsPerMinute
        public float BaseProcRate => ProcsPerMinute?.BaseProcRate ?? 0;
        public byte ProcsPerMinuteFlags => ProcsPerMinute?.Flags ?? 0;
        #endregion

        #region SpellInterrupts
        // SpellInterrupts
        public uint AuraInterruptFlags => Interrupts?.AuraInterruptFlags[0] ?? 0;
        public uint AuraInterruptFlags2 => Interrupts?.AuraInterruptFlags[1] ?? 0;
        public uint ChannelInterruptFlags => Interrupts?.ChannelInterruptFlags[0] ?? 0;
        public uint ChannelInterruptFlags2 => Interrupts?.ChannelInterruptFlags[1] ?? 0;
        public uint InterruptFlags => Interrupts?.InterruptFlags ?? 0;
        #endregion

        public List<string> AreaNames = new List<string>();
        public List<ushort> AreaIds = new List<ushort>();
        public List<string> MapNames = new List<string>();
        public List<ushort> MapIds = new List<ushort>();

        public void UpdateAreaRelatedFields()
        {
            if (RequiredAreasId > 0)
            {
                var areas = DBC.DBC.AreaGroupMember.Values.Where(ag => ag.AreaGroupId == RequiredAreasId).ToList();
                if (areas.Count > 0)
                {
                    foreach (var areaGroupMember in areas)
                    {
                        var areaId = areaGroupMember.AreaId;
                        if (!DBC.DBC.AreaTable.ContainsKey(areaId))
                            continue;

                        var areaEntry = DBC.DBC.AreaTable[areaId];
                        AreaNames.Add(areaEntry.AreaName);
                        AreaIds.Add(areaId);
                        MapIds.Add(areaEntry.MapID);
                        MapEntry map;
                        if (DBC.DBC.Map.TryGetValue(areaEntry.MapID, out map))
                            MapNames.Add(map.MapName);
                    }
                }
            }
        }

        public string ProcInfo
        {
            get
            {
                var i = 0;
                var sb = new StringBuilder();
                var proc = ProcFlags;
                while (proc != 0)
                {
                    if ((proc & 1) != 0)
                        sb.AppendFormatLine("  {0}", SpellEnums.ProcFlagDesc[i]);
                    ++i;
                    proc >>= 1;
                }

                return sb.ToString();
            }
        }

        public SpellInfo(SpellEntry spellEntry)
        {
            //SpellDescriptionVariablesEntry variables;
            Spell = spellEntry;
            //if (DBC.DBC.SpellDescriptionVariables.TryGetValue(spellEntry.DescriptionVariablesID, out variables))
            //    DescriptionVariables = variables;
        }

        public bool HasEffect(SpellEffects effect)
        {
            return Effects.Any(eff => eff != null && eff.Effect == (uint)effect);
        }

        public bool HasAura(AuraType aura)
        {
            return Effects.Any(eff => eff != null && eff.EffectAura == (uint)aura);
        }

        public bool HasTargetA(Targets target)
        {
            return Effects.Any(eff => eff != null && eff.ImplicitTarget[0] == (uint)target);
        }

        public bool HasTargetB(Targets target)
        {
            return Effects.Any(eff => eff != null && eff.ImplicitTarget[1] == (uint)target);
        }
    }

    public class SpellEffectInfo
    {
        public SpellEffectEntry SpellEffect { get; set; }

        public uint ID => SpellEffect.ID;

        public int SpellID => SpellEffect.SpellID;

        public uint DifficultyID => SpellEffect.DifficultyID;

        public uint Effect => SpellEffect.Effect;
        public uint EffectIndex => SpellEffect.EffectIndex;
        public uint EffectAttributes => SpellEffect.EffectAttributes;

        public uint EffectAura => SpellEffect.EffectAura;
        public uint EffectAuraPeriod => SpellEffect.EffectAuraPeriod;

        public int EffectBasePoints => SpellEffect.EffectBasePoints;

        public int EffectMiscValueA => SpellEffect.EffectMiscValues[0];
        public int EffectMiscValueB => SpellEffect.EffectMiscValues[1];

        public uint EffectSpellClassMaskA => SpellEffect.EffectSpellClassMask[0];
        public uint EffectSpellClassMaskB => SpellEffect.EffectSpellClassMask[1];
        public uint EffectSpellClassMaskC => SpellEffect.EffectSpellClassMask[2];
        public uint EffectSpellClassMaskD => SpellEffect.EffectSpellClassMask[3];

        public uint EffectTriggerSpell => SpellEffect.EffectTriggerSpell;

        public uint TargetA => SpellEffect.ImplicitTarget[0];
        public uint TargetB => SpellEffect.ImplicitTarget[1];

        public uint EffectRadiusIndex => SpellEffect.EffectRadiusIndex[0];
        public uint EffectRadiusMaxIndex => SpellEffect.EffectRadiusIndex[1];

        public uint EffectChainTargets => SpellEffect.EffectChainTargets;
        public int EffectDieSides => SpellEffect.EffectDieSides;
        public uint EffectItemType => SpellEffect.EffectItemType;
        public uint EffectMechanic => SpellEffect.EffectMechanic;

        public float EffectAmplitude => SpellEffect.EffectAmplitude;
        public float EffectBonusCoefficient => SpellEffect.EffectBonusCoefficient;
        public float EffectChainAmplitude => SpellEffect.EffectChainAmplitude;
        public float EffectPointsPerResource => SpellEffect.EffectPointsPerResource;
        public float EffectRealPointsPerLevel => SpellEffect.EffectRealPointsPerLevel;

        public float EffectPosFacing => SpellEffect.EffectPosFacing;
        public float BonusCoefficientFromAP => SpellEffect.BonusCoefficientFromAP;

        public SpellEffectInfo(SpellEffectEntry spellEffectEntry)
        {
            SpellEffect = spellEffectEntry;
        }
    }
}
