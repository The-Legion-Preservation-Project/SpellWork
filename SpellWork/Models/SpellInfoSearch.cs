using SpellWork.Spell;

namespace SpellWork.Models;

public class SpellInfoSearch
{
    public string IdOrName { get; set; }

    public SpellFamilyNames? Family { get; set; }

    public AuraType? Aura { get; set; }

    public SpellEffects? Effect { get; set; }

    public bool HasAnyFilter() =>
        !string.IsNullOrWhiteSpace(IdOrName) || Family.HasValue || Aura.HasValue || Effect.HasValue;
}