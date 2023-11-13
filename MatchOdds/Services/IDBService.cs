using MatchOdds.Models;
using MatchOdds.Resources;

namespace MatchOdds.Services
{
    public interface IDBService
    {
        Task<Match> GetMatchWithId(int? matchId);

        Task<MatchOdd> GetMatchOddWithId(int? id);

        Task<List<Match>> GetMatchList(DateTime? matchDate, string? team, Sport? sport);

        Task<Match> AddMatch(AddMatchRequest newMatch);

        Task<List<MatchOddItem>> AddMatchOdds(AddMatchOddsRequest matchOddsRequest);

        Task<Match> UpdateMatch(int matchId, AddMatchRequest updateMatch);

        Task<MatchOdd> UpdateMatchOdd(UpdateMatchOddRequest updateMatchOdd);

        Task<string> DeleteMatch(int matchId);

        Task<string> DeleteMatchOdd(int matchOddId);
    }
}