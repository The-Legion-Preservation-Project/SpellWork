using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpellWork.DBC.Structures;
using SpellWork.GameTables;
using SpellWork.GameTables.Structures;
using SpellWork.Spell;

namespace SpellWork.Extensions
{
    public static class SpellInfoExtensions
    {
        private const string Separator = "=================================================";

        public static void Write(this SpellInfo spellInfo, RichTextBox rtb)
        {
            rtb.Clear();

            rtb.SetBold();
            rtb.AppendFormatLine("ID - {0} {1}{2}",
                spellInfo.ID, spellInfo.Name, spellInfo.Scaling != null ? $" (Level {DBC.DBC.SelectedLevel})" : string.Empty);
            rtb.SetDefaultStyle();

            rtb.AppendFormatLine(Separator);

            rtb.AppendLine(spellInfo.Description);
            rtb.AppendFormatLineIfNotNull("ToolTip: {0}", spellInfo.Tooltip);
            if (spellInfo.DescriptionVariables != null)
            {
                rtb.AppendLine("Description variables:");
                rtb.AppendLine(spellInfo.DescriptionVariables.Variables);
            }

            rtb.AppendFormatLineIfNotNull("Modal Next Spell: {0}", spellInfo.ModalNextSpell);
            if (!string.IsNullOrEmpty(spellInfo.Spell.Description) && !string.IsNullOrEmpty(spellInfo.Spell.AuraDescription) && spellInfo.ModalNextSpell != 0)
                rtb.AppendFormatLine(Separator);

            #region Triggered by ...
            var addline = false;
            if (DBC.DBC.SpellTriggerStore.ContainsKey(spellInfo.Spell.ID))
            {
                foreach (var procSpellId in DBC.DBC.SpellTriggerStore[spellInfo.Spell.ID])
                {
                    var procname = "Spell Not Found";
                    if (DBC.DBC.Spell.ContainsKey(procSpellId))
                        procname = DBC.DBC.Spell[procSpellId].Name;
                    rtb.SetStyle(Color.Blue, FontStyle.Bold);

                    rtb.AppendFormatLine("Triggered by spell: ({0}) {1}", procSpellId, procname);
                    rtb.SetDefaultStyle();
                    addline = true;
                }
            }
            if (addline)
                rtb.AppendFormatLine(Separator);
            #endregion

            rtb.AppendFormatLine($"Category = { spellInfo.Category }, IconFileDataID = { spellInfo.IconFileDataID }, ActiveIconFileDataID = { spellInfo.ActiveIconFileDataID }, SpellVisualID = { spellInfo.SpellVisualID }");

            rtb.AppendFormatLine("Family {0} ({1}), flag [0] 0x{2:X8} [1] 0x{3:X8} [2] 0x{4:X8} [3] 0x{5:X8}",
                    (SpellFamilyNames)spellInfo.SpellFamilyName, spellInfo.SpellFamilyName,
                    spellInfo.SpellFamilyFlags[0], spellInfo.SpellFamilyFlags[1], spellInfo.SpellFamilyFlags[2], spellInfo.SpellFamilyFlags[3]);

            #region Modified by ...
            foreach (var eff in
                    from s in DBC.DBC.SpellInfoStore.Values
                    where s.SpellFamilyName == spellInfo.SpellFamilyName
                    from eff in s.Effects
                    where eff != null && ((eff.EffectSpellClassMask[0] & spellInfo.SpellFamilyFlags[0]) != 0 ||
                          (eff.EffectSpellClassMask[1] & spellInfo.SpellFamilyFlags[1]) != 0 ||
                          (eff.EffectSpellClassMask[2] & spellInfo.SpellFamilyFlags[2]) != 0 ||
                          (eff.EffectSpellClassMask[3] & spellInfo.SpellFamilyFlags[3]) != 0)
                    select eff)
            {
                rtb.SetStyle(Color.Blue, FontStyle.Bold);
                rtb.AppendFormatLine("Modified by {0} ({1})",
                    DBC.DBC.SpellInfoStore[eff.SpellID].Spell.Name, eff.SpellID);
            }
            #endregion

            rtb.AppendLine();

            rtb.AppendFormatLine("SpellSchoolMask = {0} ({1})", (SpellSchoolMask)spellInfo.SchoolMask, spellInfo.SchoolMask);
            rtb.AppendFormatLine("DamageClass = {0} ({1})", spellInfo.DamageClass, (SpellDmgClass)spellInfo.DamageClass);
            rtb.AppendFormatLine("PreventionType = {0} ({1})", spellInfo.PreventionType, (SpellPreventionType)spellInfo.PreventionType);

            #region Attributes
            if (spellInfo.Misc != null && !spellInfo.Misc.Attributes.All(a => a == 0))
                rtb.AppendLine(Separator);

            if (spellInfo.Attributes != 0)
                rtb.AppendFormatLine("spellInfo.Attributes: 0x{0:X8} ({1})", spellInfo.Attributes, (SpellAtribute)spellInfo.Attributes);
            if (spellInfo.AttributesEx != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx1: 0x{0:X8} ({1})", spellInfo.AttributesEx, (SpellAtributeEx)spellInfo.AttributesEx);
            if (spellInfo.AttributesEx2 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx2: 0x{0:X8} ({1})", spellInfo.AttributesEx2, (SpellAtributeEx2)spellInfo.AttributesEx2);
            if (spellInfo.AttributesEx3 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx3: 0x{0:X8} ({1})", spellInfo.AttributesEx3, (SpellAtributeEx3)spellInfo.AttributesEx3);
            if (spellInfo.AttributesEx4 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx4: 0x{0:X8} ({1})", spellInfo.AttributesEx4, (SpellAtributeEx4)spellInfo.AttributesEx4);
            if (spellInfo.AttributesEx5 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx5: 0x{0:X8} ({1})", spellInfo.AttributesEx5, (SpellAtributeEx5)spellInfo.AttributesEx5);
            if (spellInfo.AttributesEx6 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx6: 0x{0:X8} ({1})", spellInfo.AttributesEx6, (SpellAtributeEx6)spellInfo.AttributesEx6);
            if (spellInfo.AttributesEx7 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx7: 0x{0:X8} ({1})", spellInfo.AttributesEx7, (SpellAtributeEx7)spellInfo.AttributesEx7);
            if (spellInfo.AttributesEx8 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx8: 0x{0:X8} ({1})", spellInfo.AttributesEx8, (SpellAtributeEx8)spellInfo.AttributesEx8);
            if (spellInfo.AttributesEx9 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx9: 0x{0:X8} ({1})", spellInfo.AttributesEx9, (SpellAtributeEx9)spellInfo.AttributesEx9);
            if (spellInfo.AttributesEx10 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx10: 0x{0:X8} ({1})", spellInfo.AttributesEx10, (SpellAtributeEx10)spellInfo.AttributesEx10);
            if (spellInfo.AttributesEx11 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx11: 0x{0:X8} ({1})", spellInfo.AttributesEx11, (SpellAtributeEx11)spellInfo.AttributesEx11);
            if (spellInfo.AttributesEx12 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx12: 0x{0:X8} ({1})", spellInfo.AttributesEx12, (SpellAtributeEx12)spellInfo.AttributesEx12);
            if (spellInfo.AttributesEx13 != 0)
                rtb.AppendFormatLine("spellInfo.AttributesEx13: 0x{0:X8} ({1})", spellInfo.AttributesEx13, (SpellAtributeEx13)spellInfo.AttributesEx13);

            rtb.AppendLine(Separator);
            #endregion

            if (spellInfo.TargetRestrictions != null)
            {
                foreach (var targetRestriction in spellInfo.TargetRestrictions)
                {
                    if (targetRestriction.Targets != 0)
                        rtb.AppendFormatLine("Targets Mask = 0x{0:X8} ({1})", targetRestriction.Targets, (SpellCastTargetFlags)targetRestriction.Targets);

                    if (targetRestriction.TargetCreatureType != 0)
                        rtb.AppendFormatLine("Creature Type Mask = 0x{0:X8} ({1})",
                            targetRestriction.TargetCreatureType, (CreatureTypeMask)targetRestriction.TargetCreatureType);

                    if (targetRestriction.MaxAffectedTargets != 0)
                        rtb.AppendFormatLine("MaxAffectedTargets: {0}", targetRestriction.MaxAffectedTargets);
                }
            }

            if (spellInfo.Stances != 0)
                rtb.AppendFormatLine("Stances: {0}", (ShapeshiftFormMask)spellInfo.Stances);

            if (spellInfo.StancesNot != 0)
                rtb.AppendFormatLine("Stances Not: {0}", (ShapeshiftFormMask)spellInfo.StancesNot);

            // Skills
            {
                var query = DBC.DBC.SkillLineAbility.Where(skl => skl.Value.SpellID == spellInfo.Spell.ID).ToArray();
                if (query.Length != 0)
                {
                    var skill = query.First().Value;
                    var line = DBC.DBC.SkillLine[skill.SkillLine];

                    rtb.AppendFormatLine(@"Skill (Id {0}) ""{1}""", skill.SkillLine, line.DisplayName);
                    rtb.AppendFormat("    MinSkillLineRank {0}", skill.MinSkillLineRank);

                    rtb.AppendFormat(", SupercedesSpell = {0}, MinMaxValue ({1}, {2})", skill.SupercedesSpell,
                        skill.TrivialSkillLineRankLow, skill.TrivialSkillLineRankHigh);
                    rtb.AppendFormat(", NumSkillups ({0})", skill.NumSkillUps);
                }
            }

            // SpellReagents
            {
                var printedHeader = false;
                for (var i = 0; spellInfo.Reagents != null && i < spellInfo.Reagents.ReagentCount.Length; ++i)
                {
                    if (spellInfo.Reagents.ReagentCount[i] == 0 || spellInfo.Reagents.Reagent[i] == 0)
                        continue;

                    if (!printedHeader)
                    {
                        rtb.AppendLine();
                        rtb.Append("Reagents:");
                        printedHeader = true;
                    }

                    rtb.AppendFormat("  {0} x{1}", spellInfo.Reagents.Reagent[i], spellInfo.Reagents.ReagentCount[i]);
                }

                if (printedHeader)
                    rtb.AppendLine();
            }

            // SpellReagentsCurrency
            {
                var printedHeader = false;
                foreach (var reagentsCurrency in spellInfo.ReagentsCurrency)
                {
                    if (!printedHeader)
                    {
                        rtb.AppendLine();
                        rtb.Append("ReagentsCurrency:");
                        printedHeader = true;
                    }

                    rtb.AppendFormat("  {0} x{1}", reagentsCurrency.CurrencyTypeID, reagentsCurrency.CurrencyCount);
                }

                if (printedHeader)
                    rtb.AppendLine();
            }

            rtb.AppendFormatLine("Spell Level = {0}, base {1}, max {2}, max usable {3}",
                spellInfo.SpellLevel, spellInfo.BaseLevel, spellInfo.MaxLevel, spellInfo.MaxUsableLevel);

            if (spellInfo.EquippedItemClass != 0)
            {
                rtb.AppendFormatLine("EquippedItemClass = {0} ({1})", spellInfo.EquippedItemClass, (ItemClass)spellInfo.EquippedItemClass);

                if (spellInfo.EquippedItemSubClassMask != 0)
                {
                    switch ((ItemClass)spellInfo.EquippedItemClass)
                    {
                        case ItemClass.WEAPON:
                            rtb.AppendFormatLine("    SubClass mask 0x{0:X8} ({1})", spellInfo.EquippedItemSubClassMask, (ItemSubClassWeaponMask)spellInfo.EquippedItemSubClassMask);
                            break;
                        case ItemClass.ARMOR:
                            rtb.AppendFormatLine("    SubClass mask 0x{0:X8} ({1})", spellInfo.EquippedItemSubClassMask, (ItemSubClassArmorMask)spellInfo.EquippedItemSubClassMask);
                            break;
                        case ItemClass.MISC:
                            rtb.AppendFormatLine("    SubClass mask 0x{0:X8} ({1})", spellInfo.EquippedItemSubClassMask, (ItemSubClassMiscMask)spellInfo.EquippedItemSubClassMask);
                            break;
                    }
                }

                if (spellInfo.EquippedItemInventoryTypeMask != 0)
                    rtb.AppendFormatLine("    InventoryType mask = 0x{0:X8} ({1})", spellInfo.EquippedItemInventoryTypeMask, (InventoryTypeMask)spellInfo.EquippedItemInventoryTypeMask);
            }

            rtb.AppendLine();
            rtb.AppendFormatLine("Category = {0}", spellInfo.Category);
            rtb.AppendFormatLine("DispelType = {0} ({1})", spellInfo.Dispel, (DispelType)spellInfo.Dispel);
            rtb.AppendFormatLine("Mechanic = {0} ({1})", spellInfo.Mechanic, (Mechanics)spellInfo.Mechanic);

            if (spellInfo.Range != null)
            {
                rtb.AppendFormatLine("SpellRange: (Id {0}) \"{1}\":", spellInfo.RangeIndex, spellInfo.Range.DisplayName);
                rtb.AppendFormatLine("    MinRangeNegative = {0}, MinRangePositive = {1}", spellInfo.Range.MinRange[0], spellInfo.Range.MinRange[1]);
                rtb.AppendFormatLine("    MaxRangeNegative = {0}, MaxRangePositive = {1}", spellInfo.Range.MaxRange[0], spellInfo.Range.MaxRange[1]);
            }

            if (spellInfo.Misc != null)
                rtb.AppendFormatLineIfNotNull("Speed {0:F}", spellInfo.Speed);

            rtb.AppendFormatLineIfNotNull("Stackable up to {0}", spellInfo.CumulativeAura);

            if (spellInfo.CastingTimeIndex != 0)
            {
                var castTimeEntry = DBC.DBC.SpellCastTimes[spellInfo.CastingTimeIndex];

                var level = DBC.DBC.SelectedLevel;
                if (spellInfo.Scaling != null && level > spellInfo.Scaling.MaxScalingLevel)
                    level = spellInfo.Scaling.MaxScalingLevel;

                if (((SpellAtributeEx13)spellInfo.AttributesEx13).HasFlag(SpellAtributeEx13.SPELL_ATTR13_UNK17))
                    level *= 5;

                if (spellInfo.Scaling != null && level > spellInfo.Scaling.MaxScalingLevel)
                    level = spellInfo.Scaling.MaxScalingLevel;

                if (spellInfo.Levels != null && spellInfo.Levels.BaseLevel != 0)
                    level -= spellInfo.Levels.BaseLevel;

                if (spellInfo.Scaling != null && level < spellInfo.Scaling.MinScalingLevel)
                    level = spellInfo.Scaling.MinScalingLevel;

                var castTime = castTimeEntry.CastTime + castTimeEntry.CastTimePerLevel * level;
                if (castTime < castTimeEntry.MinCastTime)
                    castTime = castTimeEntry.MinCastTime;

                rtb.AppendFormatLine("Cast Time: (ID {0}): {1}", spellInfo.CastingTimeIndex, castTime);
            }

            if (spellInfo.RecoveryTime != 0 || spellInfo.CategoryRecoveryTime != 0 || spellInfo.StartRecoveryCategory != 0)
            {
                rtb.AppendFormatLine("RecoveryTime: {0} ms, CategoryRecoveryTime: {1} ms", spellInfo.RecoveryTime, spellInfo.CategoryRecoveryTime);
                rtb.AppendFormatLine("StartRecoveryCategory = {0}, StartRecoveryTime = {1:F} ms", spellInfo.StartRecoveryCategory, spellInfo.StartRecoveryTime);
            }

            if (spellInfo.DurationEntry != null)
                rtb.AppendFormatLine("Duration {0}, {1}, {2}", spellInfo.Duration, spellInfo.DurationPerLevel, spellInfo.MaxDuration);

            if (spellInfo.Interrupts != null)
            {
                rtb.AppendFormatLine("Interrupt Flags: 0x{0:X8} ({1})", spellInfo.Interrupts.InterruptFlags, (SpellInterruptFlags)spellInfo.Interrupts.InterruptFlags);
                rtb.AppendFormatLine("AuraInterrupt Flags: 0x{0:X8} ({1}), 0x{2:X8} ({3})", spellInfo.Interrupts.AuraInterruptFlags[0], (SpellAuraInterruptFlags)spellInfo.Interrupts.AuraInterruptFlags[0], spellInfo.Interrupts.AuraInterruptFlags[1], (SpellAuraInterruptFlags)spellInfo.Interrupts.AuraInterruptFlags[1]);
                rtb.AppendFormatLine("ChannelInterrupt Flags: 0x{0:X8} ({1}), 0x{2:X8} ({3})", spellInfo.Interrupts.ChannelInterruptFlags[0], (SpellChannelInterruptFlags)spellInfo.Interrupts.ChannelInterruptFlags[0], spellInfo.Interrupts.ChannelInterruptFlags[1], (SpellChannelInterruptFlags)spellInfo.Interrupts.ChannelInterruptFlags[1]);
            }

            if (spellInfo.CasterAuraState != 0)
                rtb.AppendFormatLine("CasterAuraState = {0} ({1})", spellInfo.CasterAuraState, (AuraState)spellInfo.CasterAuraState);

            if (spellInfo.TargetAuraState != 0)
                rtb.AppendFormatLine("TargetAuraState = {0} ({1})", spellInfo.TargetAuraState, (AuraState)spellInfo.TargetAuraState);

            if (spellInfo.CasterAuraStateNot != 0)
                rtb.AppendFormatLine("CasterAuraStateNot = {0} ({1})", spellInfo.CasterAuraStateNot, (AuraState)spellInfo.CasterAuraStateNot);

            if (spellInfo.TargetAuraStateNot != 0)
                rtb.AppendFormatLine("TargetAuraStateNot = {0} ({1})", spellInfo.TargetAuraStateNot, (AuraState)spellInfo.TargetAuraStateNot);

            if (spellInfo.CasterAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(spellInfo.CasterAuraSpell))
                    rtb.AppendFormatLine("  Caster Aura Spell ({0}) {1}", spellInfo.CasterAuraSpell, DBC.DBC.SpellInfoStore[spellInfo.CasterAuraSpell].Spell.Name);
                else
                    rtb.AppendFormatLine("  Caster Aura Spell ({0}) ?????", spellInfo.CasterAuraSpell);
            }

            if (spellInfo.TargetAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(spellInfo.TargetAuraSpell))
                    rtb.AppendFormatLine("  Target Aura Spell ({0}) {1}", spellInfo.TargetAuraSpell, DBC.DBC.SpellInfoStore[spellInfo.TargetAuraSpell].Spell.Name);
                else
                    rtb.AppendFormatLine("  Target Aura Spell ({0}) ?????", spellInfo.TargetAuraSpell);
            }

            if (spellInfo.ExcludeCasterAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(spellInfo.ExcludeCasterAuraSpell))
                    rtb.AppendFormatLine("  Ex Caster Aura Spell ({0}) {1}", spellInfo.ExcludeCasterAuraSpell, DBC.DBC.SpellInfoStore[spellInfo.ExcludeCasterAuraSpell].Spell.Name);
                else
                    rtb.AppendFormatLine("  Ex Caster Aura Spell ({0}) ?????", spellInfo.ExcludeCasterAuraSpell);
            }

            if (spellInfo.ExcludeTargetAuraSpell != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey(spellInfo.ExcludeTargetAuraSpell))
                    rtb.AppendFormatLine("  Ex Target Aura Spell ({0}) {1}", spellInfo.ExcludeTargetAuraSpell, DBC.DBC.SpellInfoStore[spellInfo.ExcludeTargetAuraSpell].Spell.Name);
                else
                    rtb.AppendFormatLine("  Ex Target Aura Spell ({0}) ?????", spellInfo.ExcludeTargetAuraSpell);
            }

            if (spellInfo.RequiredAreasId > 0)
            {
                var areas = DBC.DBC.AreaGroupMember.Values.Where(ag => ag.AreaGroupId == spellInfo.RequiredAreasId).ToList();
                if (areas.Count == 0)
                    rtb.AppendFormatLine("Cannot find area group id {0} in AreaGroupMember.db2!", spellInfo.RequiredAreasId);
                else
                {
                    rtb.AppendLine();
                    rtb.SetBold();
                    rtb.AppendLine("Allowed areas:");
                    foreach (var areaGroupMember in areas)
                    {
                        var areaId = areaGroupMember.AreaId;
                        if (!DBC.DBC.AreaTable.ContainsKey(areaId))
                            continue;

                        var areaEntry = DBC.DBC.AreaTable[areaId];

                        MapEntry map;
                        bool hasMapData = DBC.DBC.Map.TryGetValue(areaEntry.MapID, out map);

                        rtb.AppendFormatLine("{0} - {1} (Map: {2} - {3})", areaId, areaEntry.AreaName, areaEntry.MapID, hasMapData ? map.MapName : "");
                    }

                    rtb.AppendLine();
                }
            }

            rtb.AppendFormatLineIfNotNull("Requires Spell Focus {0}", spellInfo.RequiresSpellFocus);

            if (Math.Abs(spellInfo.BaseProcRate) > 1.0E-5f)
            {
                rtb.SetBold();
                rtb.AppendFormatLine("PPM flag 0x{0:X2} BaseRate {1}", spellInfo.ProcsPerMinuteFlags, spellInfo.BaseProcRate);
                rtb.SetDefaultStyle();
            }

            if (spellInfo.ProcFlags != 0)
            {
                rtb.SetBold();
                rtb.AppendFormatLine("Proc flag 0x{0:X8}, chance: {1}%, charges: {2}, cooldown: {3}",
                    spellInfo.ProcFlags, spellInfo.ProcChance, spellInfo.ProcCharges, spellInfo.ProcCooldown);
                rtb.SetDefaultStyle();
                rtb.AppendFormatLine(Separator);
                rtb.AppendText(spellInfo.ProcInfo);
            }
            else
                rtb.AppendFormatLine("Chance = {0}, charges - {1}", spellInfo.ProcChance, spellInfo.ProcCharges);

            rtb.AppendLine(Separator);
            foreach (var effect in spellInfo.Effects)
                spellInfo.AppendEffectInfo(rtb, effect);

            spellInfo.AppendItemInfo(rtb);

            spellInfo.AppendSpellVisualInfo();
        }
        
        private static void AppendEffectInfo(this SpellInfo spellInfo, RichTextBox rtb, SpellEffectEntry effect)
        {
            rtb.SetBold();
            rtb.AppendFormatLine($"Effect { effect.EffectIndex }: Id { effect.Effect } ({ (SpellEffects)effect.Effect })");
            rtb.SetBold();
            rtb.AppendFormatLine($"Difficulty: Id { effect.DifficultyID } ({ (Difficulty)effect.DifficultyID })");
            rtb.SetDefaultStyle();

            var value = 0.0f;

            if (effect.SpellEffectScalingEntry != null &&
                Math.Abs(effect.SpellEffectScalingEntry.Coefficient) > 1.0E-5f &&
                spellInfo.Scaling != null &&
                spellInfo.Scaling.ScalingClass != 0)
            {
                var level = (int)(DBC.DBC.SelectedLevel - 1);
                if ((spellInfo.AttributesEx11 & (uint)SpellAtributeEx11.SPELL_ATTR11_SCALES_WITH_ITEM_LEVEL) == 0)
                {
                    if ((spellInfo.AttributesEx10 & (uint)SpellAtributeEx10.SPELL_ATTR10_USE_SPELL_BASE_LEVEL_FOR_SCALING) != 0)
                        level = spellInfo.BaseLevel;
                }
                else
                    level = (int)DBC.DBC.SelectedItemLevel;

                if (spellInfo.Scaling.MaxScalingLevel != 0 && spellInfo.Scaling.MaxScalingLevel < level)
                    level = (int)spellInfo.Scaling.MaxScalingLevel;

                if (level < 1)
                    level = 1;

                if (spellInfo.Scaling.ScalingClass != 0)
                {

                    if (spellInfo.Scaling.ScalesFromItemLevel == 0)
                    {
                        if ((spellInfo.AttributesEx11 & (uint)SpellAtributeEx11.SPELL_ATTR11_SCALES_WITH_ITEM_LEVEL) == 0)
                        {
                            var gtScaling = GameTable<GtSpellScalingEntry>.GetRecord(level);
                            Debug.Assert(gtScaling != null);
                            value = gtScaling.GetColumnForClass(spellInfo.Scaling.ScalingClass);
                        }
                        else if (DBC.DBC.RandPropPoints.ContainsKey(level))
                        {
                            var randPropPoints = DBC.DBC.RandPropPoints[level];
                            value = randPropPoints.Superior[0];
                        }
                    }
                    else
                    {
                        if (DBC.DBC.RandPropPoints.ContainsKey(spellInfo.Scaling.ScalesFromItemLevel))
                        {
                            var randPropPoints = DBC.DBC.RandPropPoints[spellInfo.Scaling.ScalesFromItemLevel];
                            value = randPropPoints.Superior[0];
                        }
                    }

                    // if (level < Scaling.CastTimeMaxLevel && Scaling.CastTimeMax != 0)
                    //     value *= (float)(Scaling.CastTimeMin + (level - 1) * (Scaling.CastTimeMax - Scaling.CastTimeMin) / (Scaling.CastTimeMaxLevel - 1)) / (Scaling.CastTimeMax);

                    // if (level < Scaling.NerfMaxLevel)
                    //     value *= ((((1.0f - Scaling.NerfFactor) * (level - 1)) / (Scaling.NerfMaxLevel - 1)) + Scaling.NerfFactor);
                }

                value *= effect.SpellEffectScalingEntry.Coefficient;
                if (Math.Abs(value) > 1.0E-5f && value < 1.0f)
                    value = 1.0f;

                if (Math.Abs(effect.SpellEffectScalingEntry.Variance) > 1.0E-5f)
                {
                    var delta = Math.Abs(value * effect.SpellEffectScalingEntry.Variance * 0.5f);
                    rtb.AppendFormat("BasePoints = {0:F} to {1:F}", value - delta, value + delta);
                }
                else
                    rtb.AppendFormat("BasePoints = {0:F}", value);

                if (Math.Abs(effect.SpellEffectScalingEntry.ResourceCoefficient) > 1.0E-5f)
                    rtb.AppendFormatIfNotNull(" + combo * {0:F}", effect.SpellEffectScalingEntry.ResourceCoefficient * value);
            }
            else
            {
                rtb.AppendFormat("BasePoints = {0}", effect.EffectBasePoints + (effect.EffectDieSides == 0 ? 0 : 1));

                if (Math.Abs(effect.EffectRealPointsPerLevel) > 1.0E-5f)
                    rtb.AppendFormat(" + Level * {0:F}", effect.EffectRealPointsPerLevel);

                if (effect.EffectDieSides > 1)
                {
                    if (Math.Abs(effect.EffectRealPointsPerLevel) > 1.0E-5f)
                        rtb.AppendFormat(" to {0} + lvl * {1:F}",
                            effect.EffectBasePoints + effect.EffectDieSides, effect.EffectRealPointsPerLevel);
                    else
                        rtb.AppendFormat(" to {0}", effect.EffectBasePoints + effect.EffectDieSides);
                }

                rtb.AppendFormatIfNotNull(" + resource * {0:F}", effect.EffectPointsPerResource);
            }

            if (effect.EffectBonusCoefficient > 1.0E-5f)
                rtb.AppendFormat(" + spellPower * {0}", effect.EffectBonusCoefficient);

            if (effect.BonusCoefficientFromAP > 1.0E-5)
                rtb.AppendFormat(" + AP * {0}", effect.BonusCoefficientFromAP);

            // if (Math.Abs(effect.DamageMultiplier - 1.0f) > 1.0E-5f)
            //     rtb.AppendFormat(" x {0:F}", effect.DamageMultiplier);

            // rtb.AppendFormatIfNotNull("  Multiple = {0:F}", effect.ValueMultiplier);
            rtb.AppendLine();

            rtb.AppendFormatLine("Targets ({0}, {1}) ({2}, {3})",
                effect.ImplicitTarget[0], effect.ImplicitTarget[1],
                (Targets)effect.ImplicitTarget[0], (Targets)effect.ImplicitTarget[1]);

            AuraModTypeName(rtb, effect);

            var classMask = effect.EffectSpellClassMask;

            if (classMask[0] != 0 || classMask[1] != 0 || classMask[2] != 0 || classMask[3] != 0)
            {
                rtb.AppendFormatLine("SpellClassMask = {0:X8} {1:X8} {2:X8} {3:X8}", classMask[0], classMask[1], classMask[2], classMask[3]);

                var query = from spell in DBC.DBC.SpellInfoStore.Values
                            where spell.SpellFamilyName == spellInfo.SpellFamilyName && spell.SpellFamilyFlags.ContainsElement(classMask)
                            join sk in DBC.DBC.SkillLineAbility.Values on spell.ID equals sk.SpellID into temp
                            from skill in temp.DefaultIfEmpty(new SkillLineAbilityEntry())
                            select new
                            {
                                SpellID = spell.Spell.ID,
                                SpellName = spell.Spell.Name,
                                SkillId = skill.SkillLine
                            };

                foreach (var row in query)
                {
                    if (row.SkillId > 0)
                    {
                        rtb.SelectionColor = Color.Blue;
                        rtb.AppendFormatLine("\t+ {0} - {1}", row.SpellID, row.SpellName);
                    }
                    else
                    {
                        rtb.SelectionColor = Color.Red;
                        rtb.AppendFormatLine("\t- {0} - {1}", row.SpellID, row.SpellName);
                    }
                    rtb.SelectionColor = Color.Black;
                }
            }

            rtb.AppendFormatLineIfNotNull("{0}", effect.Radius);
            rtb.AppendFormatLineIfNotNull("{0}", effect.MaxRadius);

            // append trigger spell
            var trigger = effect.EffectTriggerSpell;
            if (trigger != 0)
            {
                if (DBC.DBC.SpellInfoStore.ContainsKey((int)trigger))
                {
                    var triggerSpell = DBC.DBC.SpellInfoStore[(int)trigger];
                    rtb.SetStyle(Color.Blue, FontStyle.Bold);
                    rtb.AppendFormatLine("   Trigger spell ({0}) {1}. Chance = {2}", trigger, triggerSpell.Spell.Name, spellInfo.ProcChance);
                    rtb.AppendFormatLineIfNotNull("   Description: {0}", triggerSpell.Spell.Description);
                    rtb.AppendFormatLineIfNotNull("   ToolTip: {0}", triggerSpell.Spell.AuraDescription);
                    rtb.SetDefaultStyle();
                    if (triggerSpell.ProcFlags != 0)
                    {
                        rtb.AppendFormatLine("Charges - {0}", triggerSpell.ProcCharges);
                        rtb.AppendLine(Separator);
                        rtb.AppendLine(triggerSpell.ProcInfo);
                        rtb.AppendLine(Separator);
                    }
                }
                else
                    rtb.AppendFormatLine("Trigger spell ({0}) Not found, Chance = {1}", trigger, spellInfo.ProcChance);
            }

            rtb.AppendFormatLineIfNotNull("EffectChainTargets = {0}", effect.EffectChainTargets);
            rtb.AppendFormatLineIfNotNull("EffectItemType = {0}", effect.EffectItemType);

            if ((Mechanics)effect.EffectMechanic != Mechanics.MECHANIC_NONE)
                rtb.AppendFormatLine("Effect Mechanic = {0} ({1})", effect.EffectMechanic, (Mechanics)effect.EffectMechanic);

            rtb.AppendFormatLineIfNotNull("Attributes {0:X8} ({0})", effect.EffectAttributes);
            rtb.AppendLine();
        }
        
        private static void AuraModTypeName(RichTextBox rtb, SpellEffectEntry effect)
        {
            var aura = (AuraType)effect.EffectAura;
            var misc = effect.EffectMiscValues[0];

            if (effect.EffectAura == 0)
            {
                rtb.AppendFormatLineIfNotNull("EffectMiscValueA = {0}", effect.EffectMiscValues[0]);
                rtb.AppendFormatLineIfNotNull("EffectMiscValueB = {0}", effect.EffectMiscValues[1]);
                rtb.AppendFormatLineIfNotNull("EffectAmplitude = {0}", effect.EffectAmplitude);

                return;
            }

            rtb.AppendFormat("Aura Id {0:D} ({0})", aura);
            rtb.AppendFormat(", value = {0}", effect.EffectBasePoints);
            rtb.AppendFormat(", misc = {0} (", misc);

            switch (aura)
            {
                case AuraType.SPELL_AURA_MOD_STAT:
                    rtb.Append((UnitMods)misc);
                    break;
                case AuraType.SPELL_AURA_MOD_RATING:
                case AuraType.SPELL_AURA_MOD_RATING_PCT:
                    rtb.Append((CombatRating)misc);
                    break;
                case AuraType.SPELL_AURA_ADD_FLAT_MODIFIER:
                case AuraType.SPELL_AURA_ADD_PCT_MODIFIER:
                    rtb.Append((SpellModOp)misc);
                    break;
                // TODO: more case
                default:
                    rtb.Append(misc);
                    break;
            }

            rtb.AppendFormat("), miscB = {0}", effect.EffectMiscValues[1]);
            rtb.AppendFormatLine(", amplitude = {0}, periodic = {1}", effect.EffectAmplitude, effect.EffectAuraPeriod);

            switch (aura)
            {
                case AuraType.SPELL_AURA_OVERRIDE_SPELLS:
                    if (!DBC.DBC.OverrideSpellData.ContainsKey(misc))
                    {
                        rtb.SetStyle(Color.Red, FontStyle.Bold);
                        rtb.AppendFormatLine("Cannot find key {0} in OverrideSpellData.dbc", (uint)misc);
                    }
                    else
                    {
                        rtb.AppendLine();
                        rtb.SetStyle(Color.DarkRed, FontStyle.Bold);
                        var @override = DBC.DBC.OverrideSpellData[misc];
                        for (var i = 0; i < 10; ++i)
                        {
                            if (@override.Spells[i] == 0)
                                continue;

                            rtb.SetStyle(Color.DarkBlue, FontStyle.Regular);
                            rtb.AppendFormatLine("\t - #{0} ({1}) {2}", i + 1, @override.Spells[i],
                                DBC.DBC.SpellInfoStore.ContainsKey((int)@override.Spells[i]) ? DBC.DBC.SpellInfoStore[(int)@override.Spells[i]].Name : "?????");
                        }
                        rtb.AppendLine();
                    }
                    break;
                /*case AuraType.SPELL_AURA_SCREEN_EFFECT:
                    rtb.SetStyle(Color.DarkBlue, FontStyle.Bold);
                    rtb.AppendFormatLine("ScreenEffect: {0}",
                        DBC.DBC.ScreenEffect.ContainsKey(misc) ? DBC.DBC.ScreenEffect[misc].Name : "?????");
                    break;*/
            }
        }
        
        private static void AppendItemInfo(this SpellInfo spellInfo, RichTextBox rtb)
        {
            if (!DBC.DBC.ItemEffectStore.ContainsKey(spellInfo.ID))
                return;

            var items = DBC.DBC.ItemEffectStore[spellInfo.ID];

            rtb.AppendLine(Separator);
            rtb.SetStyle(Color.Blue, FontStyle.Bold);
            rtb.AppendLine("Items using this spell:");
            rtb.SetDefaultStyle();

            foreach (var item in items)
            {
                if (!DBC.DBC.ItemSparse.ContainsKey(item.ParentItemID))
                {
                    rtb.AppendFormatLine($@"   Non-existing Item-sparse.db2 entry { item.ParentItemID }");
                    continue;
                }

                var itemTemplate = DBC.DBC.ItemSparse[(int)item.ParentItemID];

                var name = itemTemplate.Name;
                var description = itemTemplate.Description;

                description = string.IsNullOrEmpty(description) ? string.Empty : $" - \"{ description }\"";

                rtb.AppendFormatLine($@"   { item.ParentItemID }: { name } { description }");
            }
        }
        
        private static void AppendSpellVisualInfo(this SpellInfo spellInfo)
        {
            /*SpellVisualEntry visualData;
            if (!DBC.DBC.SpellVisual.TryGetValue(_spell.SpellVisual[0], out visualData))
                return;

            SpellMissileEntry missileEntry;
            SpellMissileMotionEntry missileMotionEntry;
            var hasMissileEntry = DBC.DBC.SpellMissile.TryGetValue(visualData.MissileModel, out missileEntry);
            var hasMissileMotion = DBC.DBC.SpellMissileMotion.TryGetValue(visualData.MissileMotionId, out missileMotionEntry);

            if (!hasMissileEntry && !hasMissileMotion)
                return;

            _rtb.AppendLine(_line);
            _rtb.SetBold();
            _rtb.AppendLine("Missile data");
            _rtb.SetDefaultStyle();

            // Missile Model Data.
            if (hasMissileEntry)
            {
                _rtb.AppendFormatLine("Missile Model ID: {0}", visualData.MissileModel);
                _rtb.AppendFormatLine("Missile attachment: {0}", visualData.MissileAttachment);
                _rtb.AppendFormatLine("Missile cast offset: X:{0} Y:{1} Z:{2}", visualData.MissileCastOffsetX, visualData.MissileCastOffsetY, visualData.MissileCastOffsetZ);
                _rtb.AppendFormatLine("Missile impact offset: X:{0} Y:{1} Z:{2}", visualData.MissileImpactOffsetX, visualData.MissileImpactOffsetY, visualData.MissileImpactOffsetZ);
                _rtb.AppendFormatLine("MissileEntry ID: {0}", missileEntry.Id);
                _rtb.AppendFormatLine("Collision Radius: {0}", missileEntry.CollisionRadius);
                _rtb.AppendFormatLine("Default Pitch: {0} - {1}", missileEntry.DefaultPitchMin, missileEntry.DefaultPitchMax);
                _rtb.AppendFormatLine("Random Pitch: {0} - {1}", missileEntry.RandomizePitchMax, missileEntry.RandomizePitchMax);
                _rtb.AppendFormatLine("Default Speed: {0} - {1}", missileEntry.DefaultSpeedMin, missileEntry.DefaultSpeedMax);
                _rtb.AppendFormatLine("Randomize Speed: {0} - {1}", missileEntry.RandomizeSpeedMin, missileEntry.RandomizeSpeedMax);
                _rtb.AppendFormatLine("Gravity: {0}", missileEntry.Gravity);
                _rtb.AppendFormatLine("Maximum duration:", missileEntry.MaxDuration);
                _rtb.AppendLine("");
            }

            // Missile Motion Data.
            if (hasMissileMotion)
            {
                _rtb.AppendFormatLine("Missile motion: {0}", missileMotionEntry.Name);
                _rtb.AppendFormatLine("Missile count: {0}", missileMotionEntry.MissileCount);
                _rtb.AppendLine("Missile Script body:");
                _rtb.AppendText(missileMotionEntry.Script);
            }*/
        }
    }
}