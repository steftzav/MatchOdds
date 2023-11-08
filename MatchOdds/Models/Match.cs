namespace MatchOdds.Models
{
    public partial class Match
    {
        public int Id { get; set; }

        public string? Desription { get; set; }

        public DateTime MatchDate { get; set; }

        public DateTime MatchTime { get; set; }

        public string TeamA { get; set; }

        public string TeamB { get; set; }

        public int Sport { get; set; }

        public virtual ICollection<MatchOdds> MatchOdds { get; set; }
    }
}
