using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatchOdds.Models;

public partial class MatchOdd
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    [MaxLength(20, ErrorMessage = "Specifier cannot be over 50 characters")]
    public string Specifier { get; set; } = null!;

    [Range(1, float.MaxValue, ErrorMessage = "Please enter the correct value")]
    public float Odd { get; set; }

    public virtual Match Match { get; set; } = null!;
}
