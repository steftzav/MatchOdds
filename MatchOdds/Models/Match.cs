 using System;
using System.Collections.Generic;

namespace MatchOdds.Models;

public partial class Match
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public DateTime MatchDateTime { get; set; }

    public string TeamA { get; set; } = null!;

    public string TeamB { get; set; } = null!;

    public int Sport { get; set; }

    public virtual ICollection<MatchOdd> MatchOdds { get; set; } = new List<MatchOdd>();
}
