using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using MatchOdds.Models;
using MatchOdds.Resources;

namespace MatchOdds.Services
{
    public class DBService : IDBService
    {
        private readonly MatchOddsContext _context;

        public DBService(MatchOddsContext context)
        {
            _context = context;
        }

        public async Task<Match> GetMatchWithId(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                throw new ArgumentNullException("id");
            }
            else if (!_context.Matches.Any(x => x.Id == id))
            {
                throw new KeyNotFoundException(Messages.MATCH_NOT_FOUND + id.ToString());
            }
            return await _context.Matches.Include(m => m.MatchOdds).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task<List<Match>> GetMatchList(DateTime? matchDate, string? team, Sport? sport)
        {
            return await _context.Matches.Include(m => m.MatchOdds).Where(m =>
                (matchDate == null || m.MatchDateTime == matchDate) &&
                (string.IsNullOrWhiteSpace(team) || m.TeamA == team || m.TeamB == team) &&
                (!sport.HasValue || m.Sport == (int)sport)
            ).ToListAsync();
        }

        public async Task<Match> AddMatch(AddMatchRequest newMatch)
        {
            if (newMatch.MatchDateTime == default)
            {
                throw new ArgumentNullException("MatchDateTime");
            }
            else if (string.IsNullOrWhiteSpace(newMatch.TeamA) || string.IsNullOrWhiteSpace(newMatch.TeamB))
            {
                throw new Exception(Messages.TEAMS_REQUIRED);
            }
            else if (newMatch.Sport != (int)Sport.Football && newMatch.Sport != (int)Sport.Basketball)
            {
                throw new Exception(Messages.SPORTS_VALUE);
            }

            var added = new Match()
            {
                Description = newMatch.Description,
                MatchDateTime = newMatch.MatchDateTime,
                TeamA = newMatch.TeamA,
                TeamB = newMatch.TeamB,
                Sport = newMatch.Sport
            };
            
            await _context.AddAsync(added);
            await _context.SaveChangesAsync();

            return added;
        }

        public async Task<List<MatchOddItem>> AddMatchOdds(AddMatchOddsRequest matchOddsRequest)
        {
            var match = await GetMatchWithId(matchOddsRequest.MatchId);
            
            if (matchOddsRequest.matchOdds.Any(x => string.IsNullOrWhiteSpace(x.Specifier)))
            {
                throw new Exception(Messages.SPECIFIER_REQUIRED);
            }
            else if (matchOddsRequest.matchOdds.Any(x => x.Odd == 0))
            {
                throw new Exception(Messages.ODD_REQUIRED);
            }

            var added = matchOddsRequest.matchOdds.Select(x => new MatchOdd()
            {
                MatchId = matchOddsRequest.MatchId,
                Specifier = x.Specifier,
                Odd = x.Odd
            }).ToList();

            await _context.AddRangeAsync(added);
            await _context.SaveChangesAsync();

            return added.Select(x => new MatchOddItem() {
                Id = x.Id, 
                MatchId = x.MatchId, 
                Odd = x.Odd, 
                Specifier = x.Specifier 
            }).ToList();
        }
    }
}