using System.ComponentModel.DataAnnotations;

namespace MatchOdds.Resources
{
    public class AddMatchRequest
    {
        public string? Description { get; set; }

        public string? MatchDateTime { get; set; }

        public string? TeamA { get; set; } = null!;

        public string? TeamB { get; set; } = null!;

        public int? Sport { get; set; }
    }
}
