using SpellWork.Spell;

namespace SpellWork.Models;

public class SpellInfoSearchResult
{
    public SpellInfoSearchResult(SpellInfo spellInfo)
    {
        SpellId = spellInfo.ID;
        SpellName = spellInfo.Name;
        SpellMiscId = spellInfo.MiscID;
    }

    public int SpellId { get; set; }
    public string SpellName { get; set; }
    public uint SpellMiscId { get; set; }
}