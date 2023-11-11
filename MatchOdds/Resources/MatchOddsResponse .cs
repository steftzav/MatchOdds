using MatchOdds.Models;

namespace MatchOdds.Resources
{
    public class MatchOddsResponse
    {
        public List<MatchOddItem> MatchOdds { get; set; } = new List<MatchOddItem>();
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
