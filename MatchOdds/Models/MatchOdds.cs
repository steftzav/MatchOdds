using System.Diagnostics.Metrics;

namespace MatchOdds.Models
{
    public partial class MatchOdds
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public string Specifier { get; set; }
        public float Odd { get; set; }

        public virtual Match Match { get; set; }
    }
}
