using SpellWork.Parser;

namespace SpellWork.DBC.Structures
{
    [DBFileName("SpellEffectScaling")]
    public class SpellEffectScalingEntry
    {
        public float Coefficient;
        public float Variance;
        public float ResourceCoefficient;
        public int SpellEffectId;
    }
}
