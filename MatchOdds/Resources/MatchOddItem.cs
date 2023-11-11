namespace MatchOdds.Resources
{
    public class MatchOddItem
    {
        public int Id { get; set; }
        public int MatchId { get; set; }

        public string Specifier { get; set; } = null!;

        public float Odd { get; set; }
    }
}
