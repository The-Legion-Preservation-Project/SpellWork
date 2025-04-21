using SpellWork.Database;
using SpellWork.DBC.Structures;
using SpellWork.GameTables;
using SpellWork.GameTables.Structures;
using SpellWork.Spell;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SpellWork.Parser;


namespace SpellWork.DBC
{
    public static class DBC
    {
        public const string Version = "SpellWork 7.3.5 (26365)";
        public const uint MaxLevel = 110;

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable CollectionNeverUpdated.Global
        public static DBReader<AreaGroupMemberEntry>             AreaGroupMember { get; set; }
        public static DBReader<AreaTableEntry>                   AreaTable { get; set; }
        public static DBReader<OverrideSpellDataEntry>           OverrideSpellData { get; set; }
        //public static DBReader<ScreenEffectEntry>                ScreenEffect { get; set; }
        public static DBReader<SpellEntry>                       Spell { get; set; }
        public static DBReader<SpellAuraOptionsEntry>            SpellAuraOptions { get; set; }
        public static DBReader<SpellAuraRestrictionsEntry>       SpellAuraRestrictions { get; set; }
        public static DBReader<SpellCastingRequirementsEntry>    SpellCastingRequirements { get; set; }
        public static DBReader<SpellCastTimesEntry>              SpellCastTimes { get; set; }
        public static DBReader<SpellCategoriesEntry>             SpellCategories { get; set; }
        public static DBReader<SpellClassOptionsEntry>           SpellClassOptions { get; set; }
        public static DBReader<SpellCooldownsEntry>              SpellCooldowns { get; set; }
        public static DBReader<SpellDescriptionVariablesEntry>   SpellDescriptionVariables { get; set; }
        public static DBReader<SpellDurationEntry>               SpellDuration { get; set; }
        public static DBReader<SpellEffectEntry>                 SpellEffect { get; set; }
        //public static DBReader<SpellEffectScalingEntry>          SpellEffectScaling { get; set; } // removed
        public static DBReader<SpellMiscEntry>                   SpellMisc { get; set; }
        public static DBReader<SpellEquippedItemsEntry>          SpellEquippedItems { get; set; }
        public static DBReader<SpellInterruptsEntry>             SpellInterrupts { get; set; }
        public static DBReader<SpellLevelsEntry>                 SpellLevels { get; set; }
        public static DBReader<SpellPowerEntry>                  SpellPower { get; set; }
        public static DBReader<SpellRadiusEntry>                 SpellRadius { get; set; }
        public static DBReader<SpellRangeEntry>                  SpellRange { get; set; }
        public static DBReader<SpellScalingEntry>                SpellScaling { get; set; }
        public static DBReader<SpellShapeshiftEntry>             SpellShapeshift { get; set; }
        public static DBReader<SpellTargetRestrictionsEntry>     SpellTargetRestrictions { get; set; }
        //public static DBReader<SpellTotemsEntry>                 SpellTotems { get; set; }
        public static DBReader<SpellXSpellVisualEntry>           SpellXSpellVisual { get; set; }
        public static DBReader<RandPropPointsEntry>              RandPropPoints { get; set; }
        public static DBReader<SpellProcsPerMinuteEntry>         SpellProcsPerMinute { get; set; }

        public static DBReader<SkillLineAbilityEntry>            SkillLineAbility { get; set; }
        public static DBReader<SkillLineEntry>                   SkillLine { get; set; }

        public static DBReader<ItemEntry>                        Item { get; set; }
        public static DBReader<ItemEffectEntry>                  ItemEffect { get; set; }
        public static DBReader<ItemSparseEntry>                  ItemSparse { get; set; }

        public static DBReader<SpellReagentsEntry>               SpellReagents { get; set; }
        public static DBReader<SpellReagentsCurrencyEntry>       SpellReagentsCurrency { get; set; }
        public static DBReader<SpellMissileEntry>                SpellMissile { get; set; }
        public static DBReader<MapEntry> Map { get; set; }
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore CollectionNeverUpdated.Global

        public static DBReader<SpellMissileMotionEntry>         SpellMissileMotion { get; set; }
        //public static DBReader<SpellVisualEntry>                SpellVisual { get; set; }

        public static readonly IDictionary<int, SpellInfo> SpellInfoStore = new ConcurrentDictionary<int, SpellInfo>();
        public static readonly IDictionary<int, ISet<int>> SpellTriggerStore = new Dictionary<int, ISet<int>>();
        public static readonly Dictionary<int, List<ItemEffectEntry>> ItemEffectStore = new Dictionary<int, List<ItemEffectEntry>>();

        public static async void Load()
        {
            Parallel.ForEach(
                    typeof(DBC).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), dbc =>
                    {
                        if (!dbc.PropertyType.IsGenericType ||
                            dbc.PropertyType.GetGenericTypeDefinition() != typeof(DBReader<>))
                            return;
                        Type t = dbc.PropertyType.GetGenericArguments()[0];
                        DBFileNameAttribute nameAttr = (DBFileNameAttribute)t.GetCustomAttribute(typeof(DBFileNameAttribute));
                        if (nameAttr == null)
                            throw new Exception("Missing DBFileName Attribute at " + t);
                        var name = nameAttr.Filename;
                        try
                        {
                            dbc.SetValue(dbc.GetValue(null), Activator.CreateInstance(dbc.PropertyType));
                        }
                        catch (DirectoryNotFoundException)
                        {
                        }
                        catch (TargetInvocationException tie)
                        {
                            if (tie.InnerException is ArgumentException)
                                throw new ArgumentException($"Failed to load {name}.db2: {tie.InnerException.Message}");
                            throw;
                        }
                    });


