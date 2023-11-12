using System.ComponentModel.DataAnnotations;

namespace MatchOdds.Resources
{
    public class UpdateMatchOddRequest
    {
        public int Id { get; set; }

        public string? Specifier { get; set; }

        public float? Odd { get; set; }
    }
}
