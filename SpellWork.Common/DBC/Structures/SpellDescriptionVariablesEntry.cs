using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellDescriptionVariables")]
    public sealed class SpellDescriptionVariablesEntry
    {
        [Index]
        public uint ID;
        public string Variables;
    }
}
