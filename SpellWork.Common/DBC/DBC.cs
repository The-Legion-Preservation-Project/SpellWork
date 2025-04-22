using SpellWork.DBC.Structures;
using SpellWork.GameTables;
using SpellWork.GameTables.Structures;
using SpellWork.Spell;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public static async Task Load(string dbcPath, string locale, string gtPath)
        {
            var dbcProperties = typeof(DBC).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var dbcPropertiesFiltered = dbcProperties.Where(dbc => dbc.PropertyType.IsGenericType && dbc.PropertyType.GetGenericTypeDefinition() == typeof(DBReader<>));

            Parallel.ForEach(dbcPropertiesFiltered, dbc =>
                {
                    var name = dbc.Name;

                    try
                    {
                        dbc.SetValue(dbc.GetValue(null), Activator.CreateInstance(dbc.PropertyType, dbcPath, locale));
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

            var spellMiscStore = new Dictionary<int, SpellMiscEntry>();
            foreach (var misc in SpellMisc)
            {
                if (misc.Value.DifficultyID == 0) // todo difficulty
                    spellMiscStore.Add(misc.Value.SpellID, misc.Value);
            }

            foreach (var spell in Spell)
            {
                SpellInfoStore[spell.Value.ID] = new SpellInfo(spell.Value);
            }

            foreach (var eff in ItemEffect)
            {
                if (!ItemEffectStore.ContainsKey(eff.Value.SpellID))
                    ItemEffectStore.Add(eff.Value.SpellID, []);

                ItemEffectStore[eff.Value.SpellID].Add(eff.Value);
            }

            List<Action> storeProcessingActions =
            [
                () =>
                {
                    foreach (var effect in SpellInfoStore.Where(effect =>
                                 spellMiscStore.ContainsKey(effect.Value.Spell.ID)))
                    {
                        effect.Value.Misc = spellMiscStore[effect.Value.Spell.ID];

                        if (SpellDuration.TryGetValue(effect.Value.Misc.DurationIndex, out var duration))
                            effect.Value.DurationEntry = duration;

                        if (SpellRange.TryGetValue(effect.Value.Misc.RangeIndex, out var range))
                            effect.Value.Range = range;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellEffect)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"Spell effect {effect.Value.ID} is referencing unknown spell {effect.Value.SpellID}, ignoring!");
                            continue;
                        }

                        spellInfo.Effects.Add(effect.Value);
                        SpellInfoStore[effect.Value.SpellID].SpellEffectInfoStore[effect.Value.EffectIndex] =
                            new SpellEffectInfo(effect.Value); // Helper

                        var triggerId = (int)effect.Value.EffectTriggerSpell;
                        if (triggerId != 0)
                        {
                            if (SpellTriggerStore.TryGetValue(triggerId, out var spellTrigger))
                                spellTrigger.Add(effect.Value.SpellID);
                            else
                                SpellTriggerStore.Add(triggerId, new SortedSet<int> { effect.Value.SpellID });
                        }
                    }
                },

                () =>
                {
                    foreach (var effect in SpellTargetRestrictions)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellTargetRestrictions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.TargetRestrictions.Add(effect.Value);
                    }
                },

                () =>
                {
                    foreach (var effect in SpellXSpellVisual.Where(effect =>
                                 effect.Value.DifficultyID == 0 && effect.Value.PlayerConditionID == 0))
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellXSpellVisual: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.SpellXSpellVisual = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellScaling)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellScaling: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Scaling = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellAuraOptions)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellAuraOptions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.AuraOptions = effect.Value;
                        if (effect.Value.SpellProcsPerMinuteID != 0)
                            SpellInfoStore[effect.Value.SpellID].ProcsPerMinute =
                                SpellProcsPerMinute[effect.Value.SpellProcsPerMinuteID];
                    }
                },

                () =>
                {
                    foreach (var effect in SpellAuraRestrictions)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellAuraRestrictions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.AuraRestrictions = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellCategories)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellCategories: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Categories = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellCastingRequirements)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellCastingRequirements: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            return;
                        }

                        spellInfo.CastingRequirements = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellClassOptions)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellClassOptions: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.ClassOptions = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellCooldowns)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellCooldowns: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Cooldowns = effect.Value;
                    }
                },

                /*
                () =>
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
                },
                 */

                () =>
                {
                    foreach (var effect in SpellInterrupts)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellInterrupts: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Interrupts = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellEquippedItems)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellEquippedItems: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.EquippedItems = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellLevels)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellLevels: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Levels = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var effect in SpellReagents)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellReagents: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Reagents = effect.Value;
                    }
                },

                () =>
                {
                    foreach (var reagentsCurrency in SpellReagentsCurrency)
                    {
                        if (!SpellInfoStore.TryGetValue(reagentsCurrency.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellReagentsCurrency: Unknown spell {reagentsCurrency.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.ReagentsCurrency.Add(reagentsCurrency.Value);
                    }
                },

                () =>
                {
                    foreach (var effect in SpellShapeshift)
                    {
                        if (!SpellInfoStore.TryGetValue(effect.Value.SpellID, out var spellInfo))
                        {
                            Console.WriteLine(
                                $"SpellShapeshift: Unknown spell {effect.Value.SpellID} referenced, ignoring!");
                            continue;
                        }

                        spellInfo.Shapeshift = effect.Value;
                    }
                },

                () =>
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
                }
            ];

            await Task.WhenAll(storeProcessingActions.Select(Task.Run));

            foreach (var spell in SpellInfoStore)
                spell.Value.UpdateAreaRelatedFields();

            GameTable<GtSpellScalingEntry>.Open(Path.Combine(gtPath, "SpellScaling.txt"));
        }


        public static uint SelectedLevel = MaxLevel;
        public static uint SelectedItemLevel = 890;
    }
}
