using System;
using System.Collections.Generic;

namespace MatchOdds.Models;

public partial class MatchOdd
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string Specifier { get; set; } = null!;

    public float Odd { get; set; }

    public virtual Match Match { get; set; } = null!;
}
