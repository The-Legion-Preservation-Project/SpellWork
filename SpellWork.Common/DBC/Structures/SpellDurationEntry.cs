using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellDuration")]
    public sealed class SpellDurationEntry
    {
        [Index]
        public int ID;
        public int Duration;
        public int MaxDuration;
        public int DurationPerLevel;
    }
}
