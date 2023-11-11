using MatchOdds.Models;
using MatchOdds.Resources;

namespace MatchOdds.Services
{
    public interface IDBService
    {
        Task<Match> GetMatchWithId(int? id);

        Task<List<Match>> GetMatchList(DateTime? matchDate, string? team, Sport? sport);

        Task<Match> AddMatch(AddMatchRequest newMatch);

        Task<List<MatchOddItem>> AddMatchOdds(AddMatchOddsRequest matchOddsRequest);
    }
}