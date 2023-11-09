using MatchOdds.Models;
using MatchOdds.Resources;

namespace MatchOdds.Services
{
    public interface IDBService
    {
        Task<Match> GetMatchWithId(int? id);

        Task<List<Match>> GetMatchList(DateTime? matchDate, Sport? sport);
    }
}