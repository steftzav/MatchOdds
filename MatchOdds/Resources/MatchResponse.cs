using MatchOdds.Models;

namespace MatchOdds.Resources
{
    public class MatchResponse
    {
        public List<Match> Matches { get; set; } = new List<Match>();
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
