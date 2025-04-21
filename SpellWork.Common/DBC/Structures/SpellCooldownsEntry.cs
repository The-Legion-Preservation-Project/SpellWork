using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellCooldowns")]
    public class SpellCooldownsEntry
    {
        [Index]
        public uint ID;
        public uint CategoryRecoveryTime;
        public uint RecoveryTime;
        public uint StartRecoveryTime;
        public byte DifficultyID;
        [RelationField]
        public int SpellID;
    }
}
