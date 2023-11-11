using MatchOdds.Models;

namespace MatchOdds.Resources
{
    public class AddMatchOddsRequest
    {
        public int MatchId { get; set; }

        public List<MatchOddItem> matchOdds { get; set; } = new List<MatchOddItem>();
    }
}
