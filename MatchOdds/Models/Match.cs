 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatchOdds.Models;

public partial class Match
{
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Description cannot be over 100 characters")]
    public string? Description { get; set; }

    public DateTime MatchDateTime { get; set; }

    [MaxLength(50, ErrorMessage = "Team name cannot be over 50 characters")]
    public string TeamA { get; set; } = null!;

    [MaxLength(50, ErrorMessage = "Team name cannot be over 50 characters")]
    public string TeamB { get; set; } = null!;

    public int Sport { get; set; }

    public virtual ICollection<MatchOdd> MatchOdds { get; set; } = new List<MatchOdd>();
}
