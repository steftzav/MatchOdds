using MatchOdds.Enums;
using MatchOdds.Models;

namespace MatchOdds.Services
{
    public interface IDBService
    {
        Task<Match> GetMatchWithId(int? id);

        Task<List<Match>> GetMatchList(DateTime? matchDate, Sport? sport);
    }
}