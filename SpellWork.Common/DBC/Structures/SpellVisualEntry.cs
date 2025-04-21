using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellVisual")]
    public sealed class SpellVisualEntry
    {
        [Index]
        public uint ID;
        public float MissileCastOffsetX;
        public float MissileCastOffsetY;
        public float MissileCastOffsetZ;
        public float MissileImpactOffsetX;
        public float MissileImpactOffsetY;
        public float MissileImpactOffsetZ;
        public uint UnkMoP1;
        public uint PrecastKit;
        public uint CastingKit;
        public uint ImpactKit;
        public uint StateKit;
        public uint StateDoneKit;
        public uint ChannelKit;
        public uint UnkMoP2;
        public uint MissileModel;
        public uint Flags;
        public uint CasterImpactKit;
        public uint TargetImpactKit;
        public int MissileMotionId;
        public uint MissileTargetingKit;
        public uint UnkMoP3;
        public uint InstantAreaKit;
        public uint ImpactAreaKit;
        public uint PersistentAreaKit;
        public uint UnkCata1;
        public uint Id;
        public uint UnkMoP4;
        public uint UnkMoP5;
        public uint UnkMoP6;
        public uint MissileGroupingId;
    }
}