            Dictionary<int, SpellMiscEntry> SpellMiscStore = new Dictionary<int, SpellMiscEntry>();
            foreach (var misc in SpellMisc)
            {
                if (misc.Value.DifficultyID == 0) // todo difficulty
                    SpellMiscStore.Add(misc.Value.SpellID, misc.Value);
            }

            foreach (var spell in Spell)
            {
                SpellInfoStore[(int)spell.Value.ID] = new SpellInfo(spell.Value);
            }

            foreach (var eff in ItemEffect)
            {
                if (!ItemEffectStore.ContainsKey(eff.Value.SpellID))
                    ItemEffectStore.Add(eff.Value.SpellID, new List<ItemEffectEntry>());
                ItemEffectStore[eff.Value.SpellID].Add(eff.Value);
            }

            await Task.WhenAll(Task.Run(() =>
            {
                foreach (var effect in SpellInfoStore.Where(effect => SpellMiscStore.ContainsKey(effect.Value.Spell.ID)))
                {
                    effect.Value.Misc = SpellMiscStore[effect.Value.Spell.ID];

                    if (SpellDuration.ContainsKey(effect.Value.Misc.DurationIndex))
                        effect.Value.DurationEntry = SpellDuration[effect.Value.Misc.DurationIndex];

                    if (SpellRange.ContainsKey(effect.Value.Misc.RangeIndex))
                        effect.Value.Range = SpellRange[effect.Value.Misc.RangeIndex];
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellEffect)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"Spell effect {effect.Value.ID} is referencing unknown spell {effect.Value.SpellID}, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Effects.Add(effect.Value);
                    SpellInfoStore[effect.Value.SpellID].SpellEffectInfoStore[effect.Value.EffectIndex] = new SpellEffectInfo(effect.Value); // Helper

                    var triggerId = (int)effect.Value.EffectTriggerSpell;
                    if (triggerId != 0)
                    {
                        if (SpellTriggerStore.ContainsKey(triggerId))
                            SpellTriggerStore[triggerId].Add(effect.Value.SpellID);
                        else
                            SpellTriggerStore.Add(triggerId, new SortedSet<int> { effect.Value.SpellID });
                    }
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellTargetRestrictions)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellTargetRestrictions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].TargetRestrictions.Add(effect.Value);
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellXSpellVisual.Where(effect =>
                    effect.Value.DifficultyID == 0 && effect.Value.PlayerConditionID == 0))
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellXSpellVisual: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].SpellXSpellVisual = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellScaling)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellScaling: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Scaling = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellAuraOptions)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellAuraOptions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].AuraOptions = effect.Value;
                    if (effect.Value.SpellProcsPerMinuteID != 0)
                        SpellInfoStore[effect.Value.SpellID].ProcsPerMinute = SpellProcsPerMinute[effect.Value.SpellProcsPerMinuteID];
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellAuraRestrictions)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellAuraRestrictions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].AuraRestrictions = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellCategories)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellCategories: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Categories = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellCastingRequirements)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellCastingRequirements: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        return;
                    }

                    SpellInfoStore[effect.Value.SpellID].CastingRequirements = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellClassOptions)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellClassOptions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].ClassOptions = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellCooldowns)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellCooldowns: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Cooldowns = effect.Value;
                }
            }), /*Task.Run(() =>
            {
                foreach (var effect in SpellEffectScaling)
                {
                    if (!SpellEffect.ContainsKey(effect.Value.SpellEffectId))
                    {
                        Console.WriteLine(
                            $"SpellEffectScaling: Unknown spell effect {effect.Value.SpellEffectId} referenced, ignoring!");
                        continue;
                    }

                    SpellEffect[effect.Value.SpellEffectId].SpellEffectScalingEntry = effect.Value;
                }
            }), */Task.Run(() =>
            {
                foreach (var effect in SpellInterrupts)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellInterrupts: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Interrupts = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellEquippedItems)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellEquippedItems: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].EquippedItems = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellLevels)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine($"SpellLevels: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Levels = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellReagents)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellReagents: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Reagents = effect.Value;
                }
            }), Task.Run(() =>
            {
                foreach (var reagentsCurrency in SpellReagentsCurrency)
                {
                    if (!SpellInfoStore.ContainsKey(reagentsCurrency.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellReagentsCurrency: Unknown spell {reagentsCurrency.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[reagentsCurrency.Value.SpellID].ReagentsCurrency.Add(reagentsCurrency.Value);
                }
            }), Task.Run(() =>
            {
                foreach (var effect in SpellShapeshift)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine(
                            $"SpellShapeshift: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Shapeshift = effect.Value;
                }
            }), Task.Run(() =>
            {
                /*foreach (var effect in SpellTotems)
                {
                    if (!SpellInfoStore.ContainsKey(effect.Value.SpellID))
                    {
                        Console.WriteLine($"SpellTotems: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                        continue;
                    }

                    SpellInfoStore[effect.Value.SpellID].Totems = effect.Value;
                }*/
            }));

            foreach (var spell in SpellInfoStore)
                spell.Value.UpdateAreaRelatedFields();

            GameTable<GtSpellScalingEntry>.Open($@"{ConfigurationManager.AppSettings["GtPath"]}\SpellScaling.txt");
        }

        public static uint SelectedLevel = MaxLevel;
        public static uint SelectedItemLevel = 890;
    }
}
