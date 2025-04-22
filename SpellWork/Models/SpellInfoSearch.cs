using System.ComponentModel.DataAnnotations;

namespace SpellWork.Models;

public class SpellInfoSearch
{
    [Required]
    public string IdOrName { get; set; }
}