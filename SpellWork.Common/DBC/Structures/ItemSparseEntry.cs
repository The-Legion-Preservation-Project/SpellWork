using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("ItemSparse")]
    public sealed class ItemSparseEntry
    {
        [Index]
        public int ID;                                           //     uint32 ID;
        public long AllowableRace;                               //     int64 AllowableRace;
        public string Name;                                      //     LocalizedString* Display;
        public string Name2;                                     //     LocalizedString* Display1;
        public string Name3;                                     //     LocalizedString* Display2;
        public string Name4;                                     //     LocalizedString* Display3;
        public string Description;                               //     LocalizedString* Description;
        [ArraySize(4)]                                           //     
        public uint[] Flags;                                     //     int32 Flags[MAX_ITEM_PROTO_FLAGS];
        public float PriceRandomValue;                           //     float PriceRandomValue;
        public float PriceVariance;                              //     float PriceVariance;
        public uint BuyCount;                                    //     uint32 VendorStackCount;
        public uint BuyPrice;                                    //     uint32 BuyPrice;
        public uint SellPrice;                                   //     uint32 SellPrice;
        public uint RequiredSpell;                               //     uint32 RequiredAbility;
        public int MaxCount;                                    //     int32 MaxCount;
        public int Stackable;                                   //     int32 Stackable;
        [ArraySize(10)]                                          //     
        public int[] ItemStatAllocation;                         //     int32 StatPercentEditor[MAX_ITEM_PROTO_STATS];
        [ArraySize(10)]                                          //    
        public float[] ItemStatSocketCostMultiplier;             //     float StatPercentageOfSocket[MAX_ITEM_PROTO_STATS];
        public float RangedModRange;                             //     float ItemRange;
        public uint BagFamily;                                   //     uint32 BagFamily;
        public float ArmorDamageModifier;                        //     float QualityModifier;
        public uint Duration;                                    //     uint32 DurationInInventory;
        public float StatScalingFactor;                          //     float DmgVariance;
        public short AllowableClass;                            //     int16 AllowableClass;
        public ushort ItemLevel;                                 //     uint16 ItemLevel;
        public ushort RequiredSkill;                             //     uint16 RequiredSkill;
        public ushort RequiredSkillRank;                         //     uint16 RequiredSkillRank;
        public ushort RequiredReputationFaction;                 //     uint16 MinFactionID;
        [ArraySize(10)]                                          //    
        public short[] ItemStatValue;                            //     int16 ItemStatValue[MAX_ITEM_PROTO_STATS];
        public ushort ScalingStatDistribution;                   //     uint16 ScalingStatDistributionID;
        public ushort Delay;                                     //     uint16 ItemDelay;
        public ushort PageText;                                  //     uint16 PageID;
        public ushort StartQuest;                                //     uint16 StartQuestID;
        public ushort LockID;                                    //     uint16 LockID;
        public ushort RandomProperty;                            //     uint16 RandomSelect;
        public ushort RandomSuffix;                              //     uint16 ItemRandomSuffixGroupID;
        public ushort ItemSet;                                   //     uint16 ItemSet;
        public ushort Area;                                      //     uint16 ZoneBound;
        public ushort Map;                                       //     uint16 InstanceBound;
        public ushort TotemCategory;                             //     uint16 TotemCategoryID;
        public ushort SocketBonus;                               //     uint16 SocketMatchEnchantmentId;
        public ushort GemProperties;                             //     uint16 GemProperties;
        public ushort ItemLimitCategory;                         //     uint16 LimitCategory;
        public ushort HolidayID;                                 //     uint16 RequiredHoliday;
        public ushort RequiredTransmogHolidayID;                 //     uint16 RequiredTransmogHoliday;
        public ushort ItemNameDescriptionID;                     //     uint16 ItemNameDescriptionID;
        public byte Quality;                                     //     uint8 OverallQualityID;
        public byte InventoryType;                               //     uint8 InventoryType;
        public sbyte RequiredLevel;                              //     int8 RequiredLevel;
        public byte RequiredHonorRank;                           //     uint8 RequiredPVPRank;
        public byte RequiredCityRank;                            //     uint8 RequiredPVPMedal;
        public byte RequiredReputationRank;                      //     uint8 MinReputation;
        public byte ContainerSlots;                              //     uint8 ContainerSlots;
        [ArraySize(10)]                                          //    
        public sbyte[] ItemStatType;                             //     int8 StatModifierBonusStat[MAX_ITEM_PROTO_STATS];
        public byte DamageType;                                  //     uint8 DamageDamageType;
        public byte Bonding;                                     //     uint8 Bonding;
        public byte LanguageID;                                  //     uint8 LanguageID;
        public byte PageMaterial;                                //     uint8 PageMaterialID;
        public sbyte Material;                                   //     uint8 Material;
        public byte Sheath;                                      //     uint8 SheatheType;
        [ArraySize(3)]                                           //    
        public byte[] SocketColor;                               //     uint8 SocketType[MAX_ITEM_PROTO_SOCKETS];
        public byte SpellWeightCategory;                         //     uint8 SpellWeightCategory;
        public byte SpellWeight;                                 //     uint8 SpellWeight;
        public byte ArtifactID;                                  //     uint8 ArtifactID;
        public byte ExpansionID;                                 //     uint8 ExpansionID;
    }
}
