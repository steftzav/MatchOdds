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
using Azure;
using System.ComponentModel.DataAnnotations;

namespace MatchOdds.Services
{
    public class DBService : IDBService
    {
        private readonly MatchOddsContext _context;

        public DBService(MatchOddsContext context)
        {
            _context = context;
        }

        public async Task<Match> GetMatchWithId(int? matchId)
        {
            if (!matchId.HasValue || matchId == 0)
            {
                throw new ArgumentNullException("id");
            }
            else if (!_context.Matches.Any(x => x.Id == matchId))
            {
                throw new KeyNotFoundException(Messages.MATCH_NOT_FOUND + matchId.ToString());
            }
            return await _context.Matches.Include(m => m.MatchOdds).FirstOrDefaultAsync(obj => obj.Id == matchId);
        }

        public async Task<MatchOdd> GetMatchOddWithId(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                throw new ArgumentNullException("id");
            }
            else if (!_context.MatchOdds.Any(x => x.Id == id))
            {
                throw new KeyNotFoundException(Messages.MATCHODD_NOT_FOUND + id.ToString());
            }
            return await _context.MatchOdds.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task<List<Match>> GetMatchList(string? matchDate, string? team, Sport? sport)
        {
            DateTime date = default;
            if (!string.IsNullOrWhiteSpace(matchDate))
            {
                date = CheckDateInput(matchDate).Date;
            }
            return await _context.Matches.Include(m => m.MatchOdds).Where(m =>
                (string.IsNullOrWhiteSpace(matchDate) || m.MatchDateTime.Date == date) &&
                (string.IsNullOrWhiteSpace(team) || m.TeamA == team || m.TeamB == team) &&
                (!sport.HasValue || m.Sport == (int)sport)
            ).ToListAsync();
        }

        public async Task<Match> AddMatch(AddMatchRequest newMatch)
        {
            if (string.IsNullOrWhiteSpace(newMatch.MatchDateTime))
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
            else if (!string.IsNullOrWhiteSpace(newMatch.Description) && newMatch.Description.Length > 100)
            {
                throw new Exception(Messages.DESCRIPTION_LENGTH);
            }
            else if ((!string.IsNullOrWhiteSpace(newMatch.TeamA) && newMatch.TeamA.Length > 100) || !string.IsNullOrWhiteSpace(newMatch.TeamB) && newMatch.TeamB.Length > 100)
            {
                throw new Exception(Messages.TEAMNAME_LENGTH);
            }

            var added = new Match()
            {
                Description = newMatch.Description,
                MatchDateTime = CheckDateInput(newMatch.MatchDateTime),
                TeamA = newMatch.TeamA,
                TeamB = newMatch.TeamB,
                Sport = (int)newMatch.Sport
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
            else if (matchOddsRequest.matchOdds.Any(x => !x.Odd.HasValue))
            {
                throw new Exception(Messages.ODD_REQUIRED);
            }
            else if (matchOddsRequest.matchOdds.Any(x => x.Specifier.Length > 20))
            {
                throw new Exception(Messages.SPECIFIER_LENGTH);
            }
            else if (matchOddsRequest.matchOdds.Any(x => x.Odd <1))
            {
                throw new Exception(Messages.MIN_ODD);
            }

            var added = matchOddsRequest.matchOdds.Select(x => new MatchOdd()
            {
                MatchId = matchOddsRequest.MatchId,
                Specifier = x.Specifier,
                Odd = (float)x.Odd
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

        public async Task<Match> UpdateMatch(int matchId, AddMatchRequest updateMatch)
        {
            var existingMatch = await GetMatchWithId(matchId);

            if (!string.IsNullOrWhiteSpace(updateMatch.Description))
            {
                if (updateMatch.Description.Length > 100) 
                    throw new Exception(Messages.DESCRIPTION_LENGTH);

                existingMatch.Description = updateMatch.Description;
            }
            if (!string.IsNullOrWhiteSpace(updateMatch.MatchDateTime))
            {
                existingMatch.MatchDateTime = CheckDateInput(updateMatch.MatchDateTime);
            }
            if (!string.IsNullOrWhiteSpace(updateMatch.TeamA))
            {
                if (!string.IsNullOrWhiteSpace(updateMatch.TeamA) && updateMatch.TeamA.Length > 100)
                    throw new Exception(Messages.TEAMNAME_LENGTH);

                existingMatch.TeamA = updateMatch.TeamA;
            }
            if (!string.IsNullOrWhiteSpace(updateMatch.TeamB))
            {
                if (!string.IsNullOrWhiteSpace(updateMatch.TeamB) && updateMatch.TeamB.Length > 100)
                    throw new Exception(Messages.TEAMNAME_LENGTH);

                existingMatch.TeamB = updateMatch.TeamB;
            }
            if (updateMatch.Sport.HasValue)
            {
                if (updateMatch.Sport != (int)Sport.Football && updateMatch.Sport != (int)Sport.Basketball)
                    throw new Exception(Messages.SPORTS_VALUE);

                existingMatch.Sport = (int)updateMatch.Sport;
            }

            await _context.SaveChangesAsync();
            return existingMatch;
        }

        public async Task<MatchOdd> UpdateMatchOdd(UpdateMatchOddRequest updateMatchOdd)
        {
            var existingMatchOdd = await GetMatchOddWithId(updateMatchOdd.Id);

            if (!string.IsNullOrWhiteSpace(updateMatchOdd.Specifier))
            {
                if (updateMatchOdd.Specifier.Length > 20)
                {
                    throw new Exception(Messages.SPECIFIER_LENGTH);
                }
                existingMatchOdd.Specifier = updateMatchOdd.Specifier;
            }
            if (updateMatchOdd.Odd.HasValue)
            {
                if (updateMatchOdd.Odd < 1)
                {
                    throw new Exception(Messages.MIN_ODD);
                }
                existingMatchOdd.Odd = (float)updateMatchOdd.Odd;
            }

            await _context.SaveChangesAsync();
            return existingMatchOdd;
        }

        public async Task<string> DeleteMatch(int matchId)
        {
            var match = await GetMatchWithId(matchId);

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return Messages.RESPONSE_STATUS_OK;
        }

        public async Task<string> DeleteMatchOdd(int matchOddId)
        {
            var matchOdd = await GetMatchOddWithId(matchOddId);

            _context.MatchOdds.Remove(matchOdd);
            await _context.SaveChangesAsync();

            return Messages.RESPONSE_STATUS_OK;
        }

        private static DateTime CheckDateInput(string date)
        {
            if (DateTime.TryParse(date, out DateTime res)) {
                return res;
            }
            else
            {
                throw new Exception(Messages.INVALID_DATE);
            }
        }
    }
}